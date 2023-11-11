using System.Collections.Generic;

namespace Kysect.PowerShellRunner.CodeGeneration.UsedModelSearching;

public class RoslynDependentTypeSearcherResult
{
    public IReadOnlyCollection<RoslynSimpleModelSemanticDescriptor> Models { get; }
    public IReadOnlyCollection<ModelEnumTypeDescriptor> Enums { get; }

    public RoslynDependentTypeSearcherResult(IReadOnlyCollection<RoslynSimpleModelSemanticDescriptor> models, IReadOnlyCollection<ModelEnumTypeDescriptor> enums)
    {
        Models = models;
        Enums = enums;
    }
}