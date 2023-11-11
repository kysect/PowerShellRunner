using Kysect.PowerShellRunner.CodeGeneration.Common.Semantic;
using System.Collections.Generic;

namespace Kysect.PowerShellRunner.CodeGeneration.UsedModelSearching;

public class RoslynSimpleModelSemanticDescriptor
{
    public string Name { get; }
    public RoslynTypeSymbolWrapper? BaseType { get; }
    public IReadOnlyCollection<RoslynPropertySymbolWrapper> PublicProperties { get; }
    public IReadOnlyCollection<RoslynPropertySymbolWrapper> NonPublicProperties { get; }

    public RoslynSimpleModelSemanticDescriptor(string name, RoslynTypeSymbolWrapper? baseType, IReadOnlyCollection<RoslynPropertySymbolWrapper> publicProperties, IReadOnlyCollection<RoslynPropertySymbolWrapper> nonPublicProperties)
    {
        Name = name;
        BaseType = baseType;
        PublicProperties = publicProperties;
        NonPublicProperties = nonPublicProperties;
    }
}