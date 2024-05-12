using Kysect.PowerShellRunner.Abstractions.Accessors;
using Kysect.PowerShellRunner.CustomCmdlets;
using Kysect.PowerShellRunner.Executions;
using Kysect.PowerShellRunner.Extensions;

namespace Kysect.PowerShellRunner.Tools;

public class PowerShellPathChangeContext : IDisposable
{
    private readonly IPowerShellAccessor _powerShellAccessor;
    private readonly string _previousPath;
    private bool _disposedValue;

    public static PowerShellPathChangeContext TemporaryChangeCurrentDirectory(IPowerShellAccessor accessor, string newPath)
    {
        if (!Path.IsPathRooted(newPath))
            throw new ArgumentException("Path should be rooted. Actual: " + newPath);

        GetLocationCmdletWrapperResult getLocationCmdletWrapperResult = accessor.SelectCmdlet(new GetLocationCmdlet()).Execute().Single();

        accessor.ExecuteRaw(WellKnownQueryProvider.Instance.ChangeDirectory(newPath));
        return new PowerShellPathChangeContext(accessor, getLocationCmdletWrapperResult.Path);
    }

    public PowerShellPathChangeContext(IPowerShellAccessor powerShellAccessor, string previousPath)
    {
        _powerShellAccessor = powerShellAccessor;
        _previousPath = previousPath;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposedValue)
            return;

        if (disposing)
        {
            _powerShellAccessor.ExecuteRaw(WellKnownQueryProvider.Instance.ChangeDirectory(_previousPath));
        }

        _disposedValue = true;
    }

    public void Dispose()
    {
        Dispose(disposing: true);
    }
}