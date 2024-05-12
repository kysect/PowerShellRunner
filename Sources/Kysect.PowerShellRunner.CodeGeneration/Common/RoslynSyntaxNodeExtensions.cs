using Kysect.CommonLib.BaseTypes.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kysect.PowerShellRunner.CodeGeneration.Common;

public static class RoslynSyntaxNodeExtensions
{
    public static IReadOnlyCollection<InvocationExpressionSyntax> GetInvocationExpressionByName(this SyntaxNode typeSyntaxNode, string name)
    {
        typeSyntaxNode.ThrowIfNull();

        bool IsMatchedName(InvocationExpressionSyntax expressionSyntax)
        {
            return expressionSyntax.Expression is IdentifierNameSyntax identifierName
                   && identifierName.Identifier.Text == name;
        }

        return typeSyntaxNode
            .DescendantNodes()
            .OfType<InvocationExpressionSyntax>()
            .Where(IsMatchedName)
            .ToList();
    }

    public static IReadOnlyList<ExpressionSyntax> DeconstructMemberAccessChain(this ExpressionSyntax syntax)
    {
        if (syntax is MemberAccessExpressionSyntax memberAccessExpressionSyntax)
        {
            var result = new List<ExpressionSyntax>();
            result.AddRange(memberAccessExpressionSyntax.Expression.DeconstructMemberAccessChain());
            result.Add(memberAccessExpressionSyntax.Name);
            return result;
        }

        return new[] { syntax };
    }
}