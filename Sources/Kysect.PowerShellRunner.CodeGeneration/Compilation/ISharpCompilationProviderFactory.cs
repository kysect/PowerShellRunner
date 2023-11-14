using Microsoft.CodeAnalysis.CSharp;

namespace Kysect.PowerShellRunner.CodeGeneration.Compilation;

public interface ISharpCompilationProviderFactory
{
    CSharpCompilation Build();
}