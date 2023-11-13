using Kysect.PowerShellRunner.CodeGeneration.UsedModelSearching;
using System.Collections.Generic;

namespace Kysect.PowerShellRunner.CodeGeneration.SchemaGenerating;

public class PowerShellSchemaDto
{
    public IReadOnlyCollection<PowerShellCmdletDescriptor> CmdletDescriptors { get; set; }
    public IReadOnlyCollection<SimpleModelDescriptor> Models { get; set; }
    public IReadOnlyCollection<ModelEnumTypeDescriptor> Enums { get; }

    public PowerShellSchemaDto(
        IReadOnlyCollection<PowerShellCmdletDescriptor> cmdletDescriptors,
        IReadOnlyCollection<SimpleModelDescriptor> models,
        IReadOnlyCollection<ModelEnumTypeDescriptor> enums)
    {
        CmdletDescriptors = cmdletDescriptors;
        Models = models;
        Enums = enums;
    }
}