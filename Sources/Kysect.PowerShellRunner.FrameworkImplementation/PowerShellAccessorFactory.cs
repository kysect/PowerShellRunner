using Kysect.PowerShellRunner.Abstractions.Accessors;

namespace Kysect.PowerShellRunner.FrameworkImplementation;

public class PowerShellAccessorFactory : IPowerShellAccessorFactory
{
    public IPowerShellAccessor Create()
    {
        return new PowerShellAccessor(System.Management.Automation.PowerShell.Create());
    }
}