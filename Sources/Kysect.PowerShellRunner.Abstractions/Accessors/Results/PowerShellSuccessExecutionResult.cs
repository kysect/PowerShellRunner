using Kysect.PowerShellRunner.Abstractions.Objects;
using System.Collections.Generic;

namespace Kysect.PowerShellRunner.Abstractions.Accessors.Results;

public class PowerShellSuccessExecutionResult : IPowerShellExecutionResult
{
    public IReadOnlyCollection<IPowerShellObject> Output { get; }

    public PowerShellSuccessExecutionResult(IReadOnlyCollection<IPowerShellObject> output)
    {
        Output = output;
    }
}