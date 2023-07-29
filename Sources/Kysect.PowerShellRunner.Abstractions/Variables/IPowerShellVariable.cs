namespace Kysect.PowerShellRunner.Abstractions.Variables;

public interface IPowerShellVariable<out T> : IPowerShellReferenceable<T>
{
    public string Name { get; }
}