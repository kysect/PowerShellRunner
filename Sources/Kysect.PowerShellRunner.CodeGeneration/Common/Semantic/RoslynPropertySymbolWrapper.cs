using Kysect.CommonLib.BaseTypes.Extensions;
using Microsoft.CodeAnalysis;

namespace Kysect.PowerShellRunner.CodeGeneration.Common.Semantic;

public class RoslynPropertySymbolWrapper
{
    public RoslynTypeSymbolWrapper Type { get; }

    public string Name { get; }
    // TODO: validate
    public string TypeName => Type.GetNameWithContainingParent();

    public RoslynPropertySymbolWrapper(IPropertySymbol roslynSymbol)
    {
        roslynSymbol.ThrowIfNull(nameof(roslynSymbol));

        Name = roslynSymbol.Name;
        Type = new RoslynTypeSymbolWrapper(roslynSymbol.Type);
    }
}