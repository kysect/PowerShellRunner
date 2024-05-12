using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.CommonLib.Logging;
using Kysect.PowerShellRunner.CodeGeneration.Common;
using Kysect.PowerShellRunner.CodeGeneration.Common.Semantic;
using Kysect.PowerShellRunner.CodeGeneration.Compilation;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Kysect.PowerShellRunner.CodeGeneration.SemanticParsing;

public class CmdletWriteObjectMethodParser
{
    private readonly ILogger _logger;

    public CmdletWriteObjectMethodParser(ILogger logger)
    {
        _logger = logger;
    }

    public bool TryParseFirstArgumentType(
        SolutionCompilationContextItem compilationContextItem,
        InvocationExpressionSyntax invocationExpressionSyntax,
        out ITypeSymbol? result)
    {
        compilationContextItem.ThrowIfNull();
        invocationExpressionSyntax.ThrowIfNull();

        ILogger logger = _logger.WithPrefix(compilationContextItem.Symbol.Name);

        ExpressionSyntax expressionForParsing = GetFirstArgument(invocationExpressionSyntax);
        if (TryParseFirstArgumentTypeInner(compilationContextItem, expressionForParsing, out result))
        {
            result = UnwrapCollectionType(result, logger);
            return true;
        }

        logger.LogError($"Failed to resolve argument type from " + expressionForParsing.ToFullString());
        result = default;
        return false;
    }

    private ExpressionSyntax GetFirstArgument(InvocationExpressionSyntax invocationExpressionSyntax)
    {
        SeparatedSyntaxList<ArgumentSyntax> arguments = invocationExpressionSyntax.ArgumentList.Arguments;

        if (arguments.Count == 0)
            throw new RoslynAnalyzingException($"WriteObject contains 0 arguments.");

        ArgumentSyntax argumentSyntax = arguments.First();
        return argumentSyntax.Expression;
    }

    private bool TryParseFirstArgumentTypeInner(
        SolutionCompilationContextItem compilationContextItem,
        ExpressionSyntax expressionForParsing,
        [NotNullWhen(true)] out ITypeSymbol? result)
    {
        ILogger logger = _logger.WithPrefix(compilationContextItem.Symbol.Name);

        expressionForParsing = RemoveToPipelineIfNeed(expressionForParsing);
        TypeInfo typeInfo = compilationContextItem.SemanticModel.GetTypeInfo(expressionForParsing);

        if (typeInfo.Type == null || typeInfo.Type.IsUndefinedType())
        {
            if (TryUnwrapTypeFromLinqMethod(compilationContextItem, expressionForParsing, out ITypeSymbol? unwrappedSelect))
            {
                result = unwrappedSelect;
                return true;
            }

            if (expressionForParsing is IdentifierNameSyntax identifierNameSyntax)
            {
                if (TryGetVariableInitializationNode(
                        compilationContextItem,
                        identifierNameSyntax,
                        logger,
                        out ExpressionSyntax? variableInitializer))
                {
                    if (TryParseFirstArgumentTypeInner(compilationContextItem, variableInitializer, out ITypeSymbol? initializerType))
                    {
                        result = initializerType;
                        return true;
                    }
                }
            }

            logger.LogError($"Failed to resolve argument type from " + expressionForParsing.ToFullString());
            result = default;
            return false;
        }

        if (typeInfo.Type.IsObjectType())
        {
            if (TryGetTypeFromPropertyDeclaration(compilationContextItem, expressionForParsing, out ITypeSymbol? propertyType))
            {
                result = propertyType;
                return true;
            }

            if (TryGetTypeFromVariableInitialization(compilationContextItem, expressionForParsing, logger, out ExpressionSyntax? unwrappedSyntax))
            {
                if (TryParseFirstArgumentTypeInner(compilationContextItem, unwrappedSyntax, out ITypeSymbol? initializerType))
                {
                    result = initializerType;
                    return true;
                }
            }

            logger.LogWarning("Cannot get original type from Object: " + expressionForParsing.ToFullString());
            result = typeInfo.Type;
            return true;
        }

        result = UnwrapCollectionType(typeInfo.Type, logger);
        return true;
    }

    private ITypeSymbol UnwrapCollectionType(ITypeSymbol input, ILogger logger)
    {
        ITypeSymbol? typeFromCollection = TryUnwrapTypeFromCollection(input, logger);
        if (typeFromCollection != null)
            return typeFromCollection;

        return input;
    }

    private ITypeSymbol? TryUnwrapTypeFromCollection(ITypeSymbol typeInfo, ILogger logger)
    {
        if (typeInfo is IArrayTypeSymbol arrayTypeSymbol)
        {
            return arrayTypeSymbol.ElementType;
        }

        if (typeInfo is not INamedTypeSymbol namedTypeSymbol)
        {
            logger.LogWarning($"Contains method with non-named argument: {typeInfo.ToDisplayString()}.");
            return null;
        }

        if (!namedTypeSymbol.IsGenericType)
        {
            //logger.LogWarning($"Contains method {WriteObjectMethodName} with non generic argument: {typeInfo.Type.ToDisplayString()}.");
            return null;
        }

        if (namedTypeSymbol.TypeArguments.Length != 1)
        {
            logger.LogWarning($"Contains method with {namedTypeSymbol.TypeArguments.Length} generic argument: {typeInfo.Name}.");
            return null;
        }

        return namedTypeSymbol.TypeArguments.Single();
    }

    private bool TryGetTypeFromVariableInitialization(
        SolutionCompilationContextItem compilationContextItem,
        ExpressionSyntax inputExpression,
        ILogger logger,
        [NotNullWhen(true)] out ExpressionSyntax? typeInfo)
    {
        if (inputExpression is IdentifierNameSyntax identifierNameSyntax)
        {
            if (TryGetVariableInitializationNode(
                    compilationContextItem,
                    identifierNameSyntax,
                    logger,
                    out ExpressionSyntax? variableInitializer))
            {
                typeInfo = RemoveToPipelineIfNeed(variableInitializer);
                return true;
            }
        }

        typeInfo = null;
        return false;
    }

    private bool IsVariableReferenceToProperty(
        SolutionCompilationContextItem compilationContextItem,
        IdentifierNameSyntax identifierNameSyntax,
        [NotNullWhen(true)] out PropertyDeclarationSyntax? propertyDeclarationSyntax)
    {
        if (SolutionCompilationContextExtensions.TryGetVariableDeclaration(
                compilationContextItem,
                identifierNameSyntax,
                out SyntaxNode? declarationNode))
        {
            propertyDeclarationSyntax = declarationNode as PropertyDeclarationSyntax;
            return propertyDeclarationSyntax is not null;
        }

        propertyDeclarationSyntax = null;
        return false;
    }

    private bool TryGetTypeFromPropertyDeclaration(
        SolutionCompilationContextItem compilationContextItem,
        ExpressionSyntax inputExpression,
        [NotNullWhen(true)] out ITypeSymbol? result)
    {
        if (inputExpression is IdentifierNameSyntax identifierNameSyntax)
        {
            if (IsVariableReferenceToProperty(
                    compilationContextItem,
                    identifierNameSyntax,
                    out PropertyDeclarationSyntax? propertyDeclarationSyntax))
            {
                TypeInfo typeInfo = compilationContextItem.SemanticModel.GetTypeInfo(propertyDeclarationSyntax);
                if (typeInfo.Type is null || typeInfo.Type.IsUndefinedType())
                    throw new RoslynAnalyzingException("Cannot get type from " + propertyDeclarationSyntax.ToFullString());

                result = typeInfo.Type;
                return true;
            }
        }

        result = null;
        return false;
    }

    private bool TryGetVariableInitializationNode(
        SolutionCompilationContextItem compilationContextItem,
        IdentifierNameSyntax identifierNameSyntax,
        ILogger logger,
        [NotNullWhen(true)] out ExpressionSyntax? variableInitializer)
    {
        if (SolutionCompilationContextExtensions.TryGetVariableDeclaration(
                compilationContextItem,
                identifierNameSyntax,
                out SyntaxNode? declarationNode))
        {
            if (declarationNode is VariableDeclaratorSyntax variableDeclarationSyntax)
            {
                if (variableDeclarationSyntax.Initializer is not null)
                {
                    variableInitializer = variableDeclarationSyntax.Initializer.Value;
                    return true;
                }
                else
                {
                    logger.LogWarning($"Variable declarator is null for {variableDeclarationSyntax.ToFullString()}");
                    variableInitializer = null;
                    return false;
                }
            }
        }

        logger.LogWarning($"Declaration is not variable declaration. {identifierNameSyntax.ToFullString()}");
        variableInitializer = null;
        return false;
    }

    // TODO: this is hack because of bad LINQ semantic parsing
    private bool TryUnwrapTypeFromLinqMethod(
        SolutionCompilationContextItem compilationContextItem,
        ExpressionSyntax inputExpression,
        [NotNullWhen(true)] out ITypeSymbol? result)
    {
        if (inputExpression is not InvocationExpressionSyntax invocationExpressionSyntax)
        {
            result = null;
            return false;
        }

        if (invocationExpressionSyntax.Expression is not MemberAccessExpressionSyntax memberAccessExpressionSyntax)
        {
            result = null;
            return false;
        }

        var linqMethods = new HashSet<string>()
        {
            "Select",
            "SelectMany",
            "Where",
            "DistinctBy"
        };

        if (!linqMethods.Contains(memberAccessExpressionSyntax.Name.Identifier.Text))
        {
            result = null;
            return false;
        }

        ArgumentSyntax selectLambda = invocationExpressionSyntax.ArgumentList.Arguments.First();
        if (selectLambda.Expression is not SimpleLambdaExpressionSyntax simpleLambdaExpressionSyntax || simpleLambdaExpressionSyntax.ExpressionBody is null)
        {
            result = null;
            return false;
        }

        TypeInfo typeInfo = compilationContextItem.SemanticModel.GetTypeInfo(simpleLambdaExpressionSyntax.ExpressionBody);
        if (typeInfo.Type is null || typeInfo.Type.IsUndefinedType())
        {
            result = null;
            return false;
        }

        result = typeInfo.Type;
        return true;
    }

    public ExpressionSyntax RemoveToPipelineIfNeed(ExpressionSyntax inputExpression)
    {
        if (inputExpression is InvocationExpressionSyntax invocationExpressionSyntax)
        {
            var roslynMethodInvocationInfo = RoslynMethodInvocationInfo.TryGet(invocationExpressionSyntax);
            if (roslynMethodInvocationInfo is not null && roslynMethodInvocationInfo.MethodName == "ToPipeline")
            {
                return roslynMethodInvocationInfo.CalledValue;
            }
        }

        return inputExpression;
    }
}