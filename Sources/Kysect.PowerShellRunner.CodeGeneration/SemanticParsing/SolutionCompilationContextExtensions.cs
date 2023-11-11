using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.PowerShellRunner.CodeGeneration.Compilation;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Kysect.PowerShellRunner.CodeGeneration.SemanticParsing;

public static class SolutionCompilationContextExtensions
{
    public static bool TryGetVariableDeclaration(
        SolutionCompilationContextItem compilationContextItem,
        IdentifierNameSyntax identifierNameSyntax,
        [NotNullWhen(true)] out SyntaxNode? typeInfo)
    {
        compilationContextItem.ThrowIfNull();

        SymbolInfo symbolInfo = compilationContextItem.SemanticModel.GetSymbolInfo(identifierNameSyntax);

        if (symbolInfo.Symbol == null)
        {
            typeInfo = null;
            return false;
        }

        if (symbolInfo.Symbol.DeclaringSyntaxReferences.Length != 1)
        {
            typeInfo = null;
            return false;
        }

        SyntaxReference variableDeclaration = symbolInfo.Symbol.DeclaringSyntaxReferences.Single();
        typeInfo = compilationContextItem.Syntax.FindNode(variableDeclaration.Span);
        return true;
    }

    public static SyntaxNode? TryGetTypeSymbolDeclaration(this SolutionCompilationContext solutionCompilationContext, ITypeSymbol typeSymbol)
    {
        solutionCompilationContext.ThrowIfNull();
        typeSymbol.ThrowIfNull();

        // KB: full string for prevent collision
        SolutionCompilationContextItem? solutionCompilationContextItem = solutionCompilationContext
            .Items
            .FirstOrDefault(s => s.Symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) == typeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));

        if (solutionCompilationContextItem is null)
            return null;

        SyntaxReference syntaxReference = typeSymbol.DeclaringSyntaxReferences.Single();
        SyntaxNode syntaxNode = solutionCompilationContextItem
            .Syntax
            .FindNode(syntaxReference.Span);

        return syntaxNode;
    }
}