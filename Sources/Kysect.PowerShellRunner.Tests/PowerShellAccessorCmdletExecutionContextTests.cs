using FluentAssertions;
using Kysect.PowerShellRunner.Abstractions.Objects;
using Kysect.PowerShellRunner.CustomCmdlets;
using Kysect.PowerShellRunner.Executions;
using Kysect.PowerShellRunner.Tests.Mocks;
using NUnit.Framework;

namespace Kysect.PowerShellRunner.Tests;

public class PowerShellAccessorCmdletExecutionContextTests
{
    [Test]
    public void ExecuteAndSetTo_NonGenericImplementation_FinishWithoutError()
    {
        string expectedResult = @"C:\\Folder";

        using var testPowerShellAccessor = new FakePowerShellAccessor();
        var executionContext = new PowerShellAccessorCmdletExecutionContext(testPowerShellAccessor, new GetLocationCmdlet());
        testPowerShellAccessor.SetSuccessResult(new GetLocationCmdletWrapperResult(expectedResult));

        var result = executionContext.ExecuteAndSetTo("$result");

        IPowerShellObject resultObject = result.Values.Single();
        IPowerShellObjectMember? pathProperty = resultObject.GetProperties().FirstOrDefault(p => p.Name == nameof(GetLocationCmdletWrapperResult.Path));
        pathProperty.Should().NotBeNull();
    }
}