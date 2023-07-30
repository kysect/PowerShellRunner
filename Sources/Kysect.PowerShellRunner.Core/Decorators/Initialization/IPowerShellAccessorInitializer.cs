using Kysect.PowerShellRunner.Abstractions.Accessors;

namespace Kysect.PowerShellRunner.Core.Decorators.Initialization;

public interface IPowerShellAccessorInitializer
{
    public void Initialize(IPowerShellAccessor powerShellAccessor);
}