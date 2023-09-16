using Kysect.PowerShellRunner.Abstractions.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kysect.PowerShellRunner.Abstractions.Variables;

public readonly struct PowerShellReferences : IPowerShellCmdletParameterValue, IEquatable<PowerShellReferences>
{
    public string Name { get; }

    public static PowerShellReferences Create<T>(IReadOnlyCollection<IPowerShellReferenceable<T>> values)
    {
        var references = values
            .Select(v => v.AsReference())
            .ToList();

        return new PowerShellReferences(references);
    }

    public PowerShellReferences(IReadOnlyCollection<PowerShellReference> references)
    {
        Name = string.Join(",", references.Select(v => v.Name));
    }

    public override string ToString()
    {
        return Name;
    }

    public override bool Equals(object? obj)
    {
        return obj is PowerShellReferences other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name);
    }

    public static bool operator ==(PowerShellReferences left, PowerShellReferences right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(PowerShellReferences left, PowerShellReferences right)
    {
        return !(left == right);
    }

    public bool Equals(PowerShellReferences other)
    {
        return Name.Equals(other.Name);
    }
}