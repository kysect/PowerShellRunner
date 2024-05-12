using Kysect.PowerShellRunner.Abstractions.Objects;

namespace Kysect.PowerShellRunner.Abstractions.Variables;

public class PowerShellVariable : PowerShellVariable<IPowerShellObject>
{
    public PowerShellVariable(string name) : base(name)
    {
    }

    public PowerShellVariable(string name, IReadOnlyList<IPowerShellObject> values) : base(name, values)
    {
    }
}

public class PowerShellVariable<T> : IPowerShellVariable<T>
{
    private readonly PowerShellReference _reference;

    public string Name => _reference.Name;
    public IReadOnlyList<T> Values { get; }

    public PowerShellVariable(string name)
    {
        _reference = new PowerShellReference(name);

        Values = new List<T>();
    }

    public PowerShellVariable(string name, IReadOnlyList<T> values)
    {
        _reference = new PowerShellReference(name);

        Values = values;
    }

    public PowerShellReference AsReference()
    {
        return _reference;
    }

    public IEnumerable<PowerShellVariableWithIndex<T>> EnumerateElements()
    {
        for (int index = 0; index < Values.Count; index++)
        {
            yield return new PowerShellVariableWithIndex<T>(this, index);
        }
    }

    public override string ToString()
    {
        return Name;
    }
}