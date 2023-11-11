using Microsoft.CodeAnalysis;

namespace Kysect.PowerShellRunner.CodeGeneration.UsedModelSearching;

public interface IRoslynSimpleModelPropertyFilter
{
    bool Acceptable(IPropertySymbol propertySymbol);
}