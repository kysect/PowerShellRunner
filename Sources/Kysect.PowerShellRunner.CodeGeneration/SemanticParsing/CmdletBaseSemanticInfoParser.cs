using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.CommonLib.Collections.Extensions;
using Kysect.CommonLib.Logging;
using Kysect.PowerShellRunner.CodeGeneration.Common;
using Kysect.PowerShellRunner.CodeGeneration.Common.Semantic;
using Kysect.PowerShellRunner.CodeGeneration.Compilation;
using Kysect.PowerShellRunner.CodeGeneration.SyntaxParsing;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Kysect.PowerShellRunner.CodeGeneration.SemanticParsing;

public class CmdletBaseSemanticInfoParser
{
    private readonly SolutionCompilationTypeInheritancesSearcher _typeInheritancesSearcher;
    private readonly CmdletWriteObjectMethodParser _writeObjectMethodParser;
    private readonly CmdletAttributeValueParser _attributeValueParser;
    private readonly ILogger _logger;

    public CmdletBaseSemanticInfoParser(
        SolutionCompilationTypeInheritancesSearcher typeInheritancesSearcher,
        ILogger logger)
    {
        _typeInheritancesSearcher = typeInheritancesSearcher;
        _logger = logger;

        _writeObjectMethodParser = new CmdletWriteObjectMethodParser(logger);
        _attributeValueParser = new CmdletAttributeValueParser(logger);
    }

    public CmdletBaseSemanticInfo Parse(CmdletBaseSyntaxInfo syntaxParseResult)
    {
        syntaxParseResult.ThrowIfNull();

        SolutionCompilationContextItem compilationContextItem = syntaxParseResult.SolutionCompilationContextItem;
        SemanticModel semanticModel = syntaxParseResult.SolutionCompilationContextItem.SemanticModel;

        CmdletAttributeValues cmdletAttributeValues = _attributeValueParser.ParsePowerShellNameAttribute(semanticModel, syntaxParseResult.CmdletAttribute);
        IReadOnlyCollection<RoslynPropertySymbolWrapper> properties = GetPropertySemanticDescriptors(syntaxParseResult, semanticModel);
        IReadOnlyCollection<RoslynTypeSymbolWrapper> originalReturnType = GetOriginalReturnType(compilationContextItem, syntaxParseResult);
        IReadOnlyCollection<RoslynTypeSymbolWrapper> resolvedReturnTypes = GetReturnTypeInheritances(compilationContextItem, originalReturnType);
        RoslynTypeSymbolWrapper? mainReturnType = SelectMainReturnType(syntaxParseResult, originalReturnType);

        return new CmdletBaseSemanticInfo(
            syntaxParseResult.SolutionCompilationContextItem,
            cmdletAttributeValues,
            properties,
            mainReturnType,
            resolvedReturnTypes);
    }

    private IReadOnlyCollection<RoslynPropertySymbolWrapper> GetPropertySemanticDescriptors(CmdletBaseSyntaxInfo syntaxParseResult, SemanticModel semanticModel)
    {
        var result = new List<RoslynPropertySymbolWrapper>();

        foreach (PropertyDeclarationSyntax propertyDeclarationSyntax in syntaxParseResult.ParameterProperties)
        {
            IPropertySymbol? propertySymbol = semanticModel.GetDeclaredSymbol(propertyDeclarationSyntax).To<IPropertySymbol>();
            if (propertySymbol is null)
                throw new RoslynAnalyzingException($"Cannot extract property type for {propertyDeclarationSyntax.ToFullString()}");

            result.Add(new RoslynPropertySymbolWrapper(propertySymbol));
        }

        return result;
    }

    private IReadOnlyCollection<RoslynTypeSymbolWrapper> GetOriginalReturnType(
        SolutionCompilationContextItem compilationContextItem,
        CmdletBaseSyntaxInfo syntaxParseResult)
    {
        var result = new List<ITypeSymbol>();

        foreach (InvocationExpressionSyntax invocationExpressionSyntax in syntaxParseResult.WriteObjectMethodInvocations)
        {
            if (_writeObjectMethodParser.TryParseFirstArgumentType(compilationContextItem, invocationExpressionSyntax, out ITypeSymbol? typeSymbol)
                && typeSymbol is not null
                && !typeSymbol.IsUndefinedType())
            {
                result.Add(typeSymbol);
            }
            else
            {
                throw new RoslynAnalyzingException("Cannot parse return type for Cmdlet " + syntaxParseResult.SolutionCompilationContextItem.Symbol.Name);
            }
        }

        return result
            .DistinctByForLegacy(t => t.ToDisplayString())
            .Select(t => new RoslynTypeSymbolWrapper(t))
            .ToList();
    }

    private IReadOnlyCollection<RoslynTypeSymbolWrapper> GetReturnTypeInheritances(
        SolutionCompilationContextItem compilationContextItem,
        IReadOnlyCollection<RoslynTypeSymbolWrapper> originalReturnTypes)
    {
        var result = new List<ITypeSymbol>();

        foreach (RoslynTypeSymbolWrapper originalReturnType in originalReturnTypes)
        {
            if (originalReturnType.RoslynSymbol.IsObjectType())
            {
                _logger.WithPrefix(compilationContextItem.Symbol.Name).LogInformation("Skip 'object' return type");
                continue;
            }

            foreach (INamedTypeSymbol namedTypeSymbol in _typeInheritancesSearcher.GetAllInheritances(originalReturnType.RoslynSymbol))
            {
                if (originalReturnTypes.All(t => SymbolEqualityComparer.Default.Equals(t.RoslynSymbol, namedTypeSymbol)))
                    result.Add(namedTypeSymbol);
            }
        }

        return result
            .DistinctByForLegacy(t => t.ToDisplayString())
            .Select(t => new RoslynTypeSymbolWrapper(t))
            .ToList();
    }

    private RoslynTypeSymbolWrapper? SelectMainReturnType(CmdletBaseSyntaxInfo syntaxParseResult, IReadOnlyCollection<RoslynTypeSymbolWrapper> returnTypes)
    {
        // TODO: rework
        if (returnTypes.Count > 1)
            _logger.WithPrefix($"{syntaxParseResult.SolutionCompilationContextItem.Symbol.Name}").LogWarning($"Found {returnTypes.Count} return types.");

        return returnTypes.SingleOrDefault();
    }
}