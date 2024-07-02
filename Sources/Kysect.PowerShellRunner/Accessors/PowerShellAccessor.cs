using Kysect.PowerShellRunner.Abstractions.Accessors;
using Kysect.PowerShellRunner.Abstractions.Accessors.Results;
using Kysect.PowerShellRunner.Abstractions.Queries;
using Kysect.PowerShellRunner.Accessors.Models;
using System.Collections.ObjectModel;
using System.Management.Automation;

namespace Kysect.PowerShellRunner.Accessors;

public class PowerShellAccessor(PowerShell powerShellInstance) : IPowerShellAccessor
{
    public IPowerShellExecutionResult Execute(PowerShellQuery query)
    {
        string fullCommand = query.Format();

        lock (powerShellInstance)
        {
            powerShellInstance.AddScript(fullCommand);
            Collection<PSObject> result = powerShellInstance.Invoke();

            IPowerShellExecutionResult methodResult = powerShellInstance.HadErrors
                ? CreateFailedResult(result)
                : CreateSuccessResult(result);

            powerShellInstance.Streams.ClearStreams();
            powerShellInstance.Commands.Clear();
            return methodResult;
        }
    }

    private PowerShellSuccessExecutionResult CreateSuccessResult(Collection<PSObject> result)
    {
        var resultItems = result
            .Where(p => p is not null)
            .Select(p => new PowerShellObject(p))
            .ToList();

        return new PowerShellSuccessExecutionResult(resultItems);
    }

    private PowerShellFailedExecutionResult CreateFailedResult(Collection<PSObject> result)
    {
        var errors = powerShellInstance.Streams.Error.ToList();
        var errorMessages = errors.Select(e => e.ToString()).ToList();

        var failedPowerShellExecutionResult = new PowerShellFailedExecutionResult(errorMessages, result
            .Where(r => r is not null)
            .Select(r => r.ToString())
            .ToList());

        return failedPowerShellExecutionResult;
    }

    protected virtual void Dispose(bool disposing)
    {
        powerShellInstance.Dispose();
    }

    public void Dispose()
    {
        Dispose(disposing: true);
    }
}