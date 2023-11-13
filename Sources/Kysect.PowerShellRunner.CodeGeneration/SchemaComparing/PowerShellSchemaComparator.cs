using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.PowerShellRunner.CodeGeneration.Common.Semantic;
using Kysect.PowerShellRunner.CodeGeneration.SchemaGenerating;
using Kysect.PowerShellRunner.CodeGeneration.SemanticParsing;
using System.Collections.Generic;
using System.Linq;

namespace Kysect.PowerShellRunner.CodeGeneration.SchemaComparing;

public class PowerShellSchemaComparator
{
    public PowerShellSchemaComparingResult<TSemantic> Compare<TSemantic>(PowerShellSemanticSchema<TSemantic> oldSchema, PowerShellSemanticSchema<TSemantic> newSchema)
        where TSemantic : CmdletBaseSemanticInfo
    {
        oldSchema.ThrowIfNull();
        newSchema.ThrowIfNull();

        var added = new List<TSemantic>();
        var removed = new List<TSemantic>();
        var changed = new List<PowerShellSchemaCmdletDiff<TSemantic>>();

        foreach (TSemantic newSchemaCmdlet in newSchema.CmdletDescriptors)
        {
            TSemantic? oldSchemaCmdlet = oldSchema.CmdletDescriptors.FirstOrDefault(c => c.CmdletAttributeValues.GetPowerShellAlias() == newSchemaCmdlet.CmdletAttributeValues.GetPowerShellAlias());

            if (oldSchemaCmdlet is null)
                continue;

            var addedProperties = new List<RoslynPropertySymbolWrapper>();
            var removedProperties = new List<RoslynPropertySymbolWrapper>();

            foreach (RoslynPropertySymbolWrapper newCmdletProperty in newSchemaCmdlet.Properties)
            {
                RoslynPropertySymbolWrapper? secondCmdletProperty = oldSchemaCmdlet.Properties.FirstOrDefault(p => p.Name == newCmdletProperty.Name);
                if (secondCmdletProperty is null)
                {
                    addedProperties.Add(newCmdletProperty);
                }
            }

            foreach (RoslynPropertySymbolWrapper secondCmdletProperty in oldSchemaCmdlet.Properties)
            {
                RoslynPropertySymbolWrapper? firstCmdletProperty = newSchemaCmdlet.Properties.FirstOrDefault(p => p.Name == secondCmdletProperty.Name);
                if (firstCmdletProperty is null)
                {
                    removedProperties.Add(secondCmdletProperty);
                }
            }

            changed.Add(new PowerShellSchemaCmdletDiff<TSemantic>(oldSchemaCmdlet, newSchemaCmdlet, addedProperties, removedProperties));
        }

        foreach (TSemantic firstSchemaCmdlet in newSchema.CmdletDescriptors)
        {
            TSemantic? secondSchemaCmdlet = oldSchema.CmdletDescriptors.FirstOrDefault(c => c.CmdletAttributeValues.GetPowerShellAlias() == firstSchemaCmdlet.CmdletAttributeValues.GetPowerShellAlias());

            if (secondSchemaCmdlet is null)
            {
                added.Add(firstSchemaCmdlet);
            }
        }

        foreach (TSemantic secondSchemaCmdlet in oldSchema.CmdletDescriptors)
        {
            TSemantic? firstSchemaCmdlet = newSchema.CmdletDescriptors.FirstOrDefault(c => c.CmdletAttributeValues.GetPowerShellAlias() == secondSchemaCmdlet.CmdletAttributeValues.GetPowerShellAlias());

            if (firstSchemaCmdlet is null)
            {
                removed.Add(secondSchemaCmdlet);
            }
        }

        return new PowerShellSchemaComparingResult<TSemantic>(added, removed, changed);
    }
}