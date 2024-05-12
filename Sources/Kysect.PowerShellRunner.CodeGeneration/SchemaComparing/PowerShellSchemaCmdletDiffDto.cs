using Kysect.PowerShellRunner.CodeGeneration.SemanticParsing;
using Kysect.PowerShellRunner.CodeGeneration.UsedModelSearching;

namespace Kysect.PowerShellRunner.CodeGeneration.SchemaComparing;

public class PowerShellSchemaCmdletDiffDto
{
    public CmdletAttributeValues AttributeValues { get; }
    public IReadOnlyCollection<SimpleModelPropertyDescriptor> AddedProperties { get; }
    public IReadOnlyCollection<SimpleModelPropertyDescriptor> RemovedProperties { get; }

    public PowerShellSchemaCmdletDiffDto(
        CmdletAttributeValues attributeValues,
        IReadOnlyCollection<SimpleModelPropertyDescriptor> addedProperties,
        IReadOnlyCollection<SimpleModelPropertyDescriptor> removedProperties)
    {
        AttributeValues = attributeValues;
        AddedProperties = addedProperties;
        RemovedProperties = removedProperties;
    }
}