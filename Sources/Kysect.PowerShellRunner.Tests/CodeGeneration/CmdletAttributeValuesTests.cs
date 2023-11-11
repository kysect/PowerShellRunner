using FluentAssertions;
using Kysect.PowerShellRunner.CodeGeneration.SemanticParsing;
using NUnit.Framework;

namespace Kysect.PowerShellRunner.Tests.CodeGeneration;

public class CmdletAttributeValuesTests
{
    [Test]
    public void GetClassName_ShouldReturnCorrectValue()
    {
        var cmdletAttributeValues = new CmdletAttributeValues("Get", "Cats");

        cmdletAttributeValues.GetClassName().Should().Be("GetCats");
    }

    [Test]
    public void GetPowerShellAlias_ShouldReturnCorrectValue()
    {
        var cmdletAttributeValues = new CmdletAttributeValues("Get", "Cats");

        cmdletAttributeValues.GetPowerShellAlias().Should().Be("Get-Cats");
    }
}