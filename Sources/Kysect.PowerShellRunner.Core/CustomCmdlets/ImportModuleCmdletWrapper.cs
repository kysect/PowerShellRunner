using Kysect.PowerShellRunner.Abstractions.Cmdlets;
using System.Diagnostics.CodeAnalysis;
using System.Management.Automation;

namespace Kysect.PowerShellRunner.Core.CustomCmdlets;

[SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "IPowerShellCmdlet")]
[SuppressMessage("Naming", "CS8618:Non-nullable variable must contain a non-null value when exiting constructor", Justification = "IPowerShellCmdlet")]
[SuppressMessage("ReSharper", "UnassignedGetOnlyAutoProperty")]
#pragma warning disable CS8618
public class ImportModuleCmdletWrapper : IPowerShellCmdlet
{
    public string CmdletName => "Import-Module";

    public IPowerShellCmdletParameter<string> Name { get; }
    public IPowerShellCmdletParameter<SwitchParameter> Verbose { get; }
}
#pragma warning restore CS8618