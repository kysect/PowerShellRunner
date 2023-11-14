using Microsoft.CodeAnalysis.CSharp;
using System.Management.Automation;

namespace Kysect.PowerShellRunner.CodeGeneration.Compilation;

public class DefaultSharpCompilationProviderFactory : ISharpCompilationProviderFactory
{
    public CSharpCompilation Build()
    {
        return SharpCompilationProviderBuilder
            .CreateForStandard("Analysis")
            .AddReferences(typeof(SwitchParameter).Assembly)
            .Build();
    }
}