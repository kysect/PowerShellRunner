using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.PowerShellRunner.CodeGeneration.Common;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kysect.PowerShellRunner.CodeGeneration.SyntaxParsing;

public class CmdletBaseInheritorAttributeSyntaxParser
{
    public const int VerbArgumentIndex = 0;
    public const int NounArgumentIndex = 1;

    public CmdletBaseInheritorAttributeSyntaxParser()
    {
    }

    public bool HasCmdletAttributes(BaseTypeDeclarationSyntax typeDeclaration)
    {
        return typeDeclaration.HasAttribute("Cmdlet");
    }

    public CmdletBaseInheritorCmdletAttributeSyntax ExtractCmdletAttribute(BaseTypeDeclarationSyntax typeDeclarationSyntax)
    {
        typeDeclarationSyntax.ThrowIfNull();

        AttributeSyntax cmdletAttribute = typeDeclarationSyntax.GetAttribute("Cmdlet");
        if (cmdletAttribute.ArgumentList is null)
            throw new RoslynAnalyzingException("Cmdlet attribute does not contains arguments. Type " + typeDeclarationSyntax.Identifier.Text);

        // TODO: parse third argument (not evident, that can be specified as SupportsShouldProcess = true
        if (cmdletAttribute.ArgumentList.Arguments.Count < 2)
            throw new RoslynAnalyzingException($"Cmdlet attribute has unexpected argument count: {cmdletAttribute.ArgumentList.Arguments.Count}. Type " + typeDeclarationSyntax.Identifier.Text);

        ExpressionSyntax verbSyntax = cmdletAttribute.ArgumentList.Arguments[VerbArgumentIndex].Expression;
        ExpressionSyntax nounSyntax = cmdletAttribute.ArgumentList.Arguments[NounArgumentIndex].Expression;

        return new CmdletBaseInheritorCmdletAttributeSyntax(cmdletAttribute, verbSyntax, nounSyntax);
    }
}