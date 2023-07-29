using System.Diagnostics.Contracts;
using Kysect.PowerShellRunner.Abstractions.Variables;

namespace Kysect.PowerShellRunner.Abstractions.Queries;

public class PowerShellQueryArguments
{
    public PowerShellVariable? ResultVariable { get; }
    public string Query { get; }
    public bool ContainsSensitiveInfo { get; }
    public string? RedirectionPath { get; }

    public PowerShellQueryArguments(string query) : this(query, false, null)
    {
    }

    public PowerShellQueryArguments(string query, string redirectionPath) : this(query, false, redirectionPath)
    {
    }

    public PowerShellQueryArguments(string query, bool containsSensitiveInfo, string? redirectionPath) : this(null, query, containsSensitiveInfo, redirectionPath)
    {
    }

    public PowerShellQueryArguments(PowerShellVariable? resultVariable, string query, bool containsSensitiveInfo, string? redirectionPath)
    {
        ResultVariable = resultVariable;
        Query = query;
        ContainsSensitiveInfo = containsSensitiveInfo;
        RedirectionPath = redirectionPath;
    }

    [Pure]
    public PowerShellQueryArguments WithVariable(PowerShellVariable resultVariable)
    {
        return new PowerShellQueryArguments(
            resultVariable,
            Query,
            ContainsSensitiveInfo,
            RedirectionPath);
    }
}