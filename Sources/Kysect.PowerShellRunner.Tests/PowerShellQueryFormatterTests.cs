using Kysect.PowerShellRunner.Abstractions.Queries;
using Kysect.PowerShellRunner.Abstractions.Variables;

namespace Kysect.PowerShellRunner.Tests;

public class PowerShellQueryFormatterTests
{
    private readonly PowerShellQueryFormatter _sut;

    public PowerShellQueryFormatterTests()
    {
        _sut = new PowerShellQueryFormatter();
    }

    [Test]
    public void PowerShellQuery_with_only_query_return_expected_value()
    {
        var expectedValue = "GetValue";
        var query = new PowerShellQuery("GetValue");

        string result = _sut.Format(query);

        Assert.That(result, Is.EqualTo(expectedValue));
    }

    [Test]
    public void PowerShellQuery_with_redirection_path_return_expected_value()
    {
        var expectedValue = "GetValue *> \"Log/file.log\"";
        var query = new PowerShellQuery("GetValue").WithRedirection("Log/file.log");

        string result = _sut.Format(query);

        Assert.That(result, Is.EqualTo(expectedValue));
    }

    [Test]
    public void PowerShellQuery_with_varialbe_return_expected_value()
    {
        var expectedValue = "$variable_name = GetValue";
        var query = new PowerShellQuery("GetValue");

        query = query with { ResultVariable = new PowerShellVariable("$variable_name") };
        string result = _sut.Format(query);

        Assert.That(result, Is.EqualTo(expectedValue));
    }
}