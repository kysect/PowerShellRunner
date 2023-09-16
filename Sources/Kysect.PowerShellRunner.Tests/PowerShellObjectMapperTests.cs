using FluentAssertions;
using Kysect.PowerShellRunner.Abstractions.Objects;
using Kysect.PowerShellRunner.Core.Mapping;
using Kysect.PowerShellRunner.Tests.Mocks;
using NUnit.Framework;

namespace Kysect.PowerShellRunner.Tests;

public class PowerShellObjectMapperTests
{
    [Test]
    public void Map_SimplePoco_ReturnExpectedResult()
    {
        var mapper = PowerShellObjectMapper.Create();
        var input = new SimplePoco(1, "1", new[] { 2, 3 });
        IPowerShellObject powerShellObject = new FakePowerShellObject<SimplePoco>(input);

        SimplePoco output = mapper.Map<SimplePoco>(powerShellObject);

        output.Should().BeEquivalentTo(input);
    }

    [Test]
    public void Map_TypeWithComplexProperties_ReturnExpectedResult()
    {
        var mapper = PowerShellObjectMapper.Create();
        var poco = new SimplePoco(1, "1", new[] { 2, 3 });
        var input = new TypeWithComplexProperties(poco);

        IPowerShellObject powerShellObject = new FakePowerShellObject<TypeWithComplexProperties>(input);

        TypeWithComplexProperties output = mapper.Map<TypeWithComplexProperties>(powerShellObject);

        output.Should().BeEquivalentTo(input);
    }

    [Test]
    public void Map_TypeWithEquivalentProperties_ReturnExpectedResult()
    {
        var mapper = PowerShellObjectMapper.Create();
        var poco = new SimplePoco(1, "1", new[] { 2, 3 });
        var input = new TypeWithComplexProperties(poco);

        IPowerShellObject powerShellObject = new FakePowerShellObject<TypeWithComplexProperties>(input);

        OtherTypeWithComplexProperties output = mapper.Map<OtherTypeWithComplexProperties>(powerShellObject);

        output.Should().BeEquivalentTo(input);
    }

    public class SimplePoco
    {
        public int Value { get; }
        public string Value2 { get; }
        public IReadOnlyCollection<int> Value3 { get; }

        public SimplePoco(int value, string value2, IReadOnlyCollection<int> value3)
        {
            Value = value;
            Value2 = value2;
            Value3 = value3;
        }
    }

    public class OtherSimplePoco
    {
        public int Value { get; }
        public string Value2 { get; }
        public IReadOnlyCollection<int> Value3 { get; }

        public OtherSimplePoco(int value, string value2, IReadOnlyCollection<int> value3)
        {
            Value = value;
            Value2 = value2;
            Value3 = value3;
        }
    }

    public class TypeWithComplexProperties
    {
        public SimplePoco Value1 { get; }

        public TypeWithComplexProperties(SimplePoco value1)
        {
            Value1 = value1;
        }
    }

    public class OtherTypeWithComplexProperties
    {
        public SimplePoco Value1 { get; }

        public OtherTypeWithComplexProperties(SimplePoco value1)
        {
            Value1 = value1;
        }
    }
}