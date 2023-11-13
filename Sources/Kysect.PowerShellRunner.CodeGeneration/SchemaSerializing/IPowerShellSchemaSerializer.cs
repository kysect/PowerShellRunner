using Kysect.PowerShellRunner.CodeGeneration.SchemaGenerating;
using Kysect.PowerShellRunner.CodeGeneration.SemanticParsing;

namespace Kysect.PowerShellRunner.CodeGeneration.SchemaSerializing;

public interface IPowerShellSchemaSerializer<TSemantic> where TSemantic : CmdletBaseSemanticInfo
{
    PowerShellSchemaDto ConvertSchemaToSerializable(PowerShellSemanticSchema<TSemantic> schema);
    PowerShellCmdletDescriptor ConvertCmdletToSerializable(TSemantic semanticInfo);
}