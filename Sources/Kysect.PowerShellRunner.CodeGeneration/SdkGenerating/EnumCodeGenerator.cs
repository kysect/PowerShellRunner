using Kysect.PowerShellRunner.CodeGeneration.UsedModelSearching;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Kysect.CommonLib.BaseTypes.Extensions;
using System.Linq;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Kysect.PowerShellRunner.CodeGeneration.SdkGenerating;

public static class EnumCodeGenerator
{
    public static EnumDeclarationSyntax GenerateEnumDeclaration(ModelEnumTypeDescriptor descriptor)
    {
        descriptor.ThrowIfNull();

        EnumMemberDeclarationSyntax[] members = descriptor.Values.Select(GenerateEnumMember).ToArray();

        return EnumDeclaration(descriptor.Name)
            .WithModifiers(
                TokenList(
                    Token(SyntaxKind.PublicKeyword)))
            .AddMembers(members);
    }

    public static EnumMemberDeclarationSyntax GenerateEnumMember(ModelEnumTypeMemberDescriptor descriptor)
    {
        descriptor.ThrowIfNull();

        if (descriptor.Value is null)
            return EnumMemberDeclaration(Identifier(descriptor.Identifier));

        return EnumMemberDeclaration(Identifier(descriptor.Identifier))
            .WithEqualsValue(
                EqualsValueClause(
                    LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(descriptor.Value.Value))));
    }
}