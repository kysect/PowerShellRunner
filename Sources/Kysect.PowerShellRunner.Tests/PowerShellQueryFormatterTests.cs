using FluentAssertions;
using Kysect.PowerShellRunner.Abstractions.Queries;
using Kysect.PowerShellRunner.Abstractions.Variables;
using NUnit.Framework;

namespace Kysect.PowerShellRunner.Tests;

public class PowerShellQueryFormatterTests
{
    private readonly PowerShellQueryFormatter _sut;

    public PowerShellQueryFormatterTests()
    {
        _sut = new PowerShellQueryFormatter();
    }

    [Test]
    public void Format_QueryWithCmdletName_ReturnCorrectQuery()
    {
        string expectedValue = "Get-Value";
        var query = new PowerShellQuery(expectedValue);

        string result = _sut.Format(query);

        Assert.That(result, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Format_WithRedirection_ReturnCorrectQuery()
    {
        var expectedValue = "GetValue *> \"Log/file.log\"";
        var query = new PowerShellQuery("GetValue").WithRedirection("Log/file.log");

        string result = _sut.Format(query);

        Assert.That(result, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Format_ResultVariableSet_ReturnCorrectQuery()
    {
        var expectedValue = "$variable_name = GetValue";
        var query = new PowerShellQuery("GetValue");
        var variable = new PowerShellVariable("$variable_name");

        query = query with { ResultVariable = variable };
        string result = _sut.Format(query);

        Assert.That(result, Is.EqualTo(expectedValue));
    }

    [Test]
    public void With_SimpleVariable_ReturnCorrectQuery()
    {
        var query = new PowerShellQuery("Get-Value");
        var variable = new PowerShellVariable("$variable");

        query = query.With("Parameter", variable);

        query.Query
            .Should().Be("Get-Value -Parameter $variable");
    }

    [Test]
    public void With_Switch_ReturnCorrectQuery()
    {
        var query = new PowerShellQuery("Get-Value");

        query = query.WithSwitch("Force");

        query.Query
            .Should().Be("Get-Value -Force");
    }

    [Test]
    public void With_Variable_ReturnCorrectQuery()
    {
        var query = new PowerShellQuery("Get-Value");
        var variable = new PowerShellVariable("$variable_name");

        query = query.With("Parameter", variable);

        query.Query
            .Should().Be("Get-Value -Parameter $variable_name");
    }

    [Test]
    public void With_StringParameterValue_ReturnCorrectQuery()
    {
        var query = new PowerShellQuery("Get-Value");

        query = query.With("Parameter", "value");

        query.Query
            .Should().Be("Get-Value -Parameter \"value\"");
    }

    [Test]
    public void With_IntParameterValue_ReturnCorrectQuery()
    {
        var query = new PowerShellQuery("Get-Value");

        query = query.With("Parameter", 1);

        query.Query
            .Should().Be("Get-Value -Parameter 1");
    }

    [Test]
    public void With_Pipe_ReturnCorrectQuery()
    {
        var query = new PowerShellQuery("Get-Value");

        query = query
            .WithPipeline()
            .WherePropertyEqual("Property", "Value");

        query.Query
            .Should().Be("Get-Value | Where Property -eq \"Value\"");
    }
}