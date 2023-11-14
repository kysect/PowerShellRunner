using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.CommonLib.Collections.Extensions;
using Kysect.PowerShellRunner.CodeGeneration.Common;
using Kysect.PowerShellRunner.CodeGeneration.Common.Semantic;
using Kysect.PowerShellRunner.CodeGeneration.Compilation;
using Kysect.PowerShellRunner.CodeGeneration.SemanticParsing;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Kysect.PowerShellRunner.CodeGeneration.UsedModelSearching;

public class RoslynDependentTypeSearcher
{
    private readonly EnumDeclarationParser _enumDeclarationParser;
    private readonly ILogger _logger;
    private readonly RoslynSimpleModelSemanticDescriptorFactory _semanticDescriptorFactory;

    public RoslynDependentTypeSearcher(ILogger logger, RoslynSimpleModelSemanticDescriptorFactory simpleModelSemanticDescriptorFactory)
    {
        _logger = logger;
        _semanticDescriptorFactory = simpleModelSemanticDescriptorFactory;

        _enumDeclarationParser = new EnumDeclarationParser(logger);
    }

    public RoslynDependentTypeSearcherResult LoadAllDependentTypes(SolutionCompilationContext solutionCompilationContext, IReadOnlyCollection<RoslynTypeSymbolWrapper> typeSemanticDescriptors)
    {
        solutionCompilationContext.ThrowIfNull();

        var allDefinedTypes = solutionCompilationContext
            .Items
            .Select(i => i.Symbol.GetNameWithContainingParent())
            .ToImmutableHashSet();

        var rootTypes = typeSemanticDescriptors
            .DistinctByForLegacy(t => t.GetNameWithContainingParent())
            .OrderBy(t => t.GetNameWithContainingParent())
            .ToList();

        var processingQueue = new Queue<RoslynTypeSymbolWrapper>();
        foreach (RoslynTypeSymbolWrapper roslynTypeSemanticDescriptor in rootTypes)
        {
            AddToQueue(processingQueue, roslynTypeSemanticDescriptor);
        }

        var processedTypes = new HashSet<string>();
        var models = new List<RoslynSimpleModelSemanticDescriptor>();
        var enums = new List<ModelEnumTypeDescriptor>();

        while (processingQueue.Any())
        {
            RoslynTypeSymbolWrapper typeDescriptor = processingQueue.Dequeue();
            ITypeSymbol typeSymbol = typeDescriptor.RoslynSymbol;

            if (typeSymbol.IsUndefinedType())
                throw new RoslynAnalyzingException("Found unresolved type while searching usages.");

            if (!processedTypes.Add(typeSymbol.GetNameWithContainingParent()))
                continue;

            if (typeDescriptor.TryUnwrapInnerType(out ITypeSymbol? unwrapped))
            {
                AddToQueue(processingQueue, new RoslynTypeSymbolWrapper(unwrapped));
            }

            if (typeDescriptor.TryUnwrapGenerics(out IReadOnlyCollection<ITypeSymbol> unwrappedGenericSymbols))
            {
                foreach (ITypeSymbol unwrappedGenericSymbol in unwrappedGenericSymbols)
                {
                    AddToQueue(processingQueue, new RoslynTypeSymbolWrapper(unwrappedGenericSymbol));
                }
            }

            if (!allDefinedTypes.Contains(typeDescriptor.GetNameWithContainingParent()))
            {
                _logger.LogWarning($"Skip type {typeDescriptor.GetNameWithContainingParent()} because this type is not defied in analyzed assemblies.");
                continue;
            }

            if (typeSymbol.IsObjectType() || typeSymbol.IsEnumType())
                continue;

            if (typeSymbol.BaseType is not null)
            {
                AddToQueue(processingQueue, new RoslynTypeSymbolWrapper(typeSymbol.BaseType));
            }

            if (typeSymbol.BaseType?.IsEnumType() ?? false)
            {
                enums.Add(ParseAsEnum(solutionCompilationContext, typeSymbol));
                continue;
            }

            RoslynSimpleModelSemanticDescriptor roslynSimpleModelSemanticDescriptor = _semanticDescriptorFactory.Create(typeSymbol);
            models.Add(roslynSimpleModelSemanticDescriptor);

            if (roslynSimpleModelSemanticDescriptor.BaseType is not null)
            {
                AddToQueue(processingQueue, roslynSimpleModelSemanticDescriptor.BaseType);
            }

            foreach (RoslynPropertySymbolWrapper roslynPropertySemanticDescriptor in roslynSimpleModelSemanticDescriptor.PublicProperties)
            {
                AddToQueue(processingQueue, roslynPropertySemanticDescriptor.Type);
            }
        }

        return new RoslynDependentTypeSearcherResult(models, enums);
    }

    private ModelEnumTypeDescriptor ParseAsEnum(SolutionCompilationContext solutionCompilationContext, ITypeSymbol typeSymbol)
    {
        EnumDeclarationSyntax? enumDeclarationSyntaxNode = solutionCompilationContext
            .TryGetTypeSymbolDeclaration(typeSymbol)
            ?.To<EnumDeclarationSyntax>();

        if (enumDeclarationSyntaxNode is null)
            throw new RoslynAnalyzingException("Cannot get type symbol declaration for " + typeSymbol.Name);

        return _enumDeclarationParser.ParseEnum(enumDeclarationSyntaxNode);
    }

    private void AddToQueue(Queue<RoslynTypeSymbolWrapper> processingQueue, RoslynTypeSymbolWrapper type)
    {
        if (type.RoslynSymbol.IsUndefinedType())
            throw new RoslynAnalyzingException("Found unresolved type while searching usages.");

        processingQueue.Enqueue(type);
    }
}