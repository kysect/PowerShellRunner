﻿using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.PowerShellRunner.Abstractions.Accessors;
using Kysect.PowerShellRunner.Abstractions.Accessors.Results;
using Kysect.PowerShellRunner.Abstractions.Queries;

namespace Kysect.PowerShellRunner.Tests.Mocks;

public class FakePowerShellAccessor : IPowerShellAccessor
{
    private IPowerShellExecutionResult? _result;

    public void SetSuccessResult<T>(T value) where T : notnull
    {
        var powerShellObject = new FakePowerShellObject<T>(value);
        _result = new PowerShellSuccessExecutionResult(new[] { powerShellObject });
    }

    public void SetFailedResult(string error)
    {
        _result = new PowerShellFailedExecutionResult(new[] { error }, Array.Empty<string>());
    }

    public IPowerShellExecutionResult Execute(PowerShellQuery query)
    {
        _result.ThrowIfNull();

        return _result;
    }

    protected virtual void Dispose(bool disposing)
    {
    }

    public void Dispose()
    {
        Dispose(disposing: true);
    }
}