using Kysect.PowerShellRunner.Abstractions.Variables;

namespace Kysect.PowerShellRunner.Abstractions.Parameters;

public class PowerShellCmdletParameterFactory
{
    public static PowerShellCmdletParameter<T> FromValue<T>(T value) where T : notnull
    {
        return new PowerShellCmdletParameter<T>(new PowerShellCmdletParameterValue(value));
    }

    public static PowerShellCmdletParameter<T> FromVariable<T>(IPowerShellReferenceable<T> reference) where T : notnull
    {
        return new PowerShellCmdletParameter<T>(reference.AsReference());
    }

    public static PowerShellCmdletParameter<T[]> FromVariableToArray<T>(IPowerShellReferenceable<T> reference)
    {
        return new PowerShellCmdletParameter<T[]>(reference.AsReference());
    }

    public static PowerShellCmdletParameter<T[]> FromVariableToArray<T>(IPowerShellReferenceable<T>[] reference)
    {
        return new PowerShellCmdletParameter<T[]>(PowerShellReferenceCollection.Create(reference));
    }

    public static PowerShellCmdletParameter<T[]> FromValueToArray<T>(T value)
    {
        return new PowerShellCmdletParameter<T[]>(new PowerShellCmdletParameterValue(new[] { value }));
    }
}