namespace Kysect.PowerShellRunner.Abstractions.Parameters;

public interface IPowerShellCmdletParameter<in T>
{
    IPowerShellCmdletParameterValue GetValue();
}