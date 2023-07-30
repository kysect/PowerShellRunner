using FluentAssertions;
using Kysect.PowerShellRunner.Abstractions.Queries;
using Kysect.PowerShellRunner.Core.CustomCmdlets;
using Kysect.PowerShellRunner.Core.Extensions;
using Kysect.PowerShellRunner.Core.QueryBuilding;
using NUnit.Framework;

namespace Kysect.PowerShellRunner.Tests;

public class PowerShellQueryFactoryTests
{
    [Test]
    public void BuildFromCmdlet_ConvertToSecureString_ReturnExpectedString()
    {
        ConvertToSecureStringCmdlet cmdlet = new ConvertToSecureStringCmdlet()
            .Set(c => c.String, "Value")
            .Set(c => c.AsPlainText, true)
            .Set(c => c.Force, false);

        PowerShellQuery query = cmdlet.BuildFromCmdlet();

        query.Query
            .Should().Be("ConvertTo-SecureString -String \"Value\" -AsPlainText");
    }
}