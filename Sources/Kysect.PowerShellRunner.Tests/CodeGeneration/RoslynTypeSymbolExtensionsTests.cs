using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Kysect.PowerShellRunner.CodeGeneration.Common;
using Kysect.PowerShellRunner.Tests.CodeGeneration.Tools;

namespace Kysect.PowerShellRunner.Tests.CodeGeneration;

public class RoslynTypeSymbolExtensionsTests
{
    [Test]
    public void GetBaseTypes_ShouldReturnTransitiveInheritance()
    {
        string input = """
                        public class A { }
                        public class B : A { }
                        public class C : B { }
                        """;
        string[] expected = { "B", "A", "Object" };


        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(input);
        CSharpCompilation compilation = TestCompilationFactory.CreateCompilation(syntaxTree);
        var syntaxTreeTestFacade = new SyntaxTreeTestFacade(syntaxTree, compilation);

        INamedTypeSymbol classSymbol = syntaxTreeTestFacade.GetTypeSymbol("C");

        IReadOnlyCollection<string> baseTypes = RoslynTypeSymbolExtensions
            .GetBaseTypes(classSymbol)
            .Select(t => t.Name)
            .ToList();

        baseTypes.Should().BeEquivalentTo(expected);
    }
}