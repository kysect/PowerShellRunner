using FluentAssertions;
using Kysect.PowerShellRunner.Tests.CodeGeneration.Tools;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Kysect.PowerShellRunner.CodeGeneration.Compilation;
using Kysect.CommonLib.DependencyInjection;
using Kysect.CommonLib.ProgressTracking;
using NUnit.Framework;

namespace Kysect.PowerShellRunner.Tests.CodeGeneration;

public class SolutionCompilationTypeInheritancesSearcherTests
{
    private readonly SolutionCompilationContextFactory _solutionCompilationContextFactory = new(new EmptyProgressTrackerFactory(), PredefinedLogger.CreateConsoleLogger());

    [Test]
    public void GetAllInheritances_ReturnExpectedResult()
    {
        string input = """
                       public class A { }
                       public class B : A { }
                       public class C : B { }
                       """;
        string[] expected = { "B", "C" };

        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(input);
        CSharpCompilation compilation = TestCompilationFactory.CreateCompilation(syntaxTree);
        var syntaxTreeTestFacade = new SyntaxTreeTestFacade(syntaxTree, compilation);
        SolutionCompilationContext compilationContext = _solutionCompilationContextFactory.Create(compilation, new[] { syntaxTree });
        var typeInheritancesSearcher = SolutionCompilationTypeInheritancesSearcher.CreateInstance(compilationContext.Items);

        INamedTypeSymbol typeSymbol = syntaxTreeTestFacade.GetTypeSymbol("A");
        IReadOnlyCollection<string> inheritances = typeInheritancesSearcher
            .GetAllInheritances(typeSymbol)
            .Select(s => s.Name)
            .ToList();

        inheritances.Should().BeEquivalentTo(expected);
    }
}