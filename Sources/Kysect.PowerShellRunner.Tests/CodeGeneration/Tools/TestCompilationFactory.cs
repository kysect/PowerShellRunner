using Kysect.PowerShellRunner.CodeGeneration.Compilation;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;

namespace Kysect.PowerShellRunner.Tests.CodeGeneration.Tools;

public static class TestCompilationFactory
{
    public static CSharpCompilation CreateCompilation(SyntaxTree syntaxTree)
    {
        CSharpCompilation compilation = SharpCompilationProviderBuilder
            .CreateForStandard("Kysect.PowerShellRunner.Tests.CodeGeneration")
            .Build()
            .AddSyntaxTrees(syntaxTree);

        SemanticModel sm = compilation.GetSemanticModel(syntaxTree, true);

        foreach (Diagnostic diagnostic in sm.GetDiagnostics())
            Assert.Fail(diagnostic.GetMessage());

        return compilation;
    }
}