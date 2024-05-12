using Kysect.PowerShellRunner.Abstractions.Accessors.Results;
using Kysect.PowerShellRunner.Abstractions.Queries;

namespace Kysect.PowerShellRunner.Abstractions.Accessors;

public interface IPowerShellAccessor : IDisposable
{
    IPowerShellExecutionResult Execute(PowerShellQuery query);
}