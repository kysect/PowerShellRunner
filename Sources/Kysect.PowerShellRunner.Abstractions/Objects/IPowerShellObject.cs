namespace Kysect.PowerShellRunner.Abstractions.Objects;

public interface IPowerShellObject
{
    IReadOnlyCollection<IPowerShellObjectMember> GetProperties();

    string AsString();
}