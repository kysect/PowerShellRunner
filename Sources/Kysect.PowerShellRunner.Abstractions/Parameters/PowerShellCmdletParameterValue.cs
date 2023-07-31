namespace Kysect.PowerShellRunner.Abstractions.Parameters;

public class PowerShellCmdletParameterValue : IPowerShellCmdletParameterValue
{
    public object Value { get; }

    public PowerShellCmdletParameterValue(object value)
    {
        Value = value;
    }
}