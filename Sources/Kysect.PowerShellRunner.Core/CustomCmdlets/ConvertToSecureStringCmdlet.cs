using Kysect.PowerShellRunner.Abstractions.Cmdlets;
using Kysect.PowerShellRunner.Abstractions.Parameters;
using System.Diagnostics.CodeAnalysis;
using System.Management.Automation;
using System.Security;

namespace Kysect.PowerShellRunner.Core.CustomCmdlets;

[SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "IPowerShellCmdlet")]
[SuppressMessage("Naming", "CS8618:Non-nullable variable must contain a non-null value when exiting constructor", Justification = "IPowerShellCmdlet")]
[SuppressMessage("ReSharper", "UnassignedGetOnlyAutoProperty")]
#pragma warning disable CS8618
public class ConvertToSecureStringCmdlet : IPowerShellCmdlet<SecureString>
{
    public string CmdletName => "ConvertTo-SecureString";

    public IPowerShellCmdletParameter<string> String { get; }
    public IPowerShellCmdletParameter<SwitchParameter> AsPlainText { get; }
    public IPowerShellCmdletParameter<SwitchParameter> Force { get; }
}
#pragma warning restore CS8618