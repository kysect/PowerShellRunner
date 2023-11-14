using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.CommonLib.FileSystem.Extensions;
using Kysect.PowerShellRunner.CodeGeneration.SchemaGenerating;
using Kysect.PowerShellRunner.CodeGeneration.UsedModelSearching;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.IO;

namespace Kysect.PowerShellRunner.CodeGeneration.SdkGenerating;

public class PowerShellSchemaCodeGenerator
{
    private readonly string _outputPath;
    private readonly string _namespaceName;
    private readonly IPowerShellCodeGeneratorNamespaceProvider _namespaceProvider;

    public PowerShellSchemaCodeGenerator(string outputPath, string namespaceName, IPowerShellCodeGeneratorNamespaceProvider namespaceProvider)
    {
        _outputPath = outputPath;
        _namespaceName = namespaceName;
        _namespaceProvider = namespaceProvider;
    }

    public void GenerateSdkCode(PowerShellSchemaDto powerShellSchema)
    {
        powerShellSchema.ThrowIfNull();

        foreach (ModelEnumTypeDescriptor enumTypeDescriptor in powerShellSchema.Enums)
        {
            EnumDeclarationSyntax generateEnumDeclaration = EnumCodeGenerator.GenerateEnumDeclaration(enumTypeDescriptor);
            string directoryPath = Path.Combine(_outputPath, "Enums");
            WriteToCsFile(directoryPath, enumTypeDescriptor.Name, generateEnumDeclaration, _namespaceProvider.GetForEnum(enumTypeDescriptor));
        }

        foreach (SimpleModelDescriptor simpleModelDescriptor in powerShellSchema.Models)
        {
            var simpleModelGenerator = new SimpleModelGenerator();
            ClassDeclarationSyntax classDeclarationSyntax = simpleModelGenerator.Generate(simpleModelDescriptor);
            string directoryPath = Path.Combine(_outputPath, "Models");
            WriteToCsFile(directoryPath, simpleModelDescriptor.Name, classDeclarationSyntax, _namespaceProvider.GetForModel(simpleModelDescriptor));
        }

        foreach (PowerShellCmdletDescriptor cmdletDescriptor in powerShellSchema.CmdletDescriptors)
        {
            var cmdletDescriptorGenerator = new CmdletDescriptorGenerator();
            ClassDeclarationSyntax cmdletClass = cmdletDescriptorGenerator.Generate(cmdletDescriptor);

            string directoryPath = _outputPath;
            foreach (string featureName in cmdletDescriptor.Scope)
                directoryPath = Path.Combine(directoryPath, featureName);

            WriteToCsFile(directoryPath, cmdletDescriptor.CmdletAttributeValues.GetClassName(), cmdletClass, _namespaceProvider.GetForCmdlet(cmdletDescriptor));
        }
    }

    private void WriteToCsFile(string directoryPath, string typeName, MemberDeclarationSyntax declarationSyntax, string[] usingList)
    {
        DirectoryExtensions.EnsureFileExists(directoryPath);
        string fullPath = Path.Combine(directoryPath, $"{typeName}.g.cs");
        string content = CodeGenerationNamespaceWrapper.Wrap(declarationSyntax, _namespaceName, usingList).NormalizeWhitespace().ToString();

        File.WriteAllText(fullPath, content);
    }
}