using Kysect.PowerShellRunner.Abstractions.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kysect.PowerShellRunner.Abstractions.Variables;

public readonly struct PowerShellReference : IPowerShellCmdletParameterValue
{
    public string Name { get; }

    public PowerShellReference(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Name is null or empty", nameof(name));

        if (!name.StartsWith("$"))
            throw new ArgumentException("Argument should start with $", nameof(name));

        Name = name;
    }

    public override string ToString()
    {
        return Name;
    }
}

public readonly struct PowerShellReferenceCollection : IPowerShellCmdletParameterValue
{
    public string Name { get; }

    public static PowerShellReferenceCollection Create<T>(IReadOnlyCollection<IPowerShellReferenceable<T>> values)
    {
        var references = values
            .Select(v => v.AsReference())
            .ToList();

        return new PowerShellReferenceCollection(references);
    }

    public PowerShellReferenceCollection(IReadOnlyCollection<PowerShellReference> references)
    {
        Name = string.Join(",", references.Select(v => v.Name));
    }

    public override string ToString()
    {
        return Name;
    }
}