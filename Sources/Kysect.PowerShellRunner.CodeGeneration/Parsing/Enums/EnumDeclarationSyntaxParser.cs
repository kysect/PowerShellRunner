using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.PowerShellRunner.CodeGeneration.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;

namespace Kysect.PowerShellRunner.CodeGeneration.Parsing.Enums;

public class EnumDeclarationSyntaxParser
{
    private readonly ILogger _logger;
    private readonly CSharpCompilation _compilation;

    public EnumDeclarationSyntaxParser(ILogger logger, CSharpCompilation compilation)
    {
        _logger = logger;
        _compilation = compilation;
    }

    public EnumDeclarationSyntaxParseResult Parse(EnumDeclarationSyntax syntax)
    {
        syntax.ThrowIfNull();

        SemanticModel semanticModel = _compilation.GetSemanticModel(syntax.SyntaxTree);

        // TODO: support case when enum is member of other class
        string enumTypeName = syntax.Identifier.Text;

        _logger.LogInformation("Parse enum {enum}", enumTypeName);

        var members = syntax.Members
            .Select(m => ParseMember(m, semanticModel))
            .ToList();

        return new EnumDeclarationSyntaxParseResult(enumTypeName, members);
    }

    private EnumMemberDeclarationSyntaxParseResult ParseMember(EnumMemberDeclarationSyntax enumMemberDeclarationSyntax, SemanticModel semanticModel)
    {
        if (enumMemberDeclarationSyntax.EqualsValue is null)
            return new EnumMemberDeclarationSyntaxParseResult(enumMemberDeclarationSyntax.Identifier.Text, null);

        Optional<object?> constantValue = semanticModel.GetConstantValue(enumMemberDeclarationSyntax.EqualsValue.Value);
        if (!constantValue.HasValue)
            throw new RoslynAnalyzingException($"Cannot parse enum value for {enumMemberDeclarationSyntax.EqualsValue.ToFullString()}");

        return new EnumMemberDeclarationSyntaxParseResult(enumMemberDeclarationSyntax.Identifier.Text, constantValue.Value.To<int>());

    }
}