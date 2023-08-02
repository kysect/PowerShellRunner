﻿using FluentAssertions;
using Kysect.RoslynUtils.Parsing;
using Kysect.RoslynUtils.Parsing.Enums;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace Kysect.RoslynUtils.Tests;

public class EnumDeclarationSyntaxParserTests
{
    private readonly ILogger _logger = new NullLogger<EnumDeclarationSyntaxParserTests>();

    [Test]
    public void Parse_EnumMemberWithoutValue_ReturnNullValue()
    {
        var input = """
            public enum EnumType
            {
                Value1
            }
            """;

        EnumDeclarationSyntax enumDeclarationSyntax = ExtractEnum(input);
        CSharpCompilation sharpCompilation = CreateCompilation(enumDeclarationSyntax);
        var parser = new EnumDeclarationSyntaxParser(_logger, sharpCompilation);

        EnumDeclarationSyntaxParseResult result = parser.Parse(enumDeclarationSyntax);

        result.Name
            .Should().Be("EnumType");

        result.Members
            .Should().HaveCount(1)
            .And.Contain(new EnumMemberDeclarationSyntaxParseResult("Value1", null));
    }

    [Test]
    public void Parse_EnumMemberWithValue_ReturnExpectedValue()
    {
        var input = """
            public enum EnumType
            {
                Value1 = 3
            }
            """;

        EnumDeclarationSyntax enumDeclarationSyntax = ExtractEnum(input);
        CSharpCompilation sharpCompilation = CreateCompilation(enumDeclarationSyntax);
        var parser = new EnumDeclarationSyntaxParser(_logger, sharpCompilation);

        EnumDeclarationSyntaxParseResult result = parser.Parse(enumDeclarationSyntax);

        result.Name
            .Should().Be("EnumType");

        result.Members
            .Should().HaveCount(1)
            .And.Contain(new EnumMemberDeclarationSyntaxParseResult("Value1", 3));
    }

    [Test]
    public void Parse_HexValue_ReturnExpectedValue()
    {
        var input = """
            public enum EnumType
            {
                Value1 = 0xaa
            }
            """;

        EnumDeclarationSyntax enumDeclarationSyntax = ExtractEnum(input);
        CSharpCompilation sharpCompilation = CreateCompilation(enumDeclarationSyntax);
        var parser = new EnumDeclarationSyntaxParser(_logger, sharpCompilation);

        EnumDeclarationSyntaxParseResult result = parser.Parse(enumDeclarationSyntax);

        result.Name
            .Should().Be("EnumType");

        result.Members
            .Should().HaveCount(1)
            .And.Contain(new EnumMemberDeclarationSyntaxParseResult("Value1", 170));
    }

    // TODO: implement
    [Test]
    [Ignore("TODO: implement")]
    public void Parse_ReferenceToOtherMember_ReturnExpectedValue()
    {
        var input = """
            public enum EnumType
            {
                Value1 = 2,
                Value2 = Value1
            }
            """;

        EnumDeclarationSyntax enumDeclarationSyntax = ExtractEnum(input);
        CSharpCompilation sharpCompilation = CreateCompilation(enumDeclarationSyntax);
        var parser = new EnumDeclarationSyntaxParser(_logger, sharpCompilation);

        EnumDeclarationSyntaxParseResult result = parser.Parse(enumDeclarationSyntax);

        result.Name
            .Should().Be("EnumType");

        result.Members
            .Should().HaveCount(2)
            .And.Contain(new EnumMemberDeclarationSyntaxParseResult("Value2", 4));
    }

    private EnumDeclarationSyntax ExtractEnum(string input)
    {
        return CSharpSyntaxTree
            .ParseText(input)
            .GetRoot()
            .DescendantNodes()
            .OfType<EnumDeclarationSyntax>()
            .Single();
    }

    private CSharpCompilation CreateCompilation(SyntaxNode syntaxNode)
    {
        return CSharpCompilation
            .Create("TestCompilation")
            .AddSyntaxTrees(syntaxNode.SyntaxTree);
    }
}