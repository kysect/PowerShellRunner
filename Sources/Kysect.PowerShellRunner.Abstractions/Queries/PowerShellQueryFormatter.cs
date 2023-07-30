using System.Text;

namespace Kysect.PowerShellRunner.Abstractions.Queries;

public class PowerShellQueryFormatter
{
    public string Format(PowerShellQuery query)
    {
        var sb = new StringBuilder();

        if (query.ResultVariable is not null)
            sb.Append($"{query.ResultVariable.AsReference()} = ");

        sb.Append(query.Query);
        if (!string.IsNullOrEmpty(query.RedirectionPath))
            sb.Append($" *> \"{query.RedirectionPath}\"");

        return sb.ToString();
    }
}