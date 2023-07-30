using Kysect.PowerShellRunner.Abstractions.Cmdlets;
using Kysect.PowerShellRunner.Abstractions.Variables;
using System.Linq;

namespace Kysect.PowerShellRunner.Core.QueryBuilding;

public class PowerShellParameterFactory
{
    public static PowerShellParameter<T> FromValue<T>(T value) where T : notnull
    {
        return new PowerShellParameter<T>(new PowerShellCmdletParameterValue(value));
    }

    public static PowerShellParameter<T> FromVariable<T>(IPowerShellReferenceable<T> variableWithIndex) where T : notnull
    {
        return new PowerShellParameter<T>(variableWithIndex.AsReference());
    }

    public static PowerShellParameter<T[]> FromVariableToArray<T>(IPowerShellReferenceable<T> variableWithIndex)
    {
        return new PowerShellParameter<T[]>(variableWithIndex.AsReference());
    }

    public static PowerShellParameter<T[]> FromVariableToArray<T>(IPowerShellReferenceable<T>[] variableWithIndex)
    {
        string reference = string.Join(",", variableWithIndex.Select(v => v.AsReference()));
        return new PowerShellParameter<T[]>(new PowerShellCmdletParameterReferenceValue(reference));
    }

    public static PowerShellParameter<T[]> FromValueToArray<T>(T value)
    {
        return new PowerShellParameter<T[]>(new PowerShellCmdletParameterValue(new[] { value }));
    }
}