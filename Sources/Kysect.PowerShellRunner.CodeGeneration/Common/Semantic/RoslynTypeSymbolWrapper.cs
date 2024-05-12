using Kysect.CommonLib.BaseTypes.Extensions;
using Microsoft.CodeAnalysis;
using System.Diagnostics.CodeAnalysis;

namespace Kysect.PowerShellRunner.CodeGeneration.Common.Semantic;

public class RoslynTypeSymbolWrapper
{
    public ITypeSymbol RoslynSymbol { get; }

    public string Name => GetNameWithContainingParent();

    public RoslynTypeSymbolWrapper(ITypeSymbol roslynSymbol)
    {
        if (roslynSymbol == null)
            throw new ArgumentException("Type symbol cannot be null");

        if (roslynSymbol.IsUndefinedType())
            throw new ArgumentException("Type symbol cannot be undefined");

        RoslynSymbol = roslynSymbol.ThrowIfNull(nameof(roslynSymbol));
    }

    public bool TryUnwrapInnerType([NotNullWhen(true)] out ITypeSymbol? unwrapped)
    {
        unwrapped = null;

        if (RoslynSymbol.TryGetNullable(out ITypeSymbol? innerType))
            unwrapped = innerType;
        if (RoslynSymbol.TryGetArraySymbol(out ITypeSymbol? elementType))
            unwrapped = elementType;

        return unwrapped is not null;
    }

    public bool TryUnwrapGenerics(out IReadOnlyCollection<ITypeSymbol> unwrapped)
    {
        unwrapped = Array.Empty<ITypeSymbol>();

        if (RoslynSymbol is INamedTypeSymbol namedTypeSymbol && namedTypeSymbol.IsGenericType)
        {
            unwrapped = namedTypeSymbol.TypeArguments;
            return true;
        }

        return false;
    }

    public string GetNameWithContainingParent()
    {
        return RoslynSymbol.GetNameWithContainingParent();
    }
}