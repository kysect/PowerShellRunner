using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.CommonLib.Exceptions;
using Kysect.PowerShellRunner.Abstractions.Cmdlets;
using Kysect.PowerShellRunner.Abstractions.Queries;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using System.Reflection;

namespace Kysect.PowerShellRunner.Core.QueryBuilding;

public static class PowerShellQueryFactory
{
    public static PowerShellQuery BuildFromCmdlet(this IPowerShellCmdlet cmdlet)
    {
        var query = new PowerShellQuery(cmdlet.CmdletName);

        Type type = cmdlet.GetType();

        PropertyInfo[] properties = type.GetProperties();
        foreach (PropertyInfo propertyInfo in properties)
        {
            if (ShouldSkipProperty(propertyInfo))
                continue;

            if (propertyInfo.PropertyType == typeof(IPowerShellCmdletParameter<SwitchParameter>))
            {
                IPowerShellCmdletParameterValue? switchRawValue = GetRawPropertyValue(cmdlet, propertyInfo);
                if (switchRawValue is null)
                    continue;

                PowerShellCmdletParameterValue switchValue = switchRawValue.To<PowerShellCmdletParameterValue>();
                bool isSwitchSet = switchValue.Value.To<SwitchParameter>();
                query = isSwitchSet
                    ? query.WithKey(propertyInfo.Name)
                    : query.WithKey(propertyInfo.Name, false);

                continue;
            }

            string? preparedValueInvoke = GetPropertyValue(cmdlet, propertyInfo);
            if (preparedValueInvoke is null)
                continue;

            query = query.WithNoFormat(propertyInfo.Name, preparedValueInvoke);
        }

        return query;
    }

    private static bool ShouldSkipProperty(PropertyInfo propertyInfo)
    {
        if (propertyInfo.Name == nameof(IPowerShellCmdlet.CmdletName))
            return true;

        return false;
    }

    private static IPowerShellCmdletParameterValue? GetRawPropertyValue(IPowerShellCmdlet cmdlet, PropertyInfo propertyInfo)
    {
        const string prepareValueMethodName = nameof(PowerShellParameter<object>.GetValue);
        Type parameterType = propertyInfo.PropertyType;

        object propertyValue = propertyInfo.GetValue(cmdlet);
        if (propertyValue is null)
            return null;

        MethodInfo? prepareValueMethod = parameterType.GetMethod(prepareValueMethodName);
        if (prepareValueMethod is null)
            throw new ArgumentException($"Cannot get method {prepareValueMethodName}");

        return (IPowerShellCmdletParameterValue?)prepareValueMethod.Invoke(propertyValue, Array.Empty<object>());
    }

    private static string? GetPropertyValue(IPowerShellCmdlet cmdlet, PropertyInfo propertyInfo)
    {
        IPowerShellCmdletParameterValue? value = GetRawPropertyValue(cmdlet, propertyInfo);

        return value switch
        {
            PowerShellCmdletParameterReferenceValue powerShellParameterReferenceValue => powerShellParameterReferenceValue.ParameterName,
            PowerShellCmdletParameterValue powerShellParameterValue => PrepareValueInternal(powerShellParameterValue.Value),
            null => null,
            _ => throw SwitchDefaultException.OnUnexpectedType(nameof(value), value),
        };
    }

    private static string? PrepareValueInternal(object value)
    {
        static IEnumerable<string> Select(IEnumerable enumerable, Func<object, string> selector)
        {
            foreach (object o in enumerable)
                yield return selector(o);
        }

        if (value is IEnumerable enumerable && value is not string)
            return string.Join(", ", Select(enumerable, PrepareValue));

        return PrepareValue(value);
    }

    private static string PrepareValue(object value)
    {
        return value switch
        {
            string stringValue => $"\"{stringValue}\"",
            bool boolValue => boolValue ? "$True" : "$False",
            SwitchParameter switchParameter => switchParameter ? "True" : "False",
            _ => value.ToString()
        };
    }
}