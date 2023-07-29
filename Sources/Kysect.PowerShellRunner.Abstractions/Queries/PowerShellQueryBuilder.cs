using System.Text;

namespace Kysect.PowerShellRunner.Abstractions.Queries;

public class PowerShellQueryBuilder
{
    public static PowerShellQueryBuilder Instance { get; } = new PowerShellQueryBuilder();

    public string Build(PowerShellQueryArguments queryArguments)
    {
        var sb = new StringBuilder();

        if (queryArguments.ResultVariable is not null)
            sb.Append($"{queryArguments.ResultVariable.AsReference()} = ");

        sb.Append(queryArguments.Query);
        if (!string.IsNullOrEmpty(queryArguments.RedirectionPath))
            sb.Append($" *> \"{queryArguments.RedirectionPath}\"");

        return sb.ToString();
    }
}