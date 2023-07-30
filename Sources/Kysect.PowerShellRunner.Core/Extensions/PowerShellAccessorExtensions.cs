using Kysect.PowerShellRunner.Abstractions.Accessors;
using Kysect.PowerShellRunner.Abstractions.Cmdlets;
using Kysect.PowerShellRunner.Core.Executions;

namespace Kysect.PowerShellRunner.Core.Extensions;

public static class PowerShellAccessorExtensions
{
    public static PowerShellAccessorCmdletExecutionContext<T> SelectCmdlet<T>(this IPowerShellAccessor accessor, IPowerShellCmdlet<T> cmdlet) where T : notnull
    {
        return new PowerShellAccessorCmdletExecutionContext<T>(accessor, cmdlet);
    }

    public static PowerShellAccessorCmdletExecutionContext SelectCmdlet(this IPowerShellAccessor accessor, IPowerShellCmdlet cmdlet)
    {
        return new PowerShellAccessorCmdletExecutionContext(accessor, cmdlet);
    }
}