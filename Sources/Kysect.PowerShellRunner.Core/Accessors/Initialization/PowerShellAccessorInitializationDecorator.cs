using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.PowerShellRunner.Abstractions.Accessors;
using Kysect.PowerShellRunner.Abstractions.Accessors.Results;
using Kysect.PowerShellRunner.Abstractions.Queries;
using System;

namespace Kysect.PowerShellRunner.Core.Accessors.Initialization;

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
        accessor.ThrowIfNull();
        initializer.ThrowIfNull();

        initializer.Initialize(accessor);
        return accessor;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_initializedAccessor.IsValueCreated)
            _initializedAccessor.Value.Dispose();
    }

    public void Dispose()
    {
        Dispose(disposing: true);
    }
}