using Kysect.PowerShellRunner.CodeGeneration.Common.Semantic;
using Kysect.PowerShellRunner.CodeGeneration.Compilation;
using System.Collections.Generic;

namespace Kysect.PowerShellRunner.CodeGeneration.SemanticParsing;

public class CmdletBaseSemanticInfo
{
    public SolutionCompilationContextItem SolutionCompilationContextItem { get; }
    public CmdletAttributeValues CmdletAttributeValues { get; }
    public IReadOnlyCollection<RoslynPropertySymbolWrapper> Properties { get; }
    public RoslynTypeSymbolWrapper? MainReturnType { get; }
    public IReadOnlyCollection<RoslynTypeSymbolWrapper> ResolvedReturnTypes { get; }

    public CmdletBaseSemanticInfo(
        SolutionCompilationContextItem solutionCompilationContextItem,
        CmdletAttributeValues cmdletAttributeValues,
        IReadOnlyCollection<RoslynPropertySymbolWrapper> properties,
        RoslynTypeSymbolWrapper? mainReturnType,
        IReadOnlyCollection<RoslynTypeSymbolWrapper> resolvedReturnTypes)
    {
        SolutionCompilationContextItem = solutionCompilationContextItem;
        CmdletAttributeValues = cmdletAttributeValues;
        Properties = properties;
        MainReturnType = mainReturnType;
        ResolvedReturnTypes = resolvedReturnTypes;
    }
}