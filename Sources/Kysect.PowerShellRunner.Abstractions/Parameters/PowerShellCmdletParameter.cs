namespace Kysect.PowerShellRunner.Abstractions.Parameters;

public class PowerShellCmdletParameter<T> : IPowerShellCmdletParameter<T> where T : notnull
{
    private readonly IPowerShellCmdletParameterValue _parameterValue;

    public PowerShellCmdletParameter(IPowerShellCmdletParameterValue parameterValue)
    {
        _parameterValue = parameterValue;
    }

    public IPowerShellCmdletParameterValue GetValue()
    {
        return _parameterValue;
    }

    public static implicit operator PowerShellCmdletParameter<T>(T value)
    {
        return new PowerShellCmdletParameter<T>(new PowerShellCmdletParameterValue(value));
    }

    public static implicit operator PowerShellCmdletParameter<T>(PowerShellCmdletParameter<T[]> value)
    {
        return new PowerShellCmdletParameter<T>(value._parameterValue);
    }
}