using Kysect.PowerShellRunner.Abstractions.Accessors;

namespace Kysect.PowerShellRunner.Accessors;

public class PowerShellAccessorFactory : IPowerShellAccessorFactory
{
    public IPowerShellAccessor Create()
    {
#pragma warning disable CA2000 // Dispose objects before losing scope
        return new PowerShellAccessor(System.Management.Automation.PowerShell.Create());
#pragma warning restore CA2000 // Dispose objects before losing scope
    }
}