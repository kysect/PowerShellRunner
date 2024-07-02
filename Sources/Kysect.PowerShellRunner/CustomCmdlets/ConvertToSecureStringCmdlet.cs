using Kysect.PowerShellRunner.Abstractions.Cmdlets;
using Kysect.PowerShellRunner.Abstractions.Parameters;
using System.Management.Automation;
using System.Security;

namespace Kysect.PowerShellRunner.CustomCmdlets;

public class ConvertToSecureStringCmdlet : IPowerShellCmdlet<SecureString>
{
    public string CmdletName => "ConvertTo-SecureString";

    public IPowerShellCmdletParameter<string> String { get; } = null!;
    public IPowerShellCmdletParameter<SwitchParameter> AsPlainText { get; } = null!;
    public IPowerShellCmdletParameter<SwitchParameter> Force { get; } = null!;
}
