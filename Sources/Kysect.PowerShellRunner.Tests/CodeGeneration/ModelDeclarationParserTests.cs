using FluentAssertions;
using Kysect.PowerShellRunner.CodeGeneration.Parsing.Models;
using Kysect.PowerShellRunner.Tests.CodeGeneration.Tools;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace Kysect.PowerShellRunner.Tests.CodeGeneration;

public class ModelDeclarationParserTests
{
    private readonly ILogger _logger = new NullLogger<ModelDeclarationParserTests>();

    [Test]
    public void Parse_ModelWithProperties_ReturnParsedModelWithCorrectValues()
    {
        var input = """
            public class PowerShellModel
            {
                public string Name { get; set; }
                public int Version { get; }
                public IReadOnlyCollection<DateTime> Times { get; }
            }
            """;

        ClassDeclarationSyntax syntax = RoslynParsingExtensions.ExtractSyntax<ClassDeclarationSyntax>(input);
        CSharpCompilation sharpCompilation = RoslynParsingExtensions.CreateCompilation(syntax);
        var parser = new ModelDeclarationParser(_logger, sharpCompilation);

        ModelDeclarationParseResult modelDeclarationParseResult = parser.Parse(syntax);

        modelDeclarationParseResult.Name.Should().Be("PowerShellModel");
        modelDeclarationParseResult.Properties
            .Should().HaveCount(3)
            .And.Contain(new ModelDeclarationPropertyParseResult("Name", "string"))
            .And.Contain(new ModelDeclarationPropertyParseResult("Version", "int"))
            .And.Contain(new ModelDeclarationPropertyParseResult("Times", "IReadOnlyCollection<DateTime>"));
    }
}