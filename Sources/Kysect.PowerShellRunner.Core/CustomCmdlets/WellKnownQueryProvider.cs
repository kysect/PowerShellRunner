using Kysect.PowerShellRunner.Abstractions.Queries;

namespace Kysect.PowerShellRunner.Core.CustomCmdlets;

public class WellKnownQueryProvider
{
    public static WellKnownQueryProvider Instance { get; } = new WellKnownQueryProvider();

    public PowerShellQuery ChangeDirectory(string path)
    {
        return new PowerShellQuery($"cd \"{path}\"");
    }

    public PowerShellQuery GetPowerShellVersion()
    {
        return new PowerShellQuery("$PSVersionTable.PSVersion");
    }

    public PowerShellQuery OpenFileInDefaultApplication(string path)
    {
        return new PowerShellQuery($"start \"{path}\"");
    }

    public PowerShellQuery SetAlias(string aliasName, string path)
    {
        return new PowerShellQuery($"Set-Alias {aliasName} \"{path}\"");
    }
}