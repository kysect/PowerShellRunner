using Microsoft.CodeAnalysis;

namespace Kysect.PowerShellRunner.CodeGeneration.UsedModelSearching;

public interface IRoslynSimpleModelBaseTypeFilter
{
    bool Acceptable(ITypeSymbol baseType);
}