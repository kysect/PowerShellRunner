using Microsoft.Extensions.Logging;
using System;

namespace Kysect.PowerShellRunner.Configuration;

public interface IPowerShellLogger : ILogger
{
}

// TODO: this used for registration more than one logger. dotnet8 provide "named" registration that solve this problem
public class PowerShellLogger : IPowerShellLogger
{
    private readonly ILogger _logger;

    public PowerShellLogger(ILogger logger)
    {
        _logger = logger;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        _logger.Log(logLevel, eventId, state, exception, formatter);
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return _logger.IsEnabled(logLevel);
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return _logger.BeginScope(state);
    }
}