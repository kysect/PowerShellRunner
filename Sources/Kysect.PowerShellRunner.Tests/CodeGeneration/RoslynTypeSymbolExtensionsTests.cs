using FluentAssertions;
using Kysect.PowerShellRunner.CodeGeneration.Common;
using Kysect.PowerShellRunner.Tests.CodeGeneration.Tools;
using Microsoft.CodeAnalysis;
using NUnit.Framework;

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

        var syntaxTreeTestFacade = SyntaxTreeTestFacade.Create(input);

        INamedTypeSymbol classSymbol = syntaxTreeTestFacade.GetTypeSymbol("C");

        IReadOnlyCollection<string> baseTypes = RoslynTypeSymbolExtensions
            .GetBaseTypes(classSymbol)
            .Select(t => t.Name)
            .ToList();

        baseTypes.Should().BeEquivalentTo(expected);
    }
}