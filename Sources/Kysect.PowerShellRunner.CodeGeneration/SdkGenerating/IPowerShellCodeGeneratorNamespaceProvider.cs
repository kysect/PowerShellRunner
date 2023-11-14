using Kysect.PowerShellRunner.CodeGeneration.SchemaGenerating;
using Kysect.PowerShellRunner.CodeGeneration.UsedModelSearching;

namespace Kysect.PowerShellRunner.CodeGeneration.SdkGenerating;

public interface IPowerShellCodeGeneratorNamespaceProvider
{
    string[] GetForEnum(ModelEnumTypeDescriptor enumTypeDescriptor);
    string[] GetForModel(SimpleModelDescriptor simpleModelDescriptor);
    string[] GetForCmdlet(PowerShellCmdletDescriptor cmdletDescriptor);
}