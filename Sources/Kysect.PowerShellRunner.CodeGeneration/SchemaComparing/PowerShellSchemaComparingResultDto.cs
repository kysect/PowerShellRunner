using Kysect.PowerShellRunner.CodeGeneration.SchemaGenerating;

namespace Kysect.PowerShellRunner.CodeGeneration.SchemaComparing;

public class PowerShellSchemaComparingResultDto
{
    public IReadOnlyCollection<PowerShellCmdletDescriptor> AddedCmdlets { get; }
    public IReadOnlyCollection<PowerShellCmdletDescriptor> RemovedCmdlets { get; }
    public IReadOnlyCollection<PowerShellSchemaCmdletDiffDto> ChangedCmdlets { get; }

    public PowerShellSchemaComparingResultDto(
        IReadOnlyCollection<PowerShellCmdletDescriptor> addedCmdlets,
        IReadOnlyCollection<PowerShellCmdletDescriptor> removedCmdlets,
        IReadOnlyCollection<PowerShellSchemaCmdletDiffDto> changedCmdlets)
    {
        AddedCmdlets = addedCmdlets;
        RemovedCmdlets = removedCmdlets;
        ChangedCmdlets = changedCmdlets;
    }
}