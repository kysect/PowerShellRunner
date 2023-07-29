using Kysect.PowerShellRunner.Abstractions.Queries;
using Kysect.PowerShellRunner.Abstractions.Variables;

namespace Kysect.PowerShellRunner.Tests;
public class PowerShellQueryBuilderTests
{
    private readonly PowerShellQueryBuilder _sut;

    public PowerShellQueryBuilderTests()
    {
        _sut = new PowerShellQueryBuilder();
    }

    [Test]
    public void PowerShellQuery_with_only_query_return_expected_value()
    {
        var expectedValue = "GetValue";
        var query = new PowerShellQueryArguments("GetValue");

        string result = _sut.Build(query);

        Assert.That(result, Is.EqualTo(expectedValue));
    }

    [Test]
    public void PowerShellQuery_with_redirection_path_return_expected_value()
    {
        var expectedValue = "GetValue *> \"Log/file.log\"";
        var query = new PowerShellQueryArguments("GetValue", "Log/file.log");

        string result = _sut.Build(query);

        Assert.That(result, Is.EqualTo(expectedValue));
    }

    [Test]
    public void PowerShellQuery_with_varialbe_return_expected_value()
    {
        var expectedValue = "$variable_name = GetValue";
        var query = new PowerShellQueryArguments("GetValue").WithVariable(new PowerShellVariable("variable_name"));

        string result = _sut.Build(query);

        Assert.That(result, Is.EqualTo(expectedValue));
    }
}