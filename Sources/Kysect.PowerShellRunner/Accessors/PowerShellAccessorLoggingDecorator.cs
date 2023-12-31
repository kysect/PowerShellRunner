﻿using Kysect.CommonLib.Exceptions;
using Kysect.CommonLib.Logging;
using Kysect.PowerShellRunner.Abstractions.Accessors;
using Kysect.PowerShellRunner.Abstractions.Accessors.Results;
using Kysect.PowerShellRunner.Abstractions.Objects;
using Kysect.PowerShellRunner.Abstractions.Queries;
using Kysect.PowerShellRunner.Extensions;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Kysect.PowerShellRunner.Accessors;

public class PowerShellAccessorLoggingDecorator : IPowerShellAccessor
{
    private readonly IPowerShellAccessor _innerImplementation;
    private readonly ILogger _logger;
    private readonly PowerShellQueryFormatter _executionStringFormatter;

    public PowerShellAccessorLoggingDecorator(IPowerShellAccessor innerImplementation, ILogger logger)
    {
        _innerImplementation = innerImplementation;
        _logger = logger;

        _executionStringFormatter = new PowerShellQueryFormatter();
    }

    public IPowerShellExecutionResult Execute(PowerShellQuery query)
    {
        if (query.ContainsSensitiveInfo)
        {
            _logger.LogDebug("Execute PowerShell command with sensitive data. Query will not be logged.");
        }
        else
        {
            string executableQuery = _executionStringFormatter.Format(query);
            _logger.LogDebug($"Execute PowerShell command: {executableQuery}");
        }

        IPowerShellExecutionResult result = _innerImplementation.Execute(query);
        switch (result)
        {
            case PowerShellFailedExecutionResult failedPowerShellExecutionResult:
                _logger.LogError("PS command executed failed.");
                if (failedPowerShellExecutionResult.Errors.Any())
                {
                    _logger.LogError("Errors:");
                    foreach (string error in failedPowerShellExecutionResult.Errors)
                        _logger.LogTabError(1, error);
                }

                if (failedPowerShellExecutionResult.OtherMessages.Any())
                {
                    _logger.LogError("Other messages:");
                    foreach (string otherMessage in failedPowerShellExecutionResult.OtherMessages)
                        _logger.LogTabError(1, otherMessage);
                }

                break;

            case PowerShellSuccessExecutionResult successPowerShellExecutionResult:
                _logger.LogDebug("PS command executed successfully.");
                foreach (IPowerShellObject powerShellObject in successPowerShellExecutionResult.Output)
                    _logger.LogPowerShellObject(powerShellObject);
                break;

            default:
                throw SwitchDefaultExceptions.OnUnexpectedType(result);
        }

        return result;
    }

    protected virtual void Dispose(bool disposing)
    {
        _innerImplementation.Dispose();
    }

    public void Dispose()
    {
        Dispose(disposing: true);
    }
}