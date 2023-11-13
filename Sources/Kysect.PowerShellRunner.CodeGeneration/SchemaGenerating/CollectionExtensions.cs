using System.Collections.Generic;
using System.Linq;

namespace Kysect.PowerShellRunner.CodeGeneration.SchemaGenerating;

public static class CollectionExtensions
{
    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> values)
    {
        return values
            .Where(v => v is not null)
            .Select(v => v!);
    }
}