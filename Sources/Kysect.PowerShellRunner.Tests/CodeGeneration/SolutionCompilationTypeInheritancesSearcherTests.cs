using FluentAssertions;
using Kysect.PowerShellRunner.CodeGeneration.Compilation;
using Kysect.PowerShellRunner.Tests.CodeGeneration.Tools;
using Microsoft.CodeAnalysis;
using NUnit.Framework;

namespace Kysect.PowerShellRunner.Tests.CodeGeneration;

public class SolutionCompilationTypeInheritancesSearcherTests
{
    [Test]
    public void GetAllInheritances_ReturnExpectedResult()
    {
        string input = """
                       public class A { }
                       public class B : A { }
                       public class C : B { }
                       """;
        string[] expected = { "B", "C" };

        var syntaxTreeTestFacade = SyntaxTreeTestFacade.Create(input);
        SolutionCompilationContext compilationContext = syntaxTreeTestFacade.CreateCompilationContext();
        var typeInheritancesSearcher = SolutionCompilationTypeInheritancesSearcher.CreateInstance(compilationContext.Items);

        INamedTypeSymbol typeSymbol = syntaxTreeTestFacade.GetTypeSymbol("A");
        IReadOnlyCollection<string> inheritances = typeInheritancesSearcher
            .GetAllInheritances(typeSymbol)
            .Select(s => s.Name)
            .ToList();

        inheritances.Should().BeEquivalentTo(expected);
    }
}