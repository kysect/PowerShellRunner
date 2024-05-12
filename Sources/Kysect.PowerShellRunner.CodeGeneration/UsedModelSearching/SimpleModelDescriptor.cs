namespace Kysect.PowerShellRunner.CodeGeneration.UsedModelSearching;

public class SimpleModelDescriptor
{
    public string Name { get; set; }
    public string? BaseType { get; set; }
    public IReadOnlyCollection<SimpleModelPropertyDescriptor> PublicProperties { get; set; }
    public IReadOnlyCollection<SimpleModelPropertyDescriptor> NonPublicProperties { get; set; }

    public SimpleModelDescriptor(
        string name,
        string? baseType,
        IReadOnlyCollection<SimpleModelPropertyDescriptor> publicProperties,
        IReadOnlyCollection<SimpleModelPropertyDescriptor> nonPublicProperties)
    {
        Name = name;
        BaseType = baseType;
        PublicProperties = publicProperties;
        NonPublicProperties = nonPublicProperties;
    }
}