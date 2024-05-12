using System.Diagnostics.CodeAnalysis;

namespace Kysect.PowerShellRunner.Mapping;

public class PowerShellObjectMappingConfiguration
{
    private readonly List<IPowerShellObjectCustomMapping> _mappings;

    public PowerShellObjectMappingConfiguration()
    {
        _mappings = new List<IPowerShellObjectCustomMapping>();
    }

    public void AddMappingToString(string sourceTypeName)
    {
        AddMapping(sourceTypeName, o => o.ToString());
    }

    public void AddMapping(string sourceTypeName, Func<object, object> converter)
    {
        _mappings.Add(new PowerShellObjectCustomMapping(sourceTypeName, converter));
    }

    public bool TryMap(object? value, [NotNullWhen(true)] out object? result)
    {
        if (value is null)
        {
            result = null;
            return false;
        }

        string typeName = value.GetType().FullName;
        IPowerShellObjectCustomMapping powerShellObjectCustomMapping = _mappings.Find(m => m.SourceType == typeName);
        if (powerShellObjectCustomMapping is null)
        {
            result = value;
            return false;
        }

        result = powerShellObjectCustomMapping.Map(value);
        return true;
    }
}