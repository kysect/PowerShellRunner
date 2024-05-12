using Kysect.PowerShellRunner.CodeGeneration.SemanticParsing;
using Kysect.PowerShellRunner.CodeGeneration.UsedModelSearching;

namespace Kysect.PowerShellRunner.CodeGeneration.SchemaGenerating;

public class PowerShellSemanticSchema : PowerShellSemanticSchema<CmdletBaseSemanticInfo>
{
    public PowerShellSemanticSchema(
        IReadOnlyCollection<CmdletBaseSemanticInfo> cmdletDescriptors,
        IReadOnlyCollection<RoslynSimpleModelSemanticDescriptor> models,
        IReadOnlyCollection<ModelEnumTypeDescriptor> enums)
        : base(cmdletDescriptors, models, enums)
    {
    }
}

public class PowerShellSemanticSchema<T> where T : CmdletBaseSemanticInfo
{
    public IReadOnlyCollection<T> CmdletDescriptors { get; set; }
    public IReadOnlyCollection<RoslynSimpleModelSemanticDescriptor> Models { get; set; }
    public IReadOnlyCollection<ModelEnumTypeDescriptor> Enums { get; }

    public PowerShellSemanticSchema(
        IReadOnlyCollection<T> cmdletDescriptors,
        IReadOnlyCollection<RoslynSimpleModelSemanticDescriptor> models,
        IReadOnlyCollection<ModelEnumTypeDescriptor> enums)
    {
        CmdletDescriptors = cmdletDescriptors;
        Models = models;
        Enums = enums;
    }
}