using Kysect.PowerShellRunner.Abstractions.Objects;

namespace Kysect.PowerShellRunner.Tests.Mocks;

public class FakePowerShellObjectMember : IPowerShellObjectMember
{
    public string Name { get; }
    public object? Value { get; }

    public FakePowerShellObjectMember(string name, object? value)
    {
        Name = name;
        Value = value;
    }
}