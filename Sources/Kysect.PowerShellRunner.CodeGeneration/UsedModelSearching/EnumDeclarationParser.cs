using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.PowerShellRunner.CodeGeneration.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;

namespace Kysect.PowerShellRunner.CodeGeneration.UsedModelSearching;

public class EnumDeclarationParser
{
    private readonly ILogger _logger;

    public EnumDeclarationParser(ILogger logger)
    {
        _logger = logger;
    }

    public ModelEnumTypeDescriptor ParseEnum(EnumDeclarationSyntax enumDeclaration)
    {
        enumDeclaration.ThrowIfNull();

        _logger.LogTrace($"Parse enum declaration syntax {enumDeclaration.Identifier.Text}");
        string enumName = RoslynEnumFullNameResolver.GetEnumNameWithContainingClass(enumDeclaration);
        if (enumName != enumDeclaration.Identifier.Text)
            _logger.LogTrace($"Enum {enumDeclaration.Identifier.Text} resolved to full name: {enumName}");

        var members = enumDeclaration
            .DescendantNodes()
            .OfType<EnumMemberDeclarationSyntax>()
            .Select(syntax => ParseEnumMember(enumDeclaration, syntax))
            .ToList();

        return new ModelEnumTypeDescriptor(enumName, members);
    }

    public ModelEnumTypeMemberDescriptor ParseEnumMember(EnumDeclarationSyntax enumDeclaration, EnumMemberDeclarationSyntax enumMember)
    {
        enumDeclaration.ThrowIfNull();
        enumMember.ThrowIfNull();

        if (enumMember.EqualsValue is null)
            return new ModelEnumTypeMemberDescriptor(enumMember.Identifier.Text);

        if (enumMember.EqualsValue.Value is LiteralExpressionSyntax literalExpression
            && literalExpression.IsKind(SyntaxKind.NumericLiteralExpression))
        {
            if (literalExpression.Token.Text.StartsWith("0x"))
                return new ModelEnumTypeMemberDescriptor(enumMember.Identifier.Text, Convert.ToInt32(literalExpression.Token.Text, 16));

            return new ModelEnumTypeMemberDescriptor(enumMember.Identifier.Text, int.Parse(literalExpression.Token.Text));
        }

        if (enumMember.EqualsValue.Value is PrefixUnaryExpressionSyntax prefixUnaryExpressionSyntax)
        {
            string text = prefixUnaryExpressionSyntax.ToString();
            return new ModelEnumTypeMemberDescriptor(enumMember.Identifier.Text, int.Parse(text));
        }

        // TODO: implement MemberAccessExpression
        string message = $"Cannot parse {enumMember.Identifier.Value} member {enumMember.Identifier.Text}: " + enumMember.EqualsValue.Value.ToFullString();
        return new ModelEnumTypeMemberDescriptor(enumMember.Identifier.Text);
    }
}