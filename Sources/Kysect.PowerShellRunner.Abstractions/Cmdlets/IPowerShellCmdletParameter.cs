namespace Kysect.PowerShellRunner.Abstractions.Cmdlets;

public interface IPowerShellCmdletParameter<in T>
{
    IPowerShellCmdletParameterValue GetValue();
}