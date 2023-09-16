using Kysect.CommonLib.BaseTypes.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Kysect.PowerShellRunner.Tests.CodeGeneration.Tools;

public static class RoslynParsingExtensions
{
    public static T ExtractSyntax<T>(string source) where T : CSharpSyntaxNode
    {
        return CSharpSyntaxTree
            .ParseText(source)
            .GetRoot()
            .DescendantNodes()
            .OfType<T>()
            .Single();
    }

    public static CSharpCompilation CreateCompilation(SyntaxNode syntaxNode)
    {
        syntaxNode.ThrowIfNull();

        return CSharpCompilation
            .Create("TestCompilation")
            .AddSyntaxTrees(syntaxNode.SyntaxTree);
    }
}