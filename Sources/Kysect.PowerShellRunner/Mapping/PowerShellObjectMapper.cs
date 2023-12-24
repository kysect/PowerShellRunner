using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.CommonLib.Reflection;
using Kysect.CommonLib.Reflection.TypeCache;
using Kysect.PowerShellRunner.Abstractions.Objects;
using Kysect.PowerShellRunner.Tools;
using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace Kysect.PowerShellRunner.Mapping;

public class PowerShellObjectMapper
{
    private readonly PowerShellObjectMappingConfiguration _configuration;
    private readonly JsonSerializerOptions _serializerOptions;

    public static PowerShellObjectMapper Instance { get; } = Create();
    public static PowerShellObjectMapper Create()
    {
        return new PowerShellObjectMapper(
            new PowerShellObjectMappingConfiguration(),
            new JsonSerializerOptions());
    }

    public PowerShellObjectMapper(PowerShellObjectMappingConfiguration configuration, JsonSerializerOptions serializerOptions)
    {
        _configuration = configuration;
        _serializerOptions = serializerOptions;
    }

    public T Map<T>(IPowerShellObject powerShellObject) where T : notnull
    {
        powerShellObject.ThrowIfNull();

        var reflectionInstanceInitializer = new ReflectionInstanceInitializer<T>();
        foreach (IPowerShellObjectMember powerShellMember in powerShellObject.GetProperties())
        {

            if (powerShellMember.Value is null)
                continue;

            PropertyInfo? targetProperty = TypeInstanceCache<T>.GetPublicProperties().SingleOrDefault(p => p.Name == powerShellMember.Name);
            if (targetProperty is null)
                continue;

            object convertValue = ConvertValue(powerShellMember.Value);
            if (targetProperty.PropertyType != convertValue.GetType())
            {
                convertValue = Map(convertValue, targetProperty);
            }

            reflectionInstanceInitializer.Set(powerShellMember.Name, convertValue);
        }

        return reflectionInstanceInitializer.GetValue();
    }

    private object ConvertValue(object sourceValue)
    {
        if (_configuration.TryMap(sourceValue, out object? converterValue))
            return converterValue;

        return sourceValue;
    }

    // TODO: move to ReflectionJsonInstanceCreator
    private object Map(object sourceValue, PropertyInfo targetProperty)
    {
        try
        {
            string serializeObject = JsonSerializer.Serialize(sourceValue, _serializerOptions);
            object? result = JsonSerializer.Deserialize(serializeObject, targetProperty.PropertyType, _serializerOptions);
            return result.ThrowIfNull();
        }
        catch (Exception e)
        {
            throw new PowerShellIntegrationException($"Cannot map instance of type {sourceValue.GetType().Name} to type {targetProperty.PropertyType.Name}", e);
        }
    }
}