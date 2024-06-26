﻿using Kysect.PowerShellRunner.Abstractions.Objects;
using System.Management.Automation;

namespace Kysect.PowerShellRunner.Accessors.Models;

internal class PowerShellObject : IPowerShellObject
{
    private readonly PSObject _powerShellObject;

    public PowerShellObject(PSObject powerShellObject)
    {
        _powerShellObject = powerShellObject;
    }

    public IReadOnlyCollection<IPowerShellObjectMember> GetProperties()
    {
        return _powerShellObject
            .Properties
            .Select(m => new PowerShellObjectMember(m))
            .ToList();
    }

    public string AsString()
    {
        return _powerShellObject.ToString();
    }

    public override string ToString()
    {
        return _powerShellObject.ToString();
    }
}