using Kysect.PowerShellRunner.Abstractions.Accessors;
using Kysect.PowerShellRunner.Abstractions.Accessors.Results;
using Kysect.PowerShellRunner.Abstractions.Queries;

namespace Kysect.PowerShellRunner.Core.Decorators;

public class PowerShellResettableAccessor : IPowerShellAccessor
{
    private readonly IPowerShellAccessorFactory _powerShellAccessorFactory;

    private IPowerShellAccessor? _powerShellAccessor;

    public PowerShellResettableAccessor(IPowerShellAccessorFactory powerShellAccessorFactory)
    {
        _powerShellAccessorFactory = powerShellAccessorFactory;
    }

    public IPowerShellExecutionResult Execute(PowerShellQuery query)
    {
        return GetOrCreate().Execute(query);
    }

    public void Release()
    {
        _powerShellAccessor?.Dispose();
        _powerShellAccessor = null;
    }

    private IPowerShellAccessor GetOrCreate()
    {
        if (_powerShellAccessor is null)
            _powerShellAccessor = _powerShellAccessorFactory.Create();

        return _powerShellAccessor;
    }

    protected virtual void Dispose(bool disposing)
    {
        Release();
    }

    public void Dispose()
    {
        Dispose(disposing: true);
    }
}