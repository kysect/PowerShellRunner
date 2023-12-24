using Kysect.PowerShellRunner.Abstractions.Cmdlets;

namespace Kysect.PowerShellRunner.CustomCmdlets;

public class GetLocationCmdletWrapperResult
{
    public string Path { get; }

    public GetLocationCmdletWrapperResult(string path)
    {
        Path = path;
    }
}

public class GetLocationCmdlet : IPowerShellCmdlet<GetLocationCmdletWrapperResult>
{
    public string CmdletName => "Get-Location";
}