using Kysect.CommonLib.BaseTypes.Extensions;

namespace Kysect.PowerShellRunner.CodeGeneration.UsedModelSearching;

public class SimpleModelPropertyDescriptor
{
    public string Type { get; set; }
    public string Name { get; set; }

    public SimpleModelPropertyDescriptor(string type, string name)
    {
        Type = type.ThrowIfNull(nameof(type));
        Name = name.ThrowIfNull(nameof(name));
    }
}