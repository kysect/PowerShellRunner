using Kysect.PowerShellRunner.Abstractions.Accessors;
using Kysect.PowerShellRunner.Core.Decorators.Initialization;
using Microsoft.Extensions.Logging;

namespace Kysect.PowerShellRunner.Core.Decorators;

public class PowerShellAccessorFactoryWithLoggingAndInitialization : IPowerShellAccessorFactory
{
    private readonly IPowerShellAccessorFactory _defaultAccessorFactory;
    private readonly IPowerShellAccessorInitializer _initializer;
    private readonly ILogger _logger;

    public PowerShellAccessorFactoryWithLoggingAndInitialization(
        IPowerShellAccessorFactory defaultAccessorFactory,
        IPowerShellAccessorInitializer initializer,
        ILogger logger)
    {
        _defaultAccessorFactory = defaultAccessorFactory;
        _initializer = initializer;
        _logger = logger;
    }

    public IPowerShellAccessor Create()
    {
        return new PowerShellAccessorDecoratorBuilder(_defaultAccessorFactory)
            .WithLogging(_logger)
            .WithInitialization(_initializer)
            .Build();
    }
}