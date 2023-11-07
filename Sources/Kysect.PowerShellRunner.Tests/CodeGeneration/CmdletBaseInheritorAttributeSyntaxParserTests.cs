using FluentAssertions;
using Kysect.CommonLib.DependencyInjection;
using Kysect.CommonLib.ProgressTracking;
using Kysect.PowerShellRunner.CodeGeneration.Compilation;
using Kysect.PowerShellRunner.CodeGeneration.SyntaxParsing;
using Kysect.PowerShellRunner.Tests.CodeGeneration.Tools;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;

namespace Kysect.PowerShellRunner.Tests.CodeGeneration;

public class CmdletBaseInheritorAttributeSyntaxParserTests
{
    private readonly CmdletBaseInheritorAttributeSyntaxParser _attributeSyntaxParser;
    private readonly SolutionCompilationContextFactory _solutionCompilationContextFactory = new(new EmptyProgressTrackerFactory(), PredefinedLogger.CreateConsoleLogger());

    public CmdletBaseInheritorAttributeSyntaxParserTests()
    {
        _attributeSyntaxParser = new CmdletBaseInheritorAttributeSyntaxParser();
    }

    [Test]
    public void ExtractCmdletAttribute_ReturnAttribute()
    {
        var input = """
                    public class CmdletAttribute : System.Attribute
                    {
                        public CmdletAttribute(string verbName, string nounName) { }
                    }

                    [Cmdlet("SomeVerb", "AndNoun")]
                    public class C
                    {
                    }
                    """;

        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(input);
        CSharpCompilation compilation = TestCompilationFactory.CreateCompilation(syntaxTree);
        SolutionCompilationContext compilationContext = _solutionCompilationContextFactory.Create(compilation, new[] { syntaxTree });

        CmdletBaseInheritorCmdletAttributeSyntax attributeSyntax = _attributeSyntaxParser.ExtractCmdletAttribute(compilationContext.Items.Last().Syntax);

        attributeSyntax.VerbSyntax.ToFullString().Should().Be("\"SomeVerb\"");
        attributeSyntax.NounSyntax.ToFullString().Should().Be("\"AndNoun\"");
    }
}