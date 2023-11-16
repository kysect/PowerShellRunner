using Kysect.CommonLib.BaseTypes.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kysect.PowerShellRunner.CodeGeneration.Common;

public static class SemanticModelExtensions
{
    public static string GetStringConstantValue(this SemanticModel semanticModel, ExpressionSyntax expressionSyntax)
    {
        semanticModel.ThrowIfNull();
        expressionSyntax.ThrowIfNull();

        Optional<object?> noun = semanticModel.GetConstantValue(expressionSyntax);
        if (!noun.HasValue || noun.Value is null)
            throw new RoslynAnalyzingException("Cannot extract const value from " + expressionSyntax.ToFullString());

        return noun.Value.To<string>();
    }
}