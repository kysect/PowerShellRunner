using FluentAssertions;
using Kysect.CommonLib.DependencyInjection.Logging;
using Kysect.CommonLib.ProgressTracking;
using Kysect.PowerShellRunner.CodeGeneration.Compilation;
using Kysect.PowerShellRunner.CodeGeneration.SyntaxParsing;
using Kysect.PowerShellRunner.Tests.CodeGeneration.Tools;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;

namespace Kysect.PowerShellRunner.Tests.CodeGeneration;

public class CmdletBaseSyntaxInfoParserTests
{
    private readonly CmdletBaseSyntaxInfoParser _baseSyntaxInfoParser;
    private readonly SolutionCompilationContextFactory _solutionCompilationContextFactory = new(new EmptyProgressTrackerFactory(), DefaultLoggerConfiguration.CreateConsoleLogger());

    public CmdletBaseSyntaxInfoParserTests()
    {
        _baseSyntaxInfoParser = new CmdletBaseSyntaxInfoParser(DefaultLoggerConfiguration.CreateConsoleLogger());
    }

    [Test]
    public void GetAllParameterProperties_ForClassWithPublicAndPrivateProperties_ReturnAllWithParameterAttribute()
    {
        var input = """
                    public class ParameterAttribute : System.Attribute {}
                    
                    public class C
                    {
                        [Parameter] public int First { get; }
                        [Parameter] private int Second { get; }
                        public int Third { get; }
                    }
                    """;
        string[] expected = new[] { "First", "Second" };

        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(input);
        CSharpCompilation compilation = TestCompilationFactory.CreateCompilation(syntaxTree);
        SolutionCompilationContext compilationContext = _solutionCompilationContextFactory.Create(compilation, new[] { syntaxTree });
        var properties = _baseSyntaxInfoParser
            .GetAllParameterProperties(compilationContext.Items.Last())
            .Select(p => p.Identifier.Text)
            .ToList();

        properties.Should().BeEquivalentTo(expected);
    }
}