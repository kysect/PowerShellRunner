using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.PowerShellRunner.Abstractions.Variables;
using System.Text;

namespace Kysect.PowerShellRunner.Abstractions.Queries;

public record struct PowerShellQuery(PowerShellVariable? ResultVariable, string Query, bool ContainsSensitiveInfo, string? RedirectionPath)
{
    public PowerShellQuery(string query) : this(null, query, false, null)
    {
    }

    public PowerShellQuery WithSwitch(string argumentKey)
    {
        return this with { Query = $"{Query} -{argumentKey}" };
    }

    public PowerShellQuery With<T>(string argumentKey, IPowerShellReferenceable<T> variable)
    {
        variable.ThrowIfNull();

        return this with { Query = $"{Query} -{argumentKey} {variable.AsReference()}" };
    }

    public PowerShellQuery WithNoFormat(string argumentKey, string argumentValue)
    {
        return this with { Query = $"{Query} -{argumentKey} {argumentValue}" };
    }

    public PowerShellQuery With(string argumentKey, string argumentValue)
    {
        return this with { Query = $"{Query} -{argumentKey} \"{argumentValue}\"" };
    }

    public PowerShellQuery With(string argumentKey, int argumentValue)
    {
        return this with { Query = $"{Query} -{argumentKey} {argumentValue}" };
    }

    public PowerShellQuery WithPipeline()
    {
        return this with { Query = $"{Query} |" };
    }

    public PowerShellQuery WherePropertyLike(string propertyName, string argumentValue)
    {
        return this with { Query = $"{Query} Where {propertyName} -clike \"{argumentValue}*\"" };
    }

    public PowerShellQuery WherePropertyEqual(string propertyName, string argumentValue)
    {
        return this with { Query = $"{Query} Where {propertyName} -eq \"{argumentValue}\"" };
    }

    public PowerShellQuery ContainsSensitiveDate()
    {
        return this with { ContainsSensitiveInfo = true };
    }

    public PowerShellQuery WithRedirection(string path)
    {
        return this with { RedirectionPath = path };
    }

    public readonly string Format()
    {
        var sb = new StringBuilder();

        if (ResultVariable is not null)
            sb.Append($"{ResultVariable.AsReference()} = ");

        sb.Append(Query);
        if (!string.IsNullOrEmpty(RedirectionPath))
            sb.Append($" *> \"{RedirectionPath}\"");

        return sb.ToString();
    }
}