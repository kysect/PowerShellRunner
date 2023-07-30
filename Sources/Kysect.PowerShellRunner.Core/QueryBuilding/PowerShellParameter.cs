using Kysect.PowerShellRunner.Abstractions.Cmdlets;

namespace Kysect.PowerShellRunner.Core.QueryBuilding;

public class PowerShellParameter<T> : IPowerShellCmdletParameter<T> where T : notnull
{
    private readonly IPowerShellCmdletParameterValue _parameterValue;

    public PowerShellParameter(IPowerShellCmdletParameterValue parameterValue)
    {
        _parameterValue = parameterValue;
    }

    public IPowerShellCmdletParameterValue GetValue()
    {
        return _parameterValue;
    }

    public static implicit operator PowerShellParameter<T>(T value)
    {
        return new PowerShellParameter<T>(new PowerShellCmdletParameterValue(value));
    }

    public static implicit operator PowerShellParameter<T>(PowerShellParameter<T[]> value)
    {
        return new PowerShellParameter<T>(value._parameterValue);
    }
}