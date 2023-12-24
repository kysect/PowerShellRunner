using Kysect.PowerShellRunner.Abstractions.Accessors;

namespace Kysect.PowerShellRunner.Core.Accessors.Initialization;

public interface IPowerShellAccessorInitializer
{
    public void Initialize(IPowerShellAccessor powerShellAccessor);
}