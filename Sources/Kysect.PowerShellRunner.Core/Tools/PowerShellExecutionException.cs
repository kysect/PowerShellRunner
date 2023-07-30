using Kysect.CommonLib.Collections.Extensions;
using System;
using System.Collections.Generic;

namespace Kysect.PowerShellRunner.Core.Tools;

public class PowerShellExecutionException : PowerShellIntegrationException
{
    public IReadOnlyCollection<string> Errors { get; }

    public PowerShellExecutionException() : base("PowerShell command execution was finished with error but not details was provided.")
    {
        Errors = Array.Empty<string>();
    }

    public PowerShellExecutionException(IReadOnlyCollection<string> errors) : base(CreateErrorString(errors))
    {
        Errors = errors;
    }

    public static string CreateErrorString(IReadOnlyCollection<string> errors)
    {
        if (errors.Count == 0)
        {
            return $"Execution finished with errors.";
        }

        return "Execution finished with errors: " + errors.ToSingleString();
    }
}