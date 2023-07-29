using System;
using System.Collections.Generic;
using System.Linq;

namespace Kysect.PowerShellRunner.Abstractions.Variables;

public static class PowerShellVariableFilteredExtensions
{
    public static PowerShellVariableFiltered<T> Where<T>(this PowerShellVariable<T> variable, Predicate<T> predicate)
    {
        return new PowerShellVariableFiltered<T>(variable, predicate);
    }
}

public class PowerShellVariableFiltered<T>
{
    private readonly List<Predicate<T>> _predicates;

    public PowerShellVariable<T> Source { get; }

    public PowerShellVariableFiltered(PowerShellVariable<T> source, Predicate<T> predicate)
    {
        Source = source;
        _predicates = new List<Predicate<T>>
        {
            predicate
        };
    }

    public PowerShellVariableFiltered<T> Where(Predicate<T> predicate)
    {
        _predicates.Add(predicate);
        return this;
    }

    public IEnumerable<PowerShellVariableWithIndex<T>> GetElements()
    {
        for (int index = 0; index < Source.Values.Count; index++)
        {
            T value = Source.Values[index];
            if (_predicates.All(p => p(value)))
                yield return new PowerShellVariableWithIndex<T>(Source, index);
        }
    }
}