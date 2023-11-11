using Kysect.CommonLib.BaseTypes.Extensions;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kysect.PowerShellRunner.CodeGeneration.Common.Semantic;

public class RoslynMethodInvocationInfo
{
    public ExpressionSyntax CalledValue { get; }
    public string MethodName { get; }

    public static RoslynMethodInvocationInfo? TryGet(InvocationExpressionSyntax invocationExpressionSyntax)
    {
        invocationExpressionSyntax.ThrowIfNull();

        if (invocationExpressionSyntax.Expression is MemberAccessExpressionSyntax memberAccessExpressionSyntax)
        {
            return new RoslynMethodInvocationInfo(
                memberAccessExpressionSyntax.Expression,
                memberAccessExpressionSyntax.Name.Identifier.Text);
        }

        return null;
    }

    public RoslynMethodInvocationInfo(ExpressionSyntax calledValue, string methodName)
    {
        CalledValue = calledValue;
        MethodName = methodName;
    }
}