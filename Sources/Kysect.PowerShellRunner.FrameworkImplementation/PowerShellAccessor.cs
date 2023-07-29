using Kysect.PowerShellRunner.Abstractions.Accessors;
using Kysect.PowerShellRunner.Abstractions.Accessors.Results;
using Kysect.PowerShellRunner.Abstractions.Queries;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;

namespace Kysect.PowerShellRunner.FrameworkImplementation;

public class PowerShellAccessor : IPowerShellAccessor
{
    private readonly PowerShell _powerShellInstance;
    private readonly PowerShellQueryBuilder _executionStringBuilder;

    public PowerShellAccessor(PowerShell powerShellInstance)
    {
        _powerShellInstance = powerShellInstance;

        _executionStringBuilder = new PowerShellQueryBuilder();
    }

    public IPowerShellExecutionResult Execute(PowerShellQueryArguments query)
    {
        string fullCommand = _executionStringBuilder.Build(query);

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

    private SuccessPowerShellExecutionResult CreateSuccessResult(Collection<PSObject> result)
    {
        var resultItems = result
            .Where(p => p is not null)
            .Select(p => new PowerShellObject(p))
            .ToList();

        return new SuccessPowerShellExecutionResult(resultItems);
    }

    private FailedPowerShellExecutionResult CreateFailedResult(Collection<PSObject> result)
    {
        var errors = _powerShellInstance.Streams.Error.ToList();
        var errorMessages = errors.Select(e => e.ToString()).ToList();

        var failedPowerShellExecutionResult = new FailedPowerShellExecutionResult(errorMessages, result
            .Where(r => r is not null)
            .Select(r => r.ToString())
            .ToList());

        return failedPowerShellExecutionResult;
    }

    public void Dispose()
    {
        _powerShellInstance.Dispose();
    }
}