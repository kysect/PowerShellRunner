﻿using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.CommonLib.DependencyInjection.Logging;
using Kysect.PowerShellRunner.Abstractions.Accessors;
using Kysect.PowerShellRunner.Abstractions.Cmdlets;
using Kysect.PowerShellRunner.Accessors;
using Kysect.PowerShellRunner.Cmdlets;
using Microsoft.Extensions.DependencyInjection;

namespace Kysect.PowerShellRunner.Configuration;

public static class PowerShellServiceCollectionExtensions
{
    public static IServiceCollection AddPowerShellLogger(this IServiceCollection serviceCollection, Action<LogConfigurationBuilder> configurationAction)
    {
        serviceCollection.ThrowIfNull();
        configurationAction.ThrowIfNull();

        using var logConfigurationBuilder = new LogConfigurationBuilder();
        configurationAction(logConfigurationBuilder);
        var powerShellLogger = new PowerShellLogger(logConfigurationBuilder.Build());
        return serviceCollection.AddSingleton<IPowerShellLogger>(powerShellLogger);
    }

    public static IServiceCollection AddPowerShellAccessorFactory(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddSingleton<IPowerShellAccessorFactory, PowerShellAccessorFactory>();
    }

    public static IServiceCollection AddPowerShellAccessor(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddSingleton(GetPowerShellAccessor);
    }

    public static IServiceCollection AddPowerShellCmdletExecutor(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddSingleton<IPowerShellCmdletExecutor, PowerShellCmdletExecutor>();
    }

    private static IPowerShellAccessor GetPowerShellAccessor(IServiceProvider serviceProvider)
    {
        IPowerShellLogger powerShellLogger = serviceProvider.GetRequiredService<IPowerShellLogger>();
        IPowerShellAccessorFactory powerShellAccessorFactory = serviceProvider.GetRequiredService<IPowerShellAccessorFactory>();

        IPowerShellAccessor basePowerShellAccessor = powerShellAccessorFactory.Create();
        var basePowerShellAccessorLogDecorator = new PowerShellAccessorLoggingDecorator(basePowerShellAccessor, powerShellLogger);

        return basePowerShellAccessorLogDecorator;
    }
}