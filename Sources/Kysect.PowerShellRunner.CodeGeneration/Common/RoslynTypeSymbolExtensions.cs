using Kysect.CommonLib.BaseTypes.Extensions;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace Kysect.PowerShellRunner.CodeGeneration.Common;

public static class RoslynTypeSymbolExtensions
{
    public static bool IsInheritedOf(this ITypeSymbol type, string baseTypeName)
    {
        IReadOnlyCollection<ITypeSymbol> baseTypes = GetBaseTypes(type);
        return baseTypes.Any(b => b.Name == baseTypeName);
    }

    public static IReadOnlyCollection<ITypeSymbol> GetBaseTypes(ITypeSymbol type)
    {
        type.ThrowIfNull();

        var baseTypes = new List<ITypeSymbol>();

        INamedTypeSymbol? current = type.BaseType;
        while (current != null)
        {
            baseTypes.Add(current);
            current = current.BaseType;
        }

        return baseTypes;
    }
}