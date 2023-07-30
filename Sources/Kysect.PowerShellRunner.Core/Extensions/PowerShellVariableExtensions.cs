using Kysect.PowerShellRunner.Abstractions.Variables;

namespace Kysect.PowerShellRunner.Core.Extensions;

public static class PowerShellVariableExtensions
{
    public static PowerShellVariable<TOut> UnsafeCast<TIn, TOut>(this PowerShellVariable<TIn> variable)
    {
        return new PowerShellVariable<TOut>(variable.Name);
    }
}