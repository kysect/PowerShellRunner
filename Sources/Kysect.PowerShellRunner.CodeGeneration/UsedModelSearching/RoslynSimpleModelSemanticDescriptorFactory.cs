using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.PowerShellRunner.CodeGeneration.Common;
using Kysect.PowerShellRunner.CodeGeneration.Common.Semantic;
using Microsoft.CodeAnalysis;
using System.Linq;

namespace Kysect.PowerShellRunner.CodeGeneration.UsedModelSearching;

public class RoslynSimpleModelSemanticDescriptorFactory
{
    private readonly IRoslynSimpleModelBaseTypeFilter _baseTypeFilter;
    private readonly IRoslynSimpleModelPropertyFilter _propertyFilter;

    public RoslynSimpleModelSemanticDescriptorFactory(IRoslynSimpleModelBaseTypeFilter baseTypeFilter, IRoslynSimpleModelPropertyFilter propertyFilter)
    {
        _baseTypeFilter = baseTypeFilter;
        _propertyFilter = propertyFilter;
    }

    public RoslynSimpleModelSemanticDescriptor Create(ITypeSymbol roslynSymbol)
    {
        roslynSymbol.ThrowIfNull();

        string name = roslynSymbol.GetNameWithContainingParent();
        RoslynTypeSymbolWrapper? baseType = GetBaseType(roslynSymbol, _baseTypeFilter);
        var propertySymbols = roslynSymbol
            .GetMembers()
            .OfType<IPropertySymbol>()
            .Where(IsPropertyAcceptable)
            .ToList();

        var publicProperties = propertySymbols
            .Where(p => p.DeclaredAccessibility == Accessibility.Public)
            .Select(p => new RoslynPropertySymbolWrapper(p))
            .ToList();

        var nonPublicProperties = propertySymbols
            .Where(p => p.DeclaredAccessibility != Accessibility.Public)
            .Select(p => new RoslynPropertySymbolWrapper(p))
            .ToList();

        return new RoslynSimpleModelSemanticDescriptor(name, baseType, publicProperties, nonPublicProperties);
    }

    private static RoslynTypeSymbolWrapper? GetBaseType(ITypeSymbol roslynSymbol, IRoslynSimpleModelBaseTypeFilter filter)
    {
        if (roslynSymbol.BaseType is null)
            return null;

        bool shouldSpecifyBaseType =
            !roslynSymbol.BaseType.IsObjectType()
            && !roslynSymbol.BaseType.IsEnumType()
            && !roslynSymbol.BaseType.IsValueObjectType();

        if (!shouldSpecifyBaseType)
            return null;

        if (!filter.Acceptable(roslynSymbol.BaseType))
            return null;

        return new RoslynTypeSymbolWrapper(roslynSymbol.BaseType);
    }

    private bool IsPropertyAcceptable(IPropertySymbol propertySymbol)
    {
        if (propertySymbol.IsIndexer)
            return false;

        if (propertySymbol.IsStatic)
            return false;

        return _propertyFilter.Acceptable(propertySymbol);
    }
}