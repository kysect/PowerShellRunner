using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.PowerShellRunner.Abstractions.Accessors;
using Kysect.PowerShellRunner.Abstractions.Objects;
using Kysect.PowerShellRunner.Abstractions.Queries;
using Kysect.PowerShellRunner.Abstractions.Variables;
using System.Collections.Generic;

namespace Kysect.PowerShellRunner.Executions;

public static class PowerShellAccessorExecutionExtensions
{
    /// <summary>
    /// Main extensions that used in all other methods
    /// </summary>
    public static IReadOnlyCollection<IPowerShellObject> ExecuteAndGet(this IPowerShellAccessor accessor, PowerShellQuery query)
    {
        accessor.ThrowIfNull();

        return accessor.Execute(query).EnsureNoErrors();
    }

    public static IReadOnlyCollection<IPowerShellObject> ExecuteRaw(this IPowerShellAccessor accessor, PowerShellQuery query)
    {
        accessor.ThrowIfNull();

        return accessor.ExecuteAndGet(query);
    }

    public static IReadOnlyCollection<IPowerShellObject> GetVariableVale(this IPowerShellAccessor accessor, PowerShellVariable variable)
    {
        accessor.ThrowIfNull();
        variable.ThrowIfNull();

        return accessor.ExecuteRaw(new PowerShellQuery(variable.AsReference().Name));
    }
}