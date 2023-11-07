using Kysect.CommonLib.BaseTypes.Extensions;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;

namespace Kysect.PowerShellRunner.CodeGeneration.Compilation;

public class SolutionCompilationContext
{
    public CSharpCompilation Compilation { get; }
    public IReadOnlyCollection<SolutionCompilationContextItem> Items { get; }
    public SolutionCompilationTypeInheritancesSearcher TypeInheritancesSearcher { get; }

    public SolutionCompilationContext(
        CSharpCompilation compilation,
        IReadOnlyCollection<SolutionCompilationContextItem> items,
        SolutionCompilationTypeInheritancesSearcher typeInheritancesSearcher)
    {
        Compilation = compilation.ThrowIfNull();
        Items = items.ThrowIfNull();
        TypeInheritancesSearcher = typeInheritancesSearcher.ThrowIfNull();
    }
}