using FluentAssertions;
using Kysect.CommonLib.DependencyInjection.Logging;
using Kysect.PowerShellRunner.CodeGeneration.Compilation;
using Kysect.PowerShellRunner.CodeGeneration.SemanticParsing;
using Kysect.PowerShellRunner.CodeGeneration.UsedModelSearching;
using Kysect.PowerShellRunner.Tests.CodeGeneration.Tools;
using NUnit.Framework;

namespace Kysect.PowerShellRunner.Tests.CodeGeneration;

public class CmdletUsedModelSearcherTests
{
    [Test]
    public void GetAllUsedModels_ShouldReturnModels()
    {
        // TODO: inject CmdletAttribute instead of duplication
        var input = """
                    using System;
                    using First;
                    
                    namespace First
                    {
                        public class MyType { }
                        public enum MyEnum { }
                    }
                    
                    public class CmdletAttribute : System.Attribute
                    {
                        public CmdletAttribute(string verbName, string nounName) { }
                    }
                    public class ParameterAttribute : Attribute { }
                    public class PSCmdletBase { }
                    
                    [Cmdlet("SomeVerb", "AndNoun")]
                    public class MyCmdlet : PSCmdletBase
                    {
                        [Parameter] public MyType Value1 { get; set; }
                        [Parameter] public MyEnum Value2 { get; set; }
                        [Parameter] public Int32 Value3 { get; set; }
                    }
                    """;

        var simpleModelSemanticDescriptorFactory = new RoslynSimpleModelSemanticDescriptorFactory(new DummyRoslynSimpleModelBaseTypeFilter(), new DummyRoslynSimpleModelPropertyFilter());
        var cmdletUsedModelSearcher = new CmdletUsedModelSearcher(DefaultLoggerConfiguration.CreateConsoleLogger(), simpleModelSemanticDescriptorFactory);

        var syntaxTreeTestFacade = SyntaxTreeTestFacade.Create(input);
        SolutionCompilationContext compilationContext = syntaxTreeTestFacade.CreateCompilationContext();
        CmdletBaseSemanticInfo semanticInfo = syntaxTreeTestFacade.CreateCmdletSemanticInfo("MyCmdlet");
        RoslynDependentTypeSearcherResult roslynDependentTypeSearcherResult = cmdletUsedModelSearcher.GetAllUsedModels(compilationContext, new[] { semanticInfo });

        roslynDependentTypeSearcherResult.Models.Should().HaveCount(1);
        roslynDependentTypeSearcherResult.Enums.Should().HaveCount(1);
    }

    [Test]
    public void GetAllUsedModels_ShouldReturnModelsWithNamespace()
    {
        // TODO: inject CmdletAttribute instead of duplication
        var input = """
                    using System;
                    using First;

                    namespace First
                    {
                        public class MyType { }
                        public enum MyEnum { }
                    }

                    public class CmdletAttribute : System.Attribute
                    {
                        public CmdletAttribute(string verbName, string nounName) { }
                    }
                    public class ParameterAttribute : Attribute { }
                    public class PSCmdletBase { }

                    [Cmdlet("SomeVerb", "AndNoun")]
                    public class MyCmdlet : PSCmdletBase
                    {
                        [Parameter] public MyType Value1 { get; set; }
                        [Parameter] public MyEnum Value2 { get; set; }
                        [Parameter] public Int32 Value3 { get; set; }
                    }
                    """;

        var simpleModelSemanticDescriptorFactory = new RoslynSimpleModelSemanticDescriptorFactory(new DummyRoslynSimpleModelBaseTypeFilter(), new DummyRoslynSimpleModelPropertyFilter());
        var cmdletUsedModelSearcher = new CmdletUsedModelSearcher(DefaultLoggerConfiguration.CreateConsoleLogger(), simpleModelSemanticDescriptorFactory);

        var syntaxTreeTestFacade = SyntaxTreeTestFacade.Create(input);
        SolutionCompilationContext compilationContext = syntaxTreeTestFacade.CreateCompilationContext();
        CmdletBaseSemanticInfo semanticInfo = syntaxTreeTestFacade.CreateCmdletSemanticInfo("MyCmdlet");
        RoslynDependentTypeSearcherResult roslynDependentTypeSearcherResult = cmdletUsedModelSearcher.GetAllUsedModels(compilationContext, new[] { semanticInfo });

        roslynDependentTypeSearcherResult.Models.Should().HaveCount(1);
        roslynDependentTypeSearcherResult.Models.Single().TypeNamespace.Should().Be("First");
    }
}