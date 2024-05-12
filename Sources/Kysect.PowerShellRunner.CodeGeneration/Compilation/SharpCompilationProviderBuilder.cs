using Kysect.CommonLib.BaseTypes.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;

namespace Kysect.PowerShellRunner.CodeGeneration.Compilation;

public class SharpCompilationProviderBuilder
{
    private static readonly string StandardLibraryAssemblyPath = typeof(object).Assembly.Location;
    private static readonly string AssemblyPath = Path.GetDirectoryName(typeof(object).Assembly.Location);

    private CSharpCompilation _compilation;

    public SharpCompilationProviderBuilder(CSharpCompilation compilation)
    {
        _compilation = compilation;
    }

    public static SharpCompilationProviderBuilder CreateForStandard(string name)
    {
        return new SharpCompilationProviderBuilder(CSharpCompilation.Create(name))
            .AddReferences(StandardLibraryAssemblyPath)
            .AddReferences(Path.Combine(AssemblyPath, "System.dll"))
            .AddReferences(Path.Combine(AssemblyPath, "System.Collections.dll"))
            .AddReferences(Path.Combine(AssemblyPath, "System.Linq.dll"))
            .AddReferences(Path.Combine(AssemblyPath, "System.Linq.Queryable.dll"))
            .AddReferences(Path.Combine(AssemblyPath, "System.Linq.Expressions.dll"))
            .AddReferences(Path.Combine(AssemblyPath, "System.Runtime.dll"));
    }

    public SharpCompilationProviderBuilder AddReferences(Assembly assembly)
    {
        assembly.ThrowIfNull();

        return AddReferences(assembly.Location);
    }

    public SharpCompilationProviderBuilder AddReferences(string assemblyFilePath)
    {
        assemblyFilePath.ThrowIfNull();

        _compilation = _compilation.AddReferences(MetadataReference.CreateFromFile(assemblyFilePath));
        return this;
    }

    public CSharpCompilation Build()
    {
        return _compilation;
    }
}