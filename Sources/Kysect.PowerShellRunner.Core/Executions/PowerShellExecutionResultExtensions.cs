using Kysect.PowerShellRunner.Abstractions.Accessors.Results;
using Kysect.PowerShellRunner.Abstractions.Objects;
using Kysect.PowerShellRunner.Core.Tools;
using System;
using System.Collections.Generic;

namespace Kysect.PowerShellRunner.Core.Executions;

public static class PowerShellExecutionResultExtensions
{
    public static IReadOnlyCollection<IPowerShellObject> EnsureNoErrors(this IPowerShellExecutionResult result)
    {
        return result switch
        {
            SuccessPowerShellExecutionResult successPowerShellExecutionResult => successPowerShellExecutionResult.Output,
            FailedPowerShellExecutionResult failedPowerShellExecutionResult => throw new PowerShellExecutionException(failedPowerShellExecutionResult.Errors),
            _ => throw new ArgumentOutOfRangeException(nameof(result))
        };
    }
}