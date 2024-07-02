using Kysect.PowerShellRunner.Abstractions.Accessors;
using Kysect.PowerShellRunner.Abstractions.Cmdlets;
using Kysect.PowerShellRunner.Abstractions.Objects;
using Kysect.PowerShellRunner.Abstractions.Queries;
using Kysect.PowerShellRunner.Abstractions.Variables;
using Kysect.PowerShellRunner.Mapping;
using Kysect.PowerShellRunner.QueryBuilding;

namespace Kysect.PowerShellRunner.Executions;

public class PowerShellAccessorCmdletExecutionContext<T> where T : notnull
{
    private readonly IPowerShellAccessor _accessor;
    private readonly IPowerShellCmdlet<T> _cmdlet;
    private readonly List<Func<PowerShellQuery, PowerShellQuery>> _morphisms;
    private readonly PowerShellObjectMapper _powerShellObjectMapper;

    public PowerShellAccessorCmdletExecutionContext(IPowerShellAccessor accessor, IPowerShellCmdlet<T> cmdlet)
    {
        _cmdlet = cmdlet;
        _accessor = accessor;

        _morphisms = new List<Func<PowerShellQuery, PowerShellQuery>>();
        _powerShellObjectMapper = PowerShellObjectMapper.Instance;
    }

    public PowerShellAccessorCmdletExecutionContext<T> Continue(Func<PowerShellQuery, PowerShellQuery> morphism)
    {
        _morphisms.Add(morphism);
        return this;
    }

    public IReadOnlyCollection<T> Execute()
    {
        PowerShellQuery powerShellQuery = BuildQuery();

        return _accessor.ExecuteAndGet(powerShellQuery)
            .Select(_powerShellObjectMapper.Map<T>)
            .ToList();
    }

    public PowerShellVariable<T> ExecuteAndSetTo(string variableName)
    {
        var powerShellVariable = new PowerShellVariable(variableName);

        PowerShellQuery powerShellQuery = BuildQuery();

        return new PowerShellVariableInitializer(_accessor, powerShellVariable, _powerShellObjectMapper).With<T>(powerShellQuery);
    }

    private PowerShellQuery BuildQuery()
    {
        PowerShellQuery powerShellQuery = _cmdlet.BuildFromCmdlet();
        _morphisms.ForEach(morphism => powerShellQuery = morphism(powerShellQuery));
        return powerShellQuery;
    }
}

public class PowerShellAccessorCmdletExecutionContext
{
    private readonly IPowerShellAccessor _accessor;
    private readonly IPowerShellCmdlet _cmdlet;
    private readonly List<Func<PowerShellQuery, PowerShellQuery>> _morphisms;
    private readonly PowerShellObjectMapper _powerShellObjectMapper;

    public PowerShellAccessorCmdletExecutionContext(IPowerShellAccessor accessor, IPowerShellCmdlet cmdlet)
    {
        _cmdlet = cmdlet;
        _accessor = accessor;

        _morphisms = new List<Func<PowerShellQuery, PowerShellQuery>>();
        _powerShellObjectMapper = PowerShellObjectMapper.Instance;
    }

    public PowerShellAccessorCmdletExecutionContext Continue(Func<PowerShellQuery, PowerShellQuery> morphism)
    {
        _morphisms.Add(morphism);
        return this;
    }

    public IReadOnlyCollection<IPowerShellObject> Execute()
    {
        PowerShellQuery powerShellQuery = BuildQuery();
        return _accessor.ExecuteAndGet(powerShellQuery);
    }

    public PowerShellVariable ExecuteAndSetTo(string variableName)
    {
        var powerShellVariable = new PowerShellVariable(variableName);
        PowerShellQuery powerShellQuery = BuildQuery();
        return new PowerShellVariableInitializer(_accessor, powerShellVariable, _powerShellObjectMapper).With(powerShellQuery);
    }

    private PowerShellQuery BuildQuery()
    {
        PowerShellQuery powerShellQuery = _cmdlet.BuildFromCmdlet();
        _morphisms.ForEach(morphism => powerShellQuery = morphism(powerShellQuery));
        return powerShellQuery;
    }
}