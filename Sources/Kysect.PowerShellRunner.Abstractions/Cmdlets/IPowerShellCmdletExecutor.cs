using Kysect.PowerShellRunner.Abstractions.Objects;
using Kysect.PowerShellRunner.Abstractions.Variables;

namespace Kysect.PowerShellRunner.Abstractions.Cmdlets;

public interface IPowerShellCmdletExecutor
{
    IReadOnlyCollection<IPowerShellObject> Execute(IPowerShellCmdlet cmdlet);
    PowerShellVariable<IPowerShellObject> InitializeVariable(string variableName, IPowerShellCmdlet cmdlet);

    IReadOnlyCollection<T> Execute<T>(IPowerShellCmdlet<T> cmdlet) where T : notnull;
    PowerShellVariable<T> InitializeVariable<T>(string variableName, IPowerShellCmdlet<T> cmdlet) where T : notnull;
}