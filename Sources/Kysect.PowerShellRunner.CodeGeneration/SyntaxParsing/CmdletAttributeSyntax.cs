using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kysect.PowerShellRunner.CodeGeneration.SyntaxParsing;

public class CmdletAttributeSyntax
{
    public AttributeSyntax Attribute { get; }
    public ExpressionSyntax VerbSyntax { get; }
    public ExpressionSyntax NounSyntax { get; }

    public CmdletAttributeSyntax(AttributeSyntax attribute, ExpressionSyntax verbSyntax, ExpressionSyntax nounSyntax)
    {
        Attribute = attribute;
        VerbSyntax = verbSyntax;
        NounSyntax = nounSyntax;
    }
}