using Kysect.CommonLib.Reflection;
using Kysect.PowerShellRunner.Abstractions.Cmdlets;
using Kysect.PowerShellRunner.Abstractions.Variables;
using Kysect.PowerShellRunner.Core.QueryBuilding;
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
        PowerShellParameter<TParameter[]> parameter = PowerShellParameterFactory.FromValueToArray(value);
        ExpressionBasedInstanceModifier.Instance.ModifyProperty(cmdlet, selector, parameter);
        return cmdlet;
    }

    public static TCmdlet Set<TCmdlet, TParameter>(
        this TCmdlet cmdlet,
        Expression<Func<TCmdlet, IPowerShellCmdletParameter<TParameter>>> selector,
        TParameter value)
        where TCmdlet : IPowerShellCmdlet
        where TParameter : notnull
    {
        PowerShellParameter<TParameter> parameter = PowerShellParameterFactory.FromValue(value);
        ExpressionBasedInstanceModifier.Instance.ModifyProperty(cmdlet, selector, parameter);
        return cmdlet;
    }

    public static TCmdlet Set<TCmdlet>(
        this TCmdlet cmdlet,
        Expression<Func<TCmdlet, IPowerShellCmdletParameter<SwitchParameter>>> selector,
        bool value) where TCmdlet : IPowerShellCmdlet
    {
        if (!value)
            return cmdlet;

        PowerShellParameter<SwitchParameter> parameter = PowerShellParameterFactory.FromValue(SwitchParameter.Present);
        ExpressionBasedInstanceModifier.Instance.ModifyProperty(cmdlet, selector, parameter);
        return cmdlet;
    }

    public static TCmdlet Set<TCmdlet, TParameter>(
        this TCmdlet cmdlet,
        Expression<Func<TCmdlet, IPowerShellCmdletParameter<TParameter>>> selector,
        IPowerShellReferenceable<TParameter> variableWithIndex)
        where TCmdlet : IPowerShellCmdlet
        where TParameter : notnull
    {
        PowerShellParameter<TParameter> parameter = PowerShellParameterFactory.FromVariable(variableWithIndex);
        ExpressionBasedInstanceModifier.Instance.ModifyProperty(cmdlet, selector, parameter);
        return cmdlet;
    }

    public static TCmdlet Set<TCmdlet, TParameter>(
        this TCmdlet cmdlet,
        Expression<Func<TCmdlet, IPowerShellCmdletParameter<TParameter[]>>> selector,
        IPowerShellReferenceable<TParameter> variableWithIndex) where TCmdlet : IPowerShellCmdlet
    {
        PowerShellParameter<TParameter[]> parameter = PowerShellParameterFactory.FromVariableToArray(variableWithIndex);
        ExpressionBasedInstanceModifier.Instance.ModifyProperty(cmdlet, selector, parameter);
        return cmdlet;
    }

    public static TCmdlet Set<TCmdlet, TParameter>(
        this TCmdlet cmdlet,
        Expression<Func<TCmdlet, IPowerShellCmdletParameter<TParameter[]>>> selector,
        params IPowerShellReferenceable<TParameter>[] variableWithIndex) where TCmdlet : IPowerShellCmdlet
    {
        PowerShellParameter<TParameter[]> parameter = PowerShellParameterFactory.FromVariableToArray(variableWithIndex);
        ExpressionBasedInstanceModifier.Instance.ModifyProperty(cmdlet, selector, parameter);
        return cmdlet;
    }
}