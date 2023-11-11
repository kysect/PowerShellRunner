using Microsoft.CodeAnalysis;

namespace Kysect.PowerShellRunner.CodeGeneration.UsedModelSearching;

public class DummyRoslynSimpleModelPropertyFilter : IRoslynSimpleModelPropertyFilter
{
    public bool Acceptable(IPropertySymbol propertySymbol)
    {
        return true;
    }
}