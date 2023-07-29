using System;

namespace Kysect.PowerShellRunner.Abstractions.Cmdlets;

public class PowerShellCmdletParameterReferenceValue : IPowerShellCmdletParameterValue
{
    public string ParameterName { get; }

    public PowerShellCmdletParameterReferenceValue(string parameterName)
    {
        if (!parameterName.StartsWith("$"))
            throw new ArgumentException("Argument should start with $", nameof(parameterName));

        ParameterName = parameterName;
    }

    public override string ToString()
    {
        return ParameterName;
    }
}