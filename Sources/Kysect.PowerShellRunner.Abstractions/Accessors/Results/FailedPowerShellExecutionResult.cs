using System.Collections.Generic;

namespace Kysect.PowerShellRunner.Abstractions.Accessors.Results;

public class FailedPowerShellExecutionResult : IPowerShellExecutionResult
{
    public IReadOnlyCollection<string> Errors { get; }
    public IReadOnlyCollection<string> OtherMessages { get; }

    public FailedPowerShellExecutionResult(IReadOnlyCollection<string> errors, IReadOnlyCollection<string> otherMessages)
    {
        Errors = errors;
        OtherMessages = otherMessages;
    }
}