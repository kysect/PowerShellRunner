using Kysect.PowerShellRunner.Abstractions.Cmdlets;

namespace Kysect.PowerShellRunner.Abstractions.Variables;

public interface IPowerShellReferenceable<out T>
{
    PowerShellCmdletParameterReferenceValue AsReference();
}