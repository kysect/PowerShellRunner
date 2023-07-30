namespace Kysect.PowerShellRunner.Abstractions.Objects;

public interface IPowerShellObjectMember
{
    string Name { get; }
    object? Value { get; }
}