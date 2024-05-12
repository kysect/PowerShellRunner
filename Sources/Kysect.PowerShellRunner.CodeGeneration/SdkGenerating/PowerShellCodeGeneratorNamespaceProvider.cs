using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.PowerShellRunner.Abstractions.Parameters;
using Kysect.PowerShellRunner.CodeGeneration.SchemaGenerating;
using Kysect.PowerShellRunner.CodeGeneration.UsedModelSearching;

namespace Kysect.PowerShellRunner.CodeGeneration.SdkGenerating;

public class PowerShellCodeGeneratorNamespaceProvider : IPowerShellCodeGeneratorNamespaceProvider
{
    public string[] GetForEnum(ModelEnumTypeDescriptor enumTypeDescriptor)
    {
        return Array.Empty<string>();
    }

    public string[] GetForModel(SimpleModelDescriptor simpleModelDescriptor)
    {
        return new[]
        {
            "System",
            "System.Security",
            "System.Net",
            "System.Net.Sockets",
            "System.Net.NetworkInformation",
            "System.Security.Cryptography.X509Certificates",
            "System.Xml",
            "System.Collections.Generic",
            "System.Collections.Concurrent",
        };
    }

    public string[] GetForCmdlet(PowerShellCmdletDescriptor cmdletDescriptor)
    {
        return new[]
        {
            typeof(IPowerShellCmdletParameter<object>).Namespace.ThrowIfNull(),
            "System",
            "System.Security",
            "System.Net",
            "System.Net.Sockets",
            "System.Net.NetworkInformation",
            "System.Security.Cryptography.X509Certificates",
            "System.Xml",
            "System.Collections.Generic",
            "System.Collections.Concurrent",
            "System.Management.Automation",
        };
    }
}