using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.PowerShellRunner.CodeGeneration.SchemaSerializing;
using Kysect.PowerShellRunner.CodeGeneration.SemanticParsing;
using Kysect.PowerShellRunner.CodeGeneration.UsedModelSearching;
using System.Collections.Generic;
using System.Linq;

namespace Kysect.PowerShellRunner.CodeGeneration.SchemaComparing;

public class PowerShellSchemaComparingResultSerializer<TSemanticInfo> where TSemanticInfo : CmdletBaseSemanticInfo
{
    private readonly IPowerShellSchemaSerializer<TSemanticInfo> _schemaSerializer;

    public PowerShellSchemaComparingResultSerializer(IPowerShellSchemaSerializer<TSemanticInfo> schemaSerializer)
    {
        _schemaSerializer = schemaSerializer;
    }

    public PowerShellSchemaComparingResultDto SerializeCompareResult(PowerShellSchemaComparingResult<TSemanticInfo> result)
    {
        result.ThrowIfNull();

        var added = result
            .AddedCmdlets
            .Select(_schemaSerializer.ConvertCmdletToSerializable)
            .ToList();

        var removed = result
            .RemovedCmdlets
            .Select(_schemaSerializer.ConvertCmdletToSerializable)
            .ToList();

        var changed = result
            .ChangedCmdlets
            .Select(ConvertSchemaDiffToSerializable)
            .ToList();

        return new PowerShellSchemaComparingResultDto(added, removed, changed);
    }

    public PowerShellSchemaCmdletDiffDto ConvertSchemaDiffToSerializable(PowerShellSchemaCmdletDiff<TSemanticInfo> model)
    {
        model.ThrowIfNull();

        IReadOnlyCollection<SimpleModelPropertyDescriptor> addedProperties = DefaultPowerShellSchemaSerializer.WrapProperties(model.AddedProperties);
        IReadOnlyCollection<SimpleModelPropertyDescriptor> removedProperties = DefaultPowerShellSchemaSerializer.WrapProperties(model.AddedProperties);

        return new PowerShellSchemaCmdletDiffDto(
            model.NewCmdlet.CmdletAttributeValues,
            addedProperties,
            removedProperties);
    }
}