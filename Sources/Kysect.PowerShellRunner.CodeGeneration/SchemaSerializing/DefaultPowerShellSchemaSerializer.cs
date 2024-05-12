using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.PowerShellRunner.CodeGeneration.Common.Semantic;
using Kysect.PowerShellRunner.CodeGeneration.SchemaGenerating;
using Kysect.PowerShellRunner.CodeGeneration.SemanticParsing;
using Kysect.PowerShellRunner.CodeGeneration.UsedModelSearching;

namespace Kysect.PowerShellRunner.CodeGeneration.SchemaSerializing;

public class DefaultPowerShellSchemaSerializer : IPowerShellSchemaSerializer<CmdletBaseSemanticInfo>
{
    public PowerShellSchemaDto ConvertSchemaToSerializable(PowerShellSemanticSchema<CmdletBaseSemanticInfo> schema)
    {
        schema.ThrowIfNull();

        IReadOnlyCollection<PowerShellCmdletDescriptor> cmdletDescriptors = schema
            .CmdletDescriptors
            .Select(ConvertCmdletToSerializable)
            .ToList();

        var models = schema
            .Models
            .Select(ConvertModelToSerializable)
            .ToList();

        return new PowerShellSchemaDto(cmdletDescriptors, models, schema.Enums);
    }

    public PowerShellCmdletDescriptor ConvertCmdletToSerializable(CmdletBaseSemanticInfo semanticInfo)
    {
        semanticInfo.ThrowIfNull();

        IReadOnlyCollection<SimpleModelPropertyDescriptor> properties = WrapProperties(semanticInfo.Properties);

        string? originalReturnTypes = semanticInfo.MainReturnType?.GetNameWithContainingParent();
        var resolvedReturnTypes = semanticInfo.ResolvedReturnTypes.Select(t => t.GetNameWithContainingParent()).ToList();

        // TODO: allow customization
        IReadOnlyList<string> subScopeNames = new[] { "Cmdlets" };
        return new PowerShellCmdletDescriptor(
            subScopeNames,
            semanticInfo.CmdletAttributeValues,
            properties,
            originalReturnTypes,
            resolvedReturnTypes);
    }

    public SimpleModelDescriptor ConvertModelToSerializable(RoslynSimpleModelSemanticDescriptor model)
    {
        model.ThrowIfNull();

        return new SimpleModelDescriptor(
            model.Name,
            model.BaseType?.GetNameWithContainingParent(),
            WrapProperties(model.PublicProperties),
            WrapProperties(model.NonPublicProperties));
    }

    public static IReadOnlyCollection<SimpleModelPropertyDescriptor> WrapProperties(IReadOnlyCollection<RoslynPropertySymbolWrapper> properties)
    {
        return properties
            .Select(p => new SimpleModelPropertyDescriptor(p.TypeName, p.Name))
            .ToList();
    }
}