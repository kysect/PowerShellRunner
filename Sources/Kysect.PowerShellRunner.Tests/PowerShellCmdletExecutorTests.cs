using FluentAssertions;
using Kysect.PowerShellRunner.Abstractions.Variables;
using Kysect.PowerShellRunner.Cmdlets;
using Kysect.PowerShellRunner.CustomCmdlets;
using Kysect.PowerShellRunner.Mapping;
using Kysect.PowerShellRunner.Tests.Mocks;
using NUnit.Framework;

namespace Kysect.PowerShellRunner.Tests;

public class PowerShellCmdletExecutorTests
{
    [Test]
    public void ExecuteAndSetTo_NonGenericImplementation_FinishWithoutError()
    {
        string expectedResult = @"C:\\Folder";

        using var testPowerShellAccessor = new FakePowerShellAccessor();
        var powerShellCmdletExecutor = new PowerShellCmdletExecutor(testPowerShellAccessor, PowerShellObjectMapper.Instance);
        testPowerShellAccessor.SetSuccessResult(new GetLocationCmdletWrapperResult(expectedResult));

        PowerShellVariable<GetLocationCmdletWrapperResult> result = powerShellCmdletExecutor.InitializeVariable("$result", new GetLocationCmdlet());

        GetLocationCmdletWrapperResult resultObject = result.Values.Single();
        resultObject.Path.Should().NotBeNull();
    }
}