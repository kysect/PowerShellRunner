using Kysect.CommonLib.Exceptions;
using Kysect.CommonLib.Logging;
using Kysect.PowerShellRunner.Abstractions.Accessors;
using Kysect.PowerShellRunner.Abstractions.Accessors.Results;
using Kysect.PowerShellRunner.Abstractions.Objects;
using Kysect.PowerShellRunner.Abstractions.Queries;
using Kysect.PowerShellRunner.Extensions;
using Microsoft.Extensions.Logging;

namespace Kysect.PowerShellRunner.Accessors;

public class PowerShellAccessorLoggingDecorator(IPowerShellAccessor innerImplementation, ILogger logger)
    : IPowerShellAccessor
{
    public IPowerShellExecutionResult Execute(PowerShellQuery query)
    {
        if (query.ContainsSensitiveInfo)
        {
            logger.LogDebug("Execute PowerShell command with sensitive data. Query will not be logged.");
        }
        else
        {
            string executableQuery = query.Format();
            logger.LogDebug($"Execute PowerShell command: {executableQuery}");
        }

        IPowerShellExecutionResult result = innerImplementation.Execute(query);
        switch (result)
        {
            case PowerShellFailedExecutionResult failedPowerShellExecutionResult:
                logger.LogError("PS command executed failed.");
                if (failedPowerShellExecutionResult.Errors.Any())
                {
                    logger.LogError("Errors:");
                    foreach (string error in failedPowerShellExecutionResult.Errors)
                        logger.LogTabError(1, error);
                }

                if (failedPowerShellExecutionResult.OtherMessages.Any())
                {
                    logger.LogError("Other messages:");
                    foreach (string otherMessage in failedPowerShellExecutionResult.OtherMessages)
                        logger.LogTabError(1, otherMessage);
                }

                break;

            case PowerShellSuccessExecutionResult successPowerShellExecutionResult:
                logger.LogDebug("PS command executed successfully.");
                foreach (IPowerShellObject powerShellObject in successPowerShellExecutionResult.Output)
                    logger.LogPowerShellObject(powerShellObject);
                break;

            default:
                throw SwitchDefaultExceptions.OnUnexpectedType(result);
        }

        return result;
    }

    protected virtual void Dispose(bool disposing)
    {
        innerImplementation.Dispose();
    }

    public void Dispose()
    {
        Dispose(disposing: true);
    }
}