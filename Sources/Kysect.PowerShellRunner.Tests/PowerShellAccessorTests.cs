using FluentAssertions;
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
        var testPowerShellAccessor = new TestPowerShellAccessor();
        testPowerShellAccessor.SetSuccessResult(new GetLocationCmdletWrapperResult(expectedResult));

        IReadOnlyCollection<GetLocationCmdletWrapperResult> results = testPowerShellAccessor
            .SelectCmdlet(new GetLocationCmdlet())
            .Execute();

        results
            .Should().HaveCount(1)
            .And.Subject.ElementAt(0).Path.Should().Be(expectedResult);
    }
}