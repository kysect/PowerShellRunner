using FluentAssertions;
using Kysect.PowerShellRunner.Abstractions.Accessors.Results;
using Kysect.PowerShellRunner.Abstractions.Queries;
using Kysect.PowerShellRunner.Core.CustomCmdlets;
using Kysect.PowerShellRunner.Core.Extensions;
using Kysect.PowerShellRunner.Tests.Mocks;
using NUnit.Framework;

namespace Kysect.PowerShellRunner.Tests;

public class PowerShellAccessorTests
{
    [Test]
    public void Execute_GetLocationCmdlet_ReturnExpectedModel()
    {
        string expectedResult = @"C:\\Folder";
        using var testPowerShellAccessor = new FakePowerShellAccessor();
        testPowerShellAccessor.SetSuccessResult(new GetLocationCmdletWrapperResult(expectedResult));

        IReadOnlyCollection<GetLocationCmdletWrapperResult> results = testPowerShellAccessor
            .SelectCmdlet(new GetLocationCmdlet())
            .Execute();

        results
            .Should().HaveCount(1)
            .And.Subject.ElementAt(0).Path.Should().Be(expectedResult);
    }

    [Test]
    public void Execute_WithError_ShouldThrowException()
    {
        using var testPowerShellAccessor = new FakePowerShellAccessor();

        testPowerShellAccessor.SetFailedResult("Some error");

        IPowerShellExecutionResult powerShellExecutionResult = testPowerShellAccessor.Execute(new PowerShellQuery());

        powerShellExecutionResult.Should().BeOfType<PowerShellFailedExecutionResult>();
    }
}