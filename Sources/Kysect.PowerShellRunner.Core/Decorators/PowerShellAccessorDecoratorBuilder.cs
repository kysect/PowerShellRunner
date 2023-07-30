using Kysect.PowerShellRunner.Abstractions.Accessors;
using Kysect.PowerShellRunner.Core.Decorators.Initialization;
using Microsoft.Extensions.Logging;

namespace Kysect.PowerShellRunner.Core.Decorators;

#pragma warning disable CA1001
public class PowerShellAccessorDecoratorBuilder
#pragma warning restore CA1001
{
    private IPowerShellAccessor _powerShellAccessor;

    public PowerShellAccessorDecoratorBuilder(IPowerShellAccessorFactory powerShellAccessorFactory)
    {
        _powerShellAccessor = powerShellAccessorFactory.Create();
    }

    public PowerShellAccessorDecoratorBuilder WithLogging(ILogger logger)
    {
        _powerShellAccessor = new PowerShellAccessorLoggingDecorator(_powerShellAccessor, logger);
        return this;
    }

    public PowerShellAccessorDecoratorBuilder WithInitialization(IPowerShellAccessorInitializer initializer)
    {
        _powerShellAccessor = new PowerShellAccessorInitializationDecorator(_powerShellAccessor, initializer);
        return this;
    }

    public IPowerShellAccessor Build()
    {
        return _powerShellAccessor;
    }
}