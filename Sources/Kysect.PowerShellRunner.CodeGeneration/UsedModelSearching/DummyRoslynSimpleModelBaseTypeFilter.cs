using Microsoft.CodeAnalysis;

namespace Kysect.PowerShellRunner.CodeGeneration.UsedModelSearching;

public class DummyRoslynSimpleModelBaseTypeFilter : IRoslynSimpleModelBaseTypeFilter
{
    public bool Acceptable(ITypeSymbol baseType)
    {
        return true;
    }
}