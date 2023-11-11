using FluentAssertions;
using Kysect.PowerShellRunner.CodeGeneration.Compilation;
using Kysect.PowerShellRunner.CodeGeneration.SyntaxParsing;
using Kysect.PowerShellRunner.Tests.CodeGeneration.Tools;
using NUnit.Framework;

namespace Kysect.PowerShellRunner.Tests.CodeGeneration;

public class CmdletAttributeSyntaxParserTests
{
    private readonly CmdletAttributeSyntaxParser _attributeSyntaxParser;

    public CmdletAttributeSyntaxParserTests()
    {
        _attributeSyntaxParser = new CmdletAttributeSyntaxParser();
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

        var syntaxTreeTestFacade = SyntaxTreeTestFacade.Create(input);
        SolutionCompilationContext compilationContext = syntaxTreeTestFacade.CreateCompilationContext();

        CmdletAttributeSyntax attributeSyntax = _attributeSyntaxParser.ExtractCmdletAttribute(compilationContext.Items.Last().Syntax);

        attributeSyntax.VerbSyntax.ToFullString().Should().Be("\"SomeVerb\"");
        attributeSyntax.NounSyntax.ToFullString().Should().Be("\"AndNoun\"");
    }
}