using System.Collections.Generic;
using Kysect.PowerShellRunner.Abstractions.Objects;

namespace Kysect.PowerShellRunner.Abstractions.Accessors.Results;

public class PowerShellSuccessExecutionResult : IPowerShellExecutionResult
{
    public IReadOnlyCollection<IPowerShellObject> Output { get; }

    public PowerShellSuccessExecutionResult(IReadOnlyCollection<IPowerShellObject> output)
    {
        Output = output;
    }
}