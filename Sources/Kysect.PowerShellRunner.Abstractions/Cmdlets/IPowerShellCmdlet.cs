namespace Kysect.PowerShellRunner.Abstractions.Cmdlets;

public interface IPowerShellCmdlet
{
    public string CmdletName { get; }
}

public interface IPowerShellCmdlet<TResult> : IPowerShellCmdlet
{
}