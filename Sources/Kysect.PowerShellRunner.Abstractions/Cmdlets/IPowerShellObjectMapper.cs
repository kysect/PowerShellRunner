using Kysect.PowerShellRunner.Abstractions.Objects;

namespace Kysect.PowerShellRunner.Abstractions.Cmdlets;

public interface IPowerShellObjectMapper
{
    T Map<T>(IPowerShellObject powerShellObject) where T : notnull;
}