using Kysect.PowerShellRunner.CodeGeneration.SemanticParsing;
using Kysect.PowerShellRunner.CodeGeneration.UsedModelSearching;
using System.Collections.Generic;

namespace Kysect.PowerShellRunner.CodeGeneration.SchemaGenerating;

public class PowerShellCmdletDescriptor
{
    public IReadOnlyCollection<string> Scope { get; }
    public CmdletAttributeValues CmdletAttributeValues { get; }
    public IReadOnlyCollection<SimpleModelPropertyDescriptor> PropertyDeclarations { get; }
    public string? MainReturnType { get; }
    public IReadOnlyCollection<string> ResolvedReturnTypes { get; }

    public PowerShellCmdletDescriptor(
        IReadOnlyCollection<string> scope,
        CmdletAttributeValues cmdletAttributeValues,
        IReadOnlyCollection<SimpleModelPropertyDescriptor> propertyDeclarations,
        string? mainReturnType,
        IReadOnlyCollection<string> resolvedReturnTypes)
    {
        Scope = scope;
        CmdletAttributeValues = cmdletAttributeValues;
        PropertyDeclarations = propertyDeclarations;
        MainReturnType = mainReturnType;
        ResolvedReturnTypes = resolvedReturnTypes;
    }
}