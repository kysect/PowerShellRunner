using Kysect.PowerShellRunner.Abstractions.Parameters;

namespace Kysect.PowerShellRunner.Abstractions.Variables;

public readonly struct PowerShellReference : IPowerShellCmdletParameterValue, IEquatable<PowerShellReference>
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

    public override bool Equals(object? obj)
    {
        return obj is PowerShellReference reference &&
               Equals(reference);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name);
    }

    public bool Equals(PowerShellReference other)
    {
        return Name == other.Name;
    }

    public static bool operator ==(PowerShellReference left, PowerShellReference right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(PowerShellReference left, PowerShellReference right)
    {
        return !(left == right);
    }
}