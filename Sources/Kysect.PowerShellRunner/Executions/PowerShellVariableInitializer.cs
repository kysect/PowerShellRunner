using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.PowerShellRunner.Abstractions.Accessors;
using Kysect.PowerShellRunner.Abstractions.Cmdlets;
using Kysect.PowerShellRunner.Abstractions.Objects;
using Kysect.PowerShellRunner.Abstractions.Queries;
using Kysect.PowerShellRunner.Abstractions.Variables;
using Kysect.PowerShellRunner.QueryBuilding;

namespace Kysect.PowerShellRunner.Executions;

public class PowerShellVariableInitializer
{
    private readonly IPowerShellAccessor _accessor;
    private readonly PowerShellVariable _variable;
    private readonly IPowerShellObjectMapper _powerShellObjectMapper;

    public PowerShellVariableInitializer(IPowerShellAccessor accessor, PowerShellVariable variable, IPowerShellObjectMapper powerShellObjectMapper)
    {
        _accessor = accessor.ThrowIfNull();
        _variable = variable.ThrowIfNull();
        _powerShellObjectMapper = powerShellObjectMapper.ThrowIfNull();
    }

    public PowerShellVariable With(PowerShellQuery query)
    {
        return Execute(query);
    }

    public PowerShellVariable<T> With<T>(PowerShellQuery query) where T : notnull
    {
        PowerShellVariable powerShellVariable = Execute(query);
        return Map<T>(powerShellVariable);
    }

    public PowerShellVariable<T> With<T>(IPowerShellCmdlet cmdletWrapper) where T : notnull
    {
        PowerShellQuery powerShellQuery = cmdletWrapper.BuildFromCmdlet();
        PowerShellVariable powerShellVariable = Execute(powerShellQuery);
        return Map<T>(powerShellVariable);
    }

    public PowerShellVariable<T> With<T>(IPowerShellCmdlet<T> cmdletWrapper) where T : notnull
    {
        PowerShellQuery powerShellQuery = cmdletWrapper.BuildFromCmdlet();
        PowerShellVariable powerShellVariable = Execute(powerShellQuery);
        return Map<T>(powerShellVariable);
    }

    private PowerShellVariable Execute(PowerShellQuery query)
    {
        query = query with { ResultVariable = _variable };

        _accessor.ExecuteAndGet(query);
        IReadOnlyCollection<IPowerShellObject> variableValue = _accessor.GetVariableVale(_variable);
        return new PowerShellVariable(_variable.Name, variableValue.ToList());
    }

    private PowerShellVariable<T> Map<T>(PowerShellVariable powerShellVariable) where T : notnull
    {
        var mappedValues = powerShellVariable
            .Values
            .Select(_powerShellObjectMapper.Map<T>)
            .ToList();

        return new PowerShellVariable<T>(powerShellVariable.Name, mappedValues);
    }
}