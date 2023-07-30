using Kysect.PowerShellRunner.Abstractions.Cmdlets;

namespace Kysect.PowerShellRunner.Core.CustomCmdlets;

public class GetLocationCmdletWrapperResult
{
    public string Path { get; }

    public GetLocationCmdletWrapperResult(string path)
    {
        Path = path;
    }
}

public class GetLocationCmdletWrapper : IPowerShellCmdlet<GetLocationCmdletWrapperResult>
{
    public string CmdletName => "Get-Location";
}