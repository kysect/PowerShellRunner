using Kysect.CommonLib.BaseTypes.Extensions;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Kysect.PowerShellRunner.CodeGeneration.Common;

public static class RoslynTypeSymbolExtensions
{
    public static bool IsUndefinedType(this ISymbol s)
    {
        s.ThrowIfNull();

        return s.ToString() == "?";
    }

    public static bool TryGetArraySymbol(this ISymbol s, [NotNullWhen(returnValue: true)] out ITypeSymbol? elementType)
    {
        if (s is IArrayTypeSymbol arrayTypeSymbol)
        {
            elementType = arrayTypeSymbol.ElementType;
            return true;
        }

        elementType = null;
        return false;
    }

    public static bool TryGetGenericType(this ISymbol s, out IReadOnlyCollection<ITypeSymbol> genericTypeDefinition)
    {
        if (s is INamedTypeSymbol namedTypeSymbol)
        {
            if (namedTypeSymbol.TypeArguments.Length > 0)
            {
                genericTypeDefinition = namedTypeSymbol.TypeArguments;
                return true;
            }
        }

        genericTypeDefinition = Array.Empty<ITypeSymbol>();
        return false;
    }

    public static bool TryGetNullable(this ISymbol s, [NotNullWhen(returnValue: true)] out ITypeSymbol? innerType)
    {
        s.ThrowIfNull();

        if (s.Name == "Nullable")
        {
            if (s is INamedTypeSymbol namedTypeSymbol)
            {
                innerType = namedTypeSymbol.TypeArguments.Single();
                return true;
            }
        }

        innerType = null;
        return false;
    }

    public static string GetNameWithContainingParent(this ISymbol s)
    {
        if (s.TryGetArraySymbol(out ITypeSymbol? elementType))
            return $"{GetNameWithContainingParent(elementType)}[]";

        if (s.TryGetNullable(out ITypeSymbol? innerType))
            return $"{innerType.GetNameWithContainingParent()}?";

        if (s.ContainingType is null)
            return s.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);

        return $"{GetNameWithContainingParent(s.ContainingType)}{s.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}";
    }

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

    public static bool IsObjectType(this ITypeSymbol typeSymbol)
    {
        typeSymbol.ThrowIfNull();

        return typeSymbol.Name.ToLower() == "object";
    }

    public static bool IsValueObjectType(this ITypeSymbol typeSymbol)
    {
        typeSymbol.ThrowIfNull();

        return typeSymbol.Name.ToLower() == "ValueType".ToLower();
    }

    public static bool IsEnumType(this ITypeSymbol typeSymbol)
    {
        typeSymbol.ThrowIfNull();

        return typeSymbol?.Name.ToLower() == "enum";
    }
}