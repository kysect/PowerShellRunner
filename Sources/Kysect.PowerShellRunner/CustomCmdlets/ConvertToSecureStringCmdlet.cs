using Kysect.PowerShellRunner.Abstractions.Cmdlets;
using Kysect.PowerShellRunner.Abstractions.Parameters;
using System.Management.Automation;
using System.Security;

namespace Kysect.PowerShellRunner.CustomCmdlets;

public class ConvertToSecureStringCmdlet : IPowerShellCmdlet<SecureString>
{
    public string CmdletName => "ConvertTo-SecureString";

#pragma warning disable CA1720 // Identifier contains type name
    public IPowerShellCmdletParameter<string> String { get; } = null!;
#pragma warning restore CA1720 // Identifier contains type name
    public IPowerShellCmdletParameter<SwitchParameter> AsPlainText { get; } = null!;
    public IPowerShellCmdletParameter<SwitchParameter> Force { get; } = null!;
}
