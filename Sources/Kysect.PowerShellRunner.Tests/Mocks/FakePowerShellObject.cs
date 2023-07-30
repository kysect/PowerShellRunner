using Kysect.CommonLib.Reflection.TypeCache;
using Kysect.PowerShellRunner.Abstractions.Objects;

namespace Kysect.PowerShellRunner.Tests.Mocks;

public class FakePowerShellObject<T> : IPowerShellObject where T : notnull
{
    private readonly T _value;

    public FakePowerShellObject(T value)
    {
        _value = value;
    }

    public IReadOnlyCollection<IPowerShellObjectMember> GetProperties()
    {
        return TypeInstanceCache<T>
            .GetPublicProperties()
            .Select(p => new FakePowerShellObjectMember(p.Name, p.GetValue(_value)))
            .ToList();
    }

    public string AsString()
    {
        return _value.ToString();
    }
}