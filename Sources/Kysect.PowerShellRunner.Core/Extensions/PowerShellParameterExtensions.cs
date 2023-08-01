using Kysect.CommonLib.Reflection;
using Kysect.PowerShellRunner.Abstractions.Cmdlets;
using Kysect.PowerShellRunner.Abstractions.Parameters;
using Kysect.PowerShellRunner.Abstractions.Variables;
using System;
using System.Linq.Expressions;
using System.Management.Automation;

namespace Kysect.PowerShellRunner.Core.Extensions;

public static class PowerShellParameterExtensions
{
    public static TCmdlet Set<TCmdlet, TParameter>(
        this TCmdlet cmdlet,
        Expression<Func<TCmdlet, IPowerShellCmdletParameter<TParameter[]>>> selector,
        TParameter value) where TCmdlet : IPowerShellCmdlet
    {
        PowerShellCmdletParameter<TParameter[]> cmdletParameter = PowerShellCmdletParameterFactory.FromValueToArray(value);
        ExpressionBasedInstanceModifier.Instance.ModifyProperty(cmdlet, selector, cmdletParameter);
        return cmdlet;
    }

    public static TCmdlet Set<TCmdlet, TParameter>(
        this TCmdlet cmdlet,
        Expression<Func<TCmdlet, IPowerShellCmdletParameter<TParameter>>> selector,
        TParameter value)
        where TCmdlet : IPowerShellCmdlet
        where TParameter : notnull
    {
        PowerShellCmdletParameter<TParameter> cmdletParameter = PowerShellCmdletParameterFactory.FromValue(value);
        ExpressionBasedInstanceModifier.Instance.ModifyProperty(cmdlet, selector, cmdletParameter);
        return cmdlet;
    }

    public static TCmdlet Set<TCmdlet>(
        this TCmdlet cmdlet,
        Expression<Func<TCmdlet, IPowerShellCmdletParameter<SwitchParameter>>> selector,
        bool value) where TCmdlet : IPowerShellCmdlet
    {
        if (!value)
            return cmdlet;

        PowerShellCmdletParameter<SwitchParameter> cmdletParameter = PowerShellCmdletParameterFactory.FromValue(SwitchParameter.Present);
        ExpressionBasedInstanceModifier.Instance.ModifyProperty(cmdlet, selector, cmdletParameter);
        return cmdlet;
    }

    public static TCmdlet Set<TCmdlet, TParameter>(
        this TCmdlet cmdlet,
        Expression<Func<TCmdlet, IPowerShellCmdletParameter<TParameter>>> selector,
        IPowerShellReferenceable<TParameter> reference)
        where TCmdlet : IPowerShellCmdlet
        where TParameter : notnull
    {
        PowerShellCmdletParameter<TParameter> cmdletParameter = PowerShellCmdletParameterFactory.FromVariable(reference);
        ExpressionBasedInstanceModifier.Instance.ModifyProperty(cmdlet, selector, cmdletParameter);
        return cmdlet;
    }

    public static TCmdlet Set<TCmdlet, TParameter>(
        this TCmdlet cmdlet,
        Expression<Func<TCmdlet, IPowerShellCmdletParameter<TParameter[]>>> selector,
        IPowerShellReferenceable<TParameter> reference) where TCmdlet : IPowerShellCmdlet
    {
        PowerShellCmdletParameter<TParameter[]> cmdletParameter = PowerShellCmdletParameterFactory.FromVariableToArray(reference);
        ExpressionBasedInstanceModifier.Instance.ModifyProperty(cmdlet, selector, cmdletParameter);
        return cmdlet;
    }

    public static TCmdlet Set<TCmdlet, TParameter>(
        this TCmdlet cmdlet,
        Expression<Func<TCmdlet, IPowerShellCmdletParameter<TParameter[]>>> selector,
        params IPowerShellReferenceable<TParameter>[] reference) where TCmdlet : IPowerShellCmdlet
    {
        PowerShellCmdletParameter<TParameter[]> cmdletParameter = PowerShellCmdletParameterFactory.FromVariableToArray(reference);
        ExpressionBasedInstanceModifier.Instance.ModifyProperty(cmdlet, selector, cmdletParameter);
        return cmdlet;
    }
}