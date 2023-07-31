using Kysect.PowerShellRunner.Abstractions.Variables;

namespace Kysect.PowerShellRunner.Abstractions.Parameters;

public class PowerShellCmdletParameterFactory
{
    public static PowerShellCmdletParameter<T> FromValue<T>(T value) where T : notnull
    {
        return new PowerShellCmdletParameter<T>(new PowerShellCmdletParameterValue(value));
    }

    public static PowerShellCmdletParameter<T> FromVariable<T>(IPowerShellReferenceable<T> variableWithIndex) where T : notnull
    {
        return new PowerShellCmdletParameter<T>(variableWithIndex.AsReference());
    }

    public static PowerShellCmdletParameter<T[]> FromVariableToArray<T>(IPowerShellReferenceable<T> variableWithIndex)
    {
        return new PowerShellCmdletParameter<T[]>(variableWithIndex.AsReference());
    }

    public static PowerShellCmdletParameter<T[]> FromVariableToArray<T>(IPowerShellReferenceable<T>[] references)
    {
        return new PowerShellCmdletParameter<T[]>(PowerShellReferenceCollection.Create(references));
    }

    public static PowerShellCmdletParameter<T[]> FromValueToArray<T>(T value)
    {
        return new PowerShellCmdletParameter<T[]>(new PowerShellCmdletParameterValue(new[] { value }));
    }
}