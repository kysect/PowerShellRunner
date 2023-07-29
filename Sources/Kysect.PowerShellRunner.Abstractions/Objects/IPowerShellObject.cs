using System.Collections.Generic;

namespace Kysect.PowerShellRunner.Abstractions.Objects;

public interface IPowerShellObject
{
    object this[string memberName] { get; }

    IReadOnlyCollection<IPowerShellObjectMember> GetMembers();
    IReadOnlyCollection<IPowerShellObjectMember> GetProperties();

    string AsString();
}