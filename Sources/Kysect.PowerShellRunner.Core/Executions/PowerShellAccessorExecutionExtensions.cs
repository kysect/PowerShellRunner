using Kysect.PowerShellRunner.Abstractions.Accessors;
using Kysect.PowerShellRunner.Abstractions.Objects;
using Kysect.PowerShellRunner.Abstractions.Queries;
using Kysect.PowerShellRunner.Abstractions.Variables;
using System.Collections.Generic;

namespace Kysect.PowerShellRunner.Core.Executions;

public static class PowerShellAccessorExecutionExtensions
{
    /// <summary>
    /// Main extensions that used in all other methods
    /// </summary>
    public static IReadOnlyCollection<IPowerShellObject> ExecuteAndGet(this IPowerShellAccessor accessor, PowerShellQuery query)
    {
        return accessor.Execute(query).EnsureNoErrors();
    }

    public static IReadOnlyCollection<IPowerShellObject> ExecuteRaw(this IPowerShellAccessor accessor, PowerShellQuery query)
    {
        return accessor.ExecuteAndGet(query);
    }

    public static IReadOnlyCollection<IPowerShellObject> GetVariableVale(this IPowerShellAccessor accessor, PowerShellVariable variable)
    {
        return accessor.ExecuteRaw(new PowerShellQuery(variable.AsReference().ParameterName));
    }
}