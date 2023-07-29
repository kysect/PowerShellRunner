namespace Kysect.PowerShellRunner.Abstractions.Cmdlets;

public class PowerShellCmdletParameterValue : IPowerShellCmdletParameterValue
{
    public object Value { get; }

    public PowerShellCmdletParameterValue(object value)
    {
        Value = value;
    }
}