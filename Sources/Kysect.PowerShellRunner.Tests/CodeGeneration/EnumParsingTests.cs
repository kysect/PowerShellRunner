using FluentAssertions;
using Kysect.CommonLib.DependencyInjection.Logging;
using Kysect.PowerShellRunner.CodeGeneration.UsedModelSearching;
using Kysect.PowerShellRunner.Tests.CodeGeneration.Tools;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;

namespace Kysect.PowerShellRunner.Tests.CodeGeneration;

public class EnumParsingTests
{
    private readonly EnumDeclarationParser _parser = new(DefaultLoggerConfiguration.CreateConsoleLogger());

    [Test]
    public void ParseEnum_ShouldReturnAllMembers()
    {
        string enumWithMembers = SourceCodeProvider.EnumWithMembers();
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(enumWithMembers);
        EnumDeclarationSyntax enumDeclarationSyntax = syntaxTree
            .GetRoot()
            .DescendantNodes()
            .Where(n => n is EnumDeclarationSyntax)
            .Cast<EnumDeclarationSyntax>()
            .Single();

        ModelEnumTypeDescriptor modelEnumTypeDescriptor = _parser.ParseEnum(enumDeclarationSyntax);

        modelEnumTypeDescriptor.Should().NotBeNull();
        Assert.That(modelEnumTypeDescriptor.Values.Count, Is.EqualTo(2));
        Assert.That(modelEnumTypeDescriptor.Values.ElementAt(0).Identifier, Is.EqualTo("Value2"));
        Assert.That(modelEnumTypeDescriptor.Values.ElementAt(0).Value, Is.EqualTo(2));
        Assert.That(modelEnumTypeDescriptor.Values.ElementAt(1).Identifier, Is.EqualTo("Value3"));
        Assert.That(modelEnumTypeDescriptor.Values.ElementAt(1).Value, Is.EqualTo(null));
    }
}