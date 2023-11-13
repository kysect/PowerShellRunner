using Kysect.PowerShellRunner.CodeGeneration.Common.Semantic;
using Kysect.PowerShellRunner.CodeGeneration.SemanticParsing;
using System.Collections.Generic;

namespace Kysect.PowerShellRunner.CodeGeneration.SchemaComparing;

public class PowerShellSchemaCmdletDiff<TSemanticInfo> where TSemanticInfo : CmdletBaseSemanticInfo
{
    public TSemanticInfo OldCmdlet { get; }
    public TSemanticInfo NewCmdlet { get; }
    public IReadOnlyCollection<RoslynPropertySymbolWrapper> AddedProperties { get; }
    public IReadOnlyCollection<RoslynPropertySymbolWrapper> RemovedProperties { get; }

    public PowerShellSchemaCmdletDiff(
        TSemanticInfo oldCmdlet,
        TSemanticInfo newCmdlet,
        IReadOnlyCollection<RoslynPropertySymbolWrapper> addedProperties,
        IReadOnlyCollection<RoslynPropertySymbolWrapper> removedProperties)
    {
        OldCmdlet = oldCmdlet;
        NewCmdlet = newCmdlet;
        AddedProperties = addedProperties;
        RemovedProperties = removedProperties;
    }
}