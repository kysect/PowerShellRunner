using System;
using System.Collections.Generic;
using Kysect.PowerShellRunner.Abstractions.Cmdlets;
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
    public string Name { get; }
    public IReadOnlyList<T> Values { get; }

    public PowerShellVariable(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Name is null or empty", nameof(name));

        if (!name.StartsWith("$"))
            throw new ArgumentException("PS variable should start with prefix '$'.");

        Name = name;
        Values = new List<T>();
    }

    public PowerShellVariable(string name, IReadOnlyList<T> values)
    {
        Name = name;
        Values = values;
    }

    public PowerShellCmdletParameterReferenceValue AsReference()
    {
        return new PowerShellCmdletParameterReferenceValue(Name);
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