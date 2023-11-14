using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Kysect.PowerShellRunner.CodeGeneration.SdkGenerating;

public static class CodeGenerationNamespaceWrapper
{
    public static CompilationUnitSyntax Wrap(MemberDeclarationSyntax node, string namespaceName, IReadOnlyCollection<string> usedNamespaces)
    {
        UsingDirectiveSyntax[] usingDirectiveSyntaxes = usedNamespaces
            .Select(n => SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName(n)))
            .ToArray();

        return SyntaxFactory.CompilationUnit()
            .AddUsings(usingDirectiveSyntaxes)
            .AddMembers(
                SyntaxFactory.FileScopedNamespaceDeclaration(SyntaxFactory.IdentifierName(namespaceName))
                    .AddMembers(node));
    }
}