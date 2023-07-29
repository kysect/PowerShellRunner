﻿using System.Collections.Generic;
using Kysect.PowerShellRunner.Abstractions.Objects;

namespace Kysect.PowerShellRunner.Abstractions.Accessors.Results;

public class SuccessPowerShellExecutionResult : IPowerShellExecutionResult
{
    public IReadOnlyCollection<IPowerShellObject> Output { get; }

    public SuccessPowerShellExecutionResult(IReadOnlyCollection<IPowerShellObject> output)
    {
        Output = output;
    }
}