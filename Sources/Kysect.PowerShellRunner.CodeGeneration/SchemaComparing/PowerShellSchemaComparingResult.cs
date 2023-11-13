using Kysect.PowerShellRunner.CodeGeneration.SemanticParsing;
using System.Collections.Generic;

namespace Kysect.PowerShellRunner.CodeGeneration.SchemaComparing;

public class PowerShellSchemaComparingResult<TSemanticInfo> where TSemanticInfo : CmdletBaseSemanticInfo
{
    public IReadOnlyCollection<TSemanticInfo> AddedCmdlets { get; }
    public IReadOnlyCollection<TSemanticInfo> RemovedCmdlets { get; }
    public IReadOnlyCollection<PowerShellSchemaCmdletDiff<TSemanticInfo>> ChangedCmdlets { get; }

    public PowerShellSchemaComparingResult(
        IReadOnlyCollection<TSemanticInfo> addedCmdlets,
        IReadOnlyCollection<TSemanticInfo> removedCmdlets,
        IReadOnlyCollection<PowerShellSchemaCmdletDiff<TSemanticInfo>> changedCmdlets)
    {
        AddedCmdlets = addedCmdlets;
        RemovedCmdlets = removedCmdlets;
        ChangedCmdlets = changedCmdlets;
    }
}