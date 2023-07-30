using Kysect.PowerShellRunner.Abstractions.Accessors;
using Kysect.PowerShellRunner.Abstractions.Accessors.Results;
using Kysect.PowerShellRunner.Abstractions.Queries;
using System;

namespace Kysect.PowerShellRunner.Core.Decorators.Initialization;

public class PowerShellAccessorInitializationDecorator : IPowerShellAccessor
{
    private readonly Lazy<IPowerShellAccessor> _initializedAccessor;

    public PowerShellAccessorInitializationDecorator(IPowerShellAccessor powerShellAccessor, IPowerShellAccessorInitializer initializer)
    {
        _initializedAccessor = new Lazy<IPowerShellAccessor>(() => Initialize(powerShellAccessor, initializer));
    }

    public IPowerShellExecutionResult Execute(PowerShellQuery query)
    {
        return _initializedAccessor.Value.Execute(query);
    }

    public static IPowerShellAccessor Initialize(IPowerShellAccessor accessor, IPowerShellAccessorInitializer initializer)
    {
        initializer.Initialize(accessor);
        return accessor;
    }

    public void Dispose()
    {
        if (_initializedAccessor.IsValueCreated)
            _initializedAccessor.Value.Dispose();
    }
}