using Kysect.CommonLib.BaseTypes.Extensions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kysect.PowerShellRunner.CodeGeneration.SdkGenerating;

public static class PropertyDeclarationSyntaxGenerationExtensions
{
    public static PropertyDeclarationSyntax WithGetSetAccessorList(this PropertyDeclarationSyntax propertyDeclaration)
    {
        propertyDeclaration.ThrowIfNull();

        return propertyDeclaration
            .WithAccessorList(
                SyntaxFactory.AccessorList(
                    SyntaxFactory.List(
                        new[]
                        {
                            SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                                .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                            SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                                .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                        })));
    }
}