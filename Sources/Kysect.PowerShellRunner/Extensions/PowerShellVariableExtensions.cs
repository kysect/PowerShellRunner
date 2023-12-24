using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.PowerShellRunner.Abstractions.Variables;

namespace Kysect.PowerShellRunner.Extensions;

public static class PowerShellVariableExtensions
{
    public static PowerShellVariable<TOut> UnsafeCast<TIn, TOut>(this PowerShellVariable<TIn> variable)
    {
        variable.ThrowIfNull();

        return new PowerShellVariable<TOut>(variable.Name);
    }
}