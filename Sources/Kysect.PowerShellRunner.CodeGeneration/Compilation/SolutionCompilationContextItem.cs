using Kysect.CommonLib.BaseTypes.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kysect.PowerShellRunner.CodeGeneration.Compilation;

public class SolutionCompilationContextItem
{
    public SemanticModel SemanticModel { get; }
    public BaseTypeDeclarationSyntax Syntax { get; }
    public INamedTypeSymbol Symbol { get; }

    public SolutionCompilationContextItem(SemanticModel semanticModel, BaseTypeDeclarationSyntax syntax, INamedTypeSymbol symbol)
    {
        SemanticModel = semanticModel.ThrowIfNull();
        Syntax = syntax.ThrowIfNull();
        Symbol = symbol.ThrowIfNull();
    }
}