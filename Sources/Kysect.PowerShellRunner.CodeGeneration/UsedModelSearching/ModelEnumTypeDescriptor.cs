namespace Kysect.PowerShellRunner.CodeGeneration.UsedModelSearching;

public class ModelEnumTypeDescriptor
{
    public string Name { get; }
    public IReadOnlyCollection<ModelEnumTypeMemberDescriptor> Values { get; }

    public ModelEnumTypeDescriptor(string name, IReadOnlyCollection<ModelEnumTypeMemberDescriptor> values)
    {
        Name = name;
        Values = values;
    }
}