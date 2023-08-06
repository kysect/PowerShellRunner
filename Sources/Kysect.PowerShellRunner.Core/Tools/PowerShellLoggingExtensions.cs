using Kysect.CommonLib.Logging;
using Kysect.PowerShellRunner.Abstractions.Objects;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Kysect.PowerShellRunner.Core.Tools;

public static class PowerShellLoggingExtensions
{
    public static void LogPowerShellObject(this ILogger logger, IReadOnlyCollection<IPowerShellObject> objects)
    {
        foreach (IPowerShellObject psObject in objects)
            logger.LogPowerShellObject(psObject);
    }

    public static void LogPowerShellObject(this ILogger logger, IPowerShellObject? powerShellObject)
    {
        if (powerShellObject is null)
        {
            logger.LogInformation("<null>");
            return;
        }

        logger.LogInformation(powerShellObject.AsString());

        foreach (IPowerShellObjectMember member in powerShellObject.GetProperties())
            logger.LogTabDebug(1, $"{member.Name,10}|{member.Value}");
    }
}