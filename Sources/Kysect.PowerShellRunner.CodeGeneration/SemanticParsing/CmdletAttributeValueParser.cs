using Kysect.PowerShellRunner.CodeGeneration.Common;
using Kysect.PowerShellRunner.CodeGeneration.SyntaxParsing;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using Kysect.CommonLib.BaseTypes.Extensions;

namespace Kysect.PowerShellRunner.CodeGeneration.SemanticParsing;

public class CmdletAttributeValueParser
{
    private readonly ILogger _logger;

    public CmdletAttributeValueParser(ILogger logger)
    {
        _logger = logger;
    }

    public CmdletAttributeValues ParsePowerShellNameAttribute(SemanticModel semanticModel, CmdletAttributeSyntax cmdletAttribute)
    {
        semanticModel.ThrowIfNull();
        cmdletAttribute.ThrowIfNull();

        string verbName = GetStringValue(semanticModel, cmdletAttribute.VerbSyntax);
        string noun = GetStringValue(semanticModel, cmdletAttribute.NounSyntax);

        return new CmdletAttributeValues(verbName, noun);
    }

    private string GetStringValue(SemanticModel semanticModel, ExpressionSyntax expressionSyntax)
    {
        try
        {
            return semanticModel.GetStringConstantValue(expressionSyntax);

        }
        catch (Exception e)
        {
            // KB: cannot parse const values because some values store in nuget that is not loaded
            //string verbName = semanticModel.GetStringConstantValue(cmdletAttribute.VerbSyntax);

            _logger.LogWarning("Cannot parse attribute value from {expression}. Try parse by name. Error: {message}", expressionSyntax.ToFullString(), e.Message);
            // TODO: rework this
            return expressionSyntax.DeconstructMemberAccessChain().Last().ToString().Replace("\"", "");
        }
    }
}