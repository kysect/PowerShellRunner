using Kysect.CommonLib.BaseTypes.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kysect.PowerShellRunner.Tests.CodeGeneration.Tools;

public class SyntaxTreeTestFacade
{
    public SyntaxTree SyntaxTree { get; }
    public CSharpCompilation Compilation { get; }

    public SyntaxTreeTestFacade(SyntaxTree syntaxTree, CSharpCompilation compilation)
    {
        SyntaxTree = syntaxTree.ThrowIfNull();
        Compilation = compilation.ThrowIfNull();
    }

    public BaseTypeDeclarationSyntax GetTypeSyntax(string name)
    {
        CompilationUnitSyntax root = SyntaxTree.GetRoot().To<CompilationUnitSyntax>();
        return root.DescendantNodes().OfType<BaseTypeDeclarationSyntax>().Single(i => i.Identifier.Text == name);
    }

    public INamedTypeSymbol GetTypeSymbol(string name)
    {
        CompilationUnitSyntax root = SyntaxTree.GetRoot().To<CompilationUnitSyntax>();
        SemanticModel sm = Compilation.GetSemanticModel(SyntaxTree, true);

        BaseTypeDeclarationSyntax classC = root.DescendantNodes().OfType<BaseTypeDeclarationSyntax>().Single(i => i.Identifier.Text == name);
        INamedTypeSymbol? classSymbol = sm.GetDeclaredSymbol(classC);

        return classSymbol.ThrowIfNull();
    }
}