using Kysect.PowerShellRunner.CodeGeneration.Common.Semantic;

namespace Kysect.PowerShellRunner.CodeGeneration.UsedModelSearching;

public class RoslynSimpleModelSemanticDescriptor
{
    public string Name { get; }
    public RoslynTypeSymbolWrapper? BaseType { get; }
    public string? TypeNamespace { get; }
    public IReadOnlyCollection<RoslynPropertySymbolWrapper> PublicProperties { get; }
    public IReadOnlyCollection<RoslynPropertySymbolWrapper> NonPublicProperties { get; }

    public RoslynSimpleModelSemanticDescriptor(
        string name,
        RoslynTypeSymbolWrapper? baseType,
        string? typeNamespace,
        IReadOnlyCollection<RoslynPropertySymbolWrapper> publicProperties,
        IReadOnlyCollection<RoslynPropertySymbolWrapper> nonPublicProperties)
    {
        Name = name;
        BaseType = baseType;
        TypeNamespace = typeNamespace;
        PublicProperties = publicProperties;
        NonPublicProperties = nonPublicProperties;
    }
}