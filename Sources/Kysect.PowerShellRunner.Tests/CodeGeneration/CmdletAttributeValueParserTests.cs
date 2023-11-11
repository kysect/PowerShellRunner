using FluentAssertions;
using Kysect.CommonLib.DependencyInjection;
using Kysect.PowerShellRunner.CodeGeneration.Compilation;
using Kysect.PowerShellRunner.CodeGeneration.SemanticParsing;
using Kysect.PowerShellRunner.CodeGeneration.SyntaxParsing;
using Kysect.PowerShellRunner.Tests.CodeGeneration.Tools;
using NUnit.Framework;

namespace Kysect.PowerShellRunner.Tests.CodeGeneration;

public class CmdletAttributeValueParserTests
{
    private readonly CmdletAttributeSyntaxParser _attributeSyntaxParser = new CmdletAttributeSyntaxParser();
    private readonly CmdletAttributeValueParser _attributeValueParser = new CmdletAttributeValueParser(PredefinedLogger.CreateConsoleLogger());

    [Test]
    public void ParseCmdletAttribute_ReturnExpectedValue()
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
        CmdletAttributeValues cmdletAttributeValues = _attributeValueParser.ParseCmdletAttribute(syntaxTreeTestFacade.SemanticModel, attributeSyntax);

        cmdletAttributeValues.Verb.Should().Be("SomeVerb");
        cmdletAttributeValues.Noun.Should().Be("AndNoun");
    }
}