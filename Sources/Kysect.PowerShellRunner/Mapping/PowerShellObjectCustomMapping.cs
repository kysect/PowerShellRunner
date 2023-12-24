using System;

namespace Kysect.PowerShellRunner.Mapping;

public class PowerShellObjectCustomMapping : IPowerShellObjectCustomMapping
{
    private readonly Func<object, object> _converter;

    public string SourceType { get; }

    public PowerShellObjectCustomMapping(string sourceType, Func<object, object> converter)
    {
        SourceType = sourceType;
        _converter = converter;
    }

    public object Map(object sourceValue)
    {
        return _converter(sourceValue);
    }
}