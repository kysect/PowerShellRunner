namespace Kysect.PowerShellRunner.Abstractions.Variables;

public interface IPowerShellReferenceable<out T>
{
    PowerShellReference AsReference();
}