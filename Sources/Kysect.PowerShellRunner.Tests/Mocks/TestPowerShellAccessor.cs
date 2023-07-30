using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.PowerShellRunner.Abstractions.Accessors;
using Kysect.PowerShellRunner.Abstractions.Accessors.Results;
using Kysect.PowerShellRunner.Abstractions.Queries;

namespace Kysect.PowerShellRunner.Tests.Mocks;

public class TestPowerShellAccessor : IPowerShellAccessor
{
    private IPowerShellExecutionResult? _result;

    public void SetSuccessResult<T>(T value) where T : notnull
    {
        var powerShellObject = new FakePowerShellObject<T>(value);
        _result = new SuccessPowerShellExecutionResult(new[] { powerShellObject });
    }

    public IPowerShellExecutionResult Execute(PowerShellQuery query)
    {
        _result.ThrowIfNull();

        return _result;
    }

    public void Dispose()
    {
    }
}