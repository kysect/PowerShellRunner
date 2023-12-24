using Kysect.PowerShellRunner.Abstractions.Accessors;
using Kysect.PowerShellRunner.Abstractions.Accessors.Results;
using Kysect.PowerShellRunner.Abstractions.Queries;
using Kysect.PowerShellRunner.Accessors.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;

namespace Kysect.PowerShellRunner.Accessors;

public class PowerShellAccessor : IPowerShellAccessor
{
    private readonly PowerShell _powerShellInstance;
    private readonly PowerShellQueryFormatter _executionStringFormatter;

    public PowerShellAccessor(PowerShell powerShellInstance)
    {
        _powerShellInstance = powerShellInstance;

        _executionStringFormatter = new PowerShellQueryFormatter();
    }

    public IPowerShellExecutionResult Execute(PowerShellQuery query)
    {
        string fullCommand = _executionStringFormatter.Format(query);

        lock (_powerShellInstance)
        {
            _powerShellInstance.AddScript(fullCommand);
            Collection<PSObject> result = _powerShellInstance.Invoke();

            IPowerShellExecutionResult methodResult = _powerShellInstance.HadErrors
                ? CreateFailedResult(result)
                : CreateSuccessResult(result);

            _powerShellInstance.Streams.ClearStreams();
            _powerShellInstance.Commands.Clear();
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
        var errors = _powerShellInstance.Streams.Error.ToList();
        var errorMessages = errors.Select(e => e.ToString()).ToList();

        var failedPowerShellExecutionResult = new PowerShellFailedExecutionResult(errorMessages, result
            .Where(r => r is not null)
            .Select(r => r.ToString())
            .ToList());

        return failedPowerShellExecutionResult;
    }

    protected virtual void Dispose(bool disposing)
    {
        _powerShellInstance.Dispose();
    }

    public void Dispose()
    {
        Dispose(disposing: true);
    }
}