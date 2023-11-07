using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace Kysect.PowerShellRunner.CodeGeneration.Common;

public sealed class TypeSymbolEqualityComparer : IEqualityComparer<ITypeSymbol>
{
    public static TypeSymbolEqualityComparer Default { get; } = new TypeSymbolEqualityComparer();

    public bool Equals(ITypeSymbol x, ITypeSymbol y)
    {
        return SymbolEqualityComparer.Default.Equals(x, y);
    }

    public int GetHashCode(ITypeSymbol obj)
    {
        return SymbolEqualityComparer.Default.GetHashCode(obj);
    }
}