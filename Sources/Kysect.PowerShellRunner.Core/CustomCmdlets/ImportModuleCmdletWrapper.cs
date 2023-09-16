using Kysect.PowerShellRunner.Abstractions.Cmdlets;
using Kysect.PowerShellRunner.Abstractions.Parameters;
using System.Management.Automation;

namespace Kysect.PowerShellRunner.Core.CustomCmdlets;

public class ImportModuleCmdletWrapper : IPowerShellCmdlet
{
    public string CmdletName => "Import-Module";

    public IPowerShellCmdletParameter<string>? Name { get; }
    public IPowerShellCmdletParameter<SwitchParameter>? Verbose { get; }
}
