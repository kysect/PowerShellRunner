namespace Kysect.PowerShellRunner.Abstractions.Accessors.Results;

public class PowerShellFailedExecutionResult : IPowerShellExecutionResult
{
    public IReadOnlyCollection<string> Errors { get; }
    public IReadOnlyCollection<string> OtherMessages { get; }

    public PowerShellFailedExecutionResult(IReadOnlyCollection<string> errors, IReadOnlyCollection<string> otherMessages)
    {
        Errors = errors;
        OtherMessages = otherMessages;
    }
}