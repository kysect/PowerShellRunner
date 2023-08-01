using FluentAssertions;
using Kysect.PowerShellRunner.Abstractions.Variables;
using NUnit.Framework;

namespace Kysect.PowerShellRunner.Tests;

public class PowerShellVariableTests
{
    [Test]
    public void Ctor_ForValidName_NoThrow()
    {
        Assert.DoesNotThrow(() =>
        {
            var variable = new PowerShellVariable("$variable_name");
        });
    }

    [Test]
    public void Ctor_ForNameWithoutSymbol_ThrowException()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var variable = new PowerShellVariable("variable_name");
        });
    }

    [Test]
    public void EnumerateElements_ForVariableWithElement_ReturnCorrectNamesForEachElement()
    {
        var variable = new PowerShellVariable<int>("$variable_name", new[] { 1, 2, 3 });

        var references = variable
            .EnumerateElements()
            .Select(e => e.AsReference())
            .Select(r => r.ToString())
            .ToList();

        references
            .Should().HaveCount(3)
            .And.Equal("$variable_name[0]", "$variable_name[1]", "$variable_name[2]");
    }

    [Test]
    public void Where_FilterElement_ReturnWithoutFiltered()
    {
        var variable = new PowerShellVariable<int>("$variable_name", new[] { 1, 2, 3 });

        var references = variable
            .Where(v => v != 2)
            .EnumerateElements()
            .Select(e => e.AsReference())
            .Select(r => r.ToString())
            .ToList();

        references
            .Should().HaveCount(2)
            .And.Equal("$variable_name[0]", "$variable_name[2]");
    }

    [Test]
    public void ReferenceCollectionCreate_ForManyVariables_ReturnExpectedResult()
    {
        var variable = new PowerShellVariable<int>("$variable_name", new[] { 1, 2, 3 })
            .EnumerateElements()
            .ToList();

        var referenceCollection = PowerShellReferenceCollection.Create(variable);

        referenceCollection.Name
            .Should().Be("$variable_name[0],$variable_name[1],$variable_name[2]");
    }
}