using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.PowerShellRunner.Abstractions.Objects;
using System.Management.Automation;

namespace Kysect.PowerShellRunner.FrameworkImplementation;

public class PowerShellObjectMember : IPowerShellObjectMember
{
    public string Name { get; }
    public object Value { get; }

    public PowerShellObjectMember(PSMemberInfo memberInfo)
    {
        memberInfo.ThrowIfNull();

        Name = memberInfo.Name;
        Value = memberInfo.Value;
    }
}