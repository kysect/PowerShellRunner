using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.PowerShellRunner.CodeGeneration.Common.Semantic;
using Kysect.PowerShellRunner.CodeGeneration.Compilation;
using Kysect.PowerShellRunner.CodeGeneration.SemanticParsing;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Kysect.PowerShellRunner.CodeGeneration.UsedModelSearching;

public class CmdletUsedModelSearcher
{
    private readonly RoslynDependentTypeSearcher _dependentTypeSearcher;

    public CmdletUsedModelSearcher(ILogger logger, RoslynSimpleModelSemanticDescriptorFactory simpleModelSemanticDescriptorFactory)
    {
        _dependentTypeSearcher = new RoslynDependentTypeSearcher(logger, simpleModelSemanticDescriptorFactory);
    }

    public RoslynDependentTypeSearcherResult GetAllUsedModels(
        SolutionCompilationContext solutionCompilationContext,
        IReadOnlyCollection<CmdletBaseSemanticInfo> cmdletSemanticParseResults)
    {
        cmdletSemanticParseResults.ThrowIfNull();

        IReadOnlyCollection<RoslynTypeSymbolWrapper> usedTypesInCmdlets = GetAllTypesFromCmdletSignatures(cmdletSemanticParseResults);
        RoslynDependentTypeSearcherResult allUsedModels = _dependentTypeSearcher.LoadAllDependentTypes(solutionCompilationContext, usedTypesInCmdlets);
        return allUsedModels;
    }

    private IReadOnlyCollection<RoslynTypeSymbolWrapper> GetAllTypesFromCmdletSignatures(IReadOnlyCollection<CmdletBaseSemanticInfo> cmdletDescriptors)
    {
        var symbols = new List<RoslynTypeSymbolWrapper>();

        foreach (CmdletBaseSemanticInfo cmdletTypeDescriptor in cmdletDescriptors)
        {
            symbols.AddRange(cmdletTypeDescriptor.Properties.Select(p => p.Type));
            if (cmdletTypeDescriptor.MainReturnType is not null)
                symbols.Add(cmdletTypeDescriptor.MainReturnType);
            symbols.AddRange(cmdletTypeDescriptor.ResolvedReturnTypes);
        }

        return symbols;
    }
}