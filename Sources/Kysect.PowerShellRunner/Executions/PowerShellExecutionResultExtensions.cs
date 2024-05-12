using Kysect.PowerShellRunner.Abstractions.Accessors.Results;
using Kysect.PowerShellRunner.Abstractions.Objects;
using Kysect.PowerShellRunner.Tools;

namespace Kysect.PowerShellRunner.Executions;

public static class PowerShellExecutionResultExtensions
{
    public static IReadOnlyCollection<IPowerShellObject> EnsureNoErrors(this IPowerShellExecutionResult result)
    {
        return result switch
        {
            PowerShellSuccessExecutionResult successPowerShellExecutionResult => successPowerShellExecutionResult.Output,
            PowerShellFailedExecutionResult failedPowerShellExecutionResult => throw new PowerShellExecutionException(failedPowerShellExecutionResult.Errors),
            _ => throw new ArgumentOutOfRangeException(nameof(result))
        };
    }
}