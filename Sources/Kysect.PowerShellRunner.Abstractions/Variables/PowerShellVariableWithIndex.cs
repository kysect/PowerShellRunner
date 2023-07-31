using System.Linq;

namespace Kysect.PowerShellRunner.Abstractions.Variables;

public class PowerShellVariableWithIndex<T> : IPowerShellReferenceable<T>
{
    public PowerShellVariable<T> Source { get; }
    public int Index { get; }

    public PowerShellVariableWithIndex(PowerShellVariable<T> source, int index)
    {
        Source = source;
        Index = index;
    }

    public T GetValue()
    {
        return Source.Values.ElementAt(Index);
    }

    public PowerShellReference AsReference()
    {
        return new PowerShellReference($"{Source.AsReference()}[{Index}]");
    }

    public override string ToString()
    {
        return AsReference().ToString();
    }
}