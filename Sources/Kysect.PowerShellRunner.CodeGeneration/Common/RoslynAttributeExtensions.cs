using Kysect.CommonLib.BaseTypes.Extensions;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kysect.PowerShellRunner.CodeGeneration.Common;

public static class RoslynAttributeExtensions
{
    public static bool HasAttribute(this MemberDeclarationSyntax memberDeclarationSyntax, string attributeName)
    {
        return memberDeclarationSyntax
            .GetAllAttributes()
            .Any(a => a.Name.ToFullString() == attributeName);
    }

    public static AttributeSyntax GetAttribute(this MemberDeclarationSyntax memberDeclarationSyntax, string attributeName)
    {
        return memberDeclarationSyntax
            .GetAllAttributes()
            .Single(a => a.Name.ToFullString() == attributeName);
    }

    public static IReadOnlyCollection<AttributeSyntax> GetAllAttributes(this MemberDeclarationSyntax memberDeclarationSyntax)
    {
        memberDeclarationSyntax.ThrowIfNull();

        return memberDeclarationSyntax
            .AttributeLists
            .SelectMany(a => a.Attributes)
            .ToList();
    }
}