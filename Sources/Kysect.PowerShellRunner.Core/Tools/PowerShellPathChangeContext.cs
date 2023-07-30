using Kysect.PowerShellRunner.Abstractions.Accessors;
using Kysect.PowerShellRunner.Core.CustomCmdlets;
using Kysect.PowerShellRunner.Core.Executions;
using Kysect.PowerShellRunner.Core.Extensions;
using System;
using System.IO;
using System.Linq;

namespace Kysect.PowerShellRunner.Core.Tools;

public class PowerShellPathChangeContext : IDisposable
{
    private readonly IPowerShellAccessor _powerShellAccessor;
    private readonly string _previousPath;

    public static PowerShellPathChangeContext TemporaryChangeCurrentDirectory(IPowerShellAccessor accessor, string newPath)
    {
        if (!Path.IsPathRooted(newPath))
            throw new ArgumentException("Path should be rooted. Actual: " + newPath);

        GetLocationCmdletWrapperResult getLocationCmdletWrapperResult = accessor.SelectCmdlet(new GetLocationCmdletWrapper()).Execute().Single();

        accessor.ExecuteRaw(WellKnownQueryProvider.Instance.ChangeDirectory(newPath));
        return new PowerShellPathChangeContext(accessor, getLocationCmdletWrapperResult.Path);
    }

    public PowerShellPathChangeContext(IPowerShellAccessor powerShellAccessor, string previousPath)
    {
        _powerShellAccessor = powerShellAccessor;
        _previousPath = previousPath;
    }

    public void Dispose()
    {
        _powerShellAccessor.ExecuteRaw(WellKnownQueryProvider.Instance.ChangeDirectory(_previousPath));
    }
}