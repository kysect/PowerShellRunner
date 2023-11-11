namespace Kysect.PowerShellRunner.CodeGeneration.UsedModelSearching;

public class ModelEnumTypeMemberDescriptor
{
    public string Identifier { get; }
    public int? Value { get; }

    public ModelEnumTypeMemberDescriptor(string identifier) : this(identifier, null)
    {
    }

    public ModelEnumTypeMemberDescriptor(string identifier, int? value)
    {
        Identifier = identifier;
        Value = value;
    }
}