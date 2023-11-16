using Kysect.CommonLib.BaseTypes.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kysect.PowerShellRunner.CodeGeneration.Common;

public static class RoslynEnumFullNameResolver
{
    public static string GetEnumNameWithContainingClass(EnumDeclarationSyntax enumDeclaration)
    {
        enumDeclaration.ThrowIfNull();

        SyntaxNode? enumParent = enumDeclaration.Parent;

        if (enumParent is not ClassDeclarationSyntax parentClass)
            return enumDeclaration.Identifier.Text;

        return $"{parentClass.Identifier.Text}{enumDeclaration.Identifier.Text}";

    }
}