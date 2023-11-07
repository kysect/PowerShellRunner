using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.PowerShellRunner.CodeGeneration.Common;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace Kysect.PowerShellRunner.CodeGeneration.Compilation;

public class SolutionCompilationTypeInheritancesSearcher
{
    private readonly ILookup<ITypeSymbol, INamedTypeSymbol> _mapBaseTypeToAllImplementation;

    private SolutionCompilationTypeInheritancesSearcher(IReadOnlyCollection<SolutionCompilationContextItem> contextItems)
    {
        contextItems.ThrowIfNull();

        _mapBaseTypeToAllImplementation = contextItems
            .SelectMany(i => RoslynTypeSymbolExtensions.GetBaseTypes(i.Symbol).Select(t => (Derived: i.Symbol, Base: t)))
            .ToLookup(p => p.Base, p => p.Derived, TypeSymbolEqualityComparer.Default);
    }

    public static SolutionCompilationTypeInheritancesSearcher CreateInstance(IReadOnlyCollection<SolutionCompilationContextItem> contextItems)
    {
        return new SolutionCompilationTypeInheritancesSearcher(contextItems);
    }

    public IReadOnlyCollection<INamedTypeSymbol> GetAllInheritances(ITypeSymbol type)
    {
        return _mapBaseTypeToAllImplementation[type].ToList();
    }
}