using Kysect.PowerShellRunner.Abstractions.Accessors;
using Kysect.PowerShellRunner.Abstractions.Cmdlets;
using Kysect.PowerShellRunner.Abstractions.Objects;
using Kysect.PowerShellRunner.Abstractions.Queries;
using Kysect.PowerShellRunner.Abstractions.Variables;
using Kysect.PowerShellRunner.Executions;
using Kysect.PowerShellRunner.QueryBuilding;

namespace Kysect.PowerShellRunner.Cmdlets;

public class PowerShellCmdletExecutor(IPowerShellAccessor accessor, IPowerShellObjectMapper powerShellObjectMapper)
    : IPowerShellCmdletExecutor
{
    public IReadOnlyCollection<IPowerShellObject> Execute(IPowerShellCmdlet cmdlet)
    {
        PowerShellQuery powerShellQuery = cmdlet.BuildFromCmdlet();

        return accessor.ExecuteAndGet(powerShellQuery).ToList();
    }

    public PowerShellVariable<IPowerShellObject> InitializeVariable(string variableName, IPowerShellCmdlet cmdlet)
    {
        var powerShellVariable = new PowerShellVariable(variableName);
        PowerShellQuery powerShellQuery = cmdlet.BuildFromCmdlet();
        return new PowerShellVariableInitializer(accessor, powerShellVariable, powerShellObjectMapper).With(powerShellQuery);
    }

    public IReadOnlyCollection<T> Execute<T>(IPowerShellCmdlet<T> cmdlet) where T : notnull
    {
        PowerShellQuery powerShellQuery = cmdlet.BuildFromCmdlet();

        return accessor.ExecuteAndGet(powerShellQuery)
            .Select(powerShellObjectMapper.Map<T>)
            .ToList();
    }

    public PowerShellVariable<T> InitializeVariable<T>(string variableName, IPowerShellCmdlet<T> cmdlet) where T : notnull
    {
        var powerShellVariable = new PowerShellVariable(variableName);
        PowerShellQuery powerShellQuery = cmdlet.BuildFromCmdlet();
        return new PowerShellVariableInitializer(accessor, powerShellVariable, powerShellObjectMapper).With<T>(powerShellQuery);
    }
}