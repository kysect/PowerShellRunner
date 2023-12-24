using Kysect.PowerShellRunner.Abstractions.Accessors;

namespace Kysect.PowerShellRunner.Core.Accessors;

public class PowerShellAccessorFactory : IPowerShellAccessorFactory
{
    public IPowerShellAccessor Create()
    {
        return new PowerShellAccessor(System.Management.Automation.PowerShell.Create());
    }
}