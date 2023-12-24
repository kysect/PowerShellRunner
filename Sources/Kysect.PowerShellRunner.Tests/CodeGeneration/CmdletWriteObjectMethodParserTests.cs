using FluentAssertions;
using Kysect.CommonLib.DependencyInjection.Logging;
using Kysect.PowerShellRunner.CodeGeneration.Common;
using Kysect.PowerShellRunner.CodeGeneration.Compilation;
using Kysect.PowerShellRunner.CodeGeneration.SemanticParsing;
using Kysect.PowerShellRunner.CodeGeneration.SyntaxParsing;
using Kysect.PowerShellRunner.Tests.CodeGeneration.Tools;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace Kysect.PowerShellRunner.Tests.CodeGeneration;

public class CmdletWriteObjectMethodParserTests
{
    private readonly CmdletWriteObjectMethodParser _cmdletWriteObjectMethodParser;

    public CmdletWriteObjectMethodParserTests()
    {
        ILogger logger = DefaultLoggerConfiguration.CreateConsoleLogger();
        _cmdletWriteObjectMethodParser = new CmdletWriteObjectMethodParser(logger);
    }

    [Test]
    public void ArgumentEnumerable_ShouldBeCorrect()
    {
        string input = """
                       using System.Linq;

                       public class Container
                       {
                           public void Do()
                           {
                               var values = Enumerable.Range(1, 10);
                               WriteObject(values);
                           }

                           public void WriteObject<T>(T input)
                           {
                           }
                       }
                       """;

        var syntaxTreeTestFacade = SyntaxTreeTestFacade.Create(input);

        InvocationExpressionSyntax invocation = syntaxTreeTestFacade.GetInvocationExpressionByName(CmdletBaseSyntaxInfoParser.WriteObjectMethodName).Single();
        SolutionCompilationContextItem solutionCompilationContext = syntaxTreeTestFacade.CreateCompilationContext().Items.Single();
        bool isParsed = _cmdletWriteObjectMethodParser.TryParseFirstArgumentType(solutionCompilationContext, invocation, out ITypeSymbol? _);

        isParsed.Should().BeTrue();
    }

    [Test]
    public void ArgumentEnumerableWithToPipeline_ShouldBeCorrect()
    {
        string input = """
                       using System.Linq;

                       public static class Ext
                       {
                           public static object ToPipeline(this object value) { return value; }
                       }

                       public class GetCats
                       {
                           public void Do()
                           {
                               object values = Enumerable.Range(1, 10).Select(i => i + 1).ToPipeline();
                               WriteObject(values);
                           }
                       
                           public void WriteObject<T>(T input)
                           {
                           }
                       }
                       """;

        var syntaxTreeTestFacade = SyntaxTreeTestFacade.Create(input);
        InvocationExpressionSyntax invocation = syntaxTreeTestFacade.GetInvocationExpressionByName(CmdletBaseSyntaxInfoParser.WriteObjectMethodName).Single();
        SolutionCompilationContextItem solutionCompilationContext = syntaxTreeTestFacade.CreateCompilationContextItem("GetCats");
        bool isParsed = _cmdletWriteObjectMethodParser.TryParseFirstArgumentType(solutionCompilationContext, invocation, out ITypeSymbol? result);

        isParsed.Should().BeTrue();
        result.Should().NotBeNull();
        result!.GetNameWithContainingParent().Should().Be("int");
    }

    [Test]
    public void ArgumentEnumerableWithCast_ShouldBeCorrect()
    {
        string input = """
                       using System.Linq;

                       public class FirstType
                       {
                       }

                       public class SecondType
                       {
                           public static implicit operator SecondType(FirstType session)
                           {
                               return new SecondType();
                           }
                       }

                       public static class Ext
                       {
                           public static object ToPipeline(this object value) { return value; }
                       }

                       public class GetCats
                       {
                           public void Do()
                           {
                               var values = Enumerable.Repeat(new FirstType(), 1).Select(f => (SecondType)f);
                               WriteObject(values);
                           }
                       
                           public void WriteObject<T>(T input)
                           {
                           }
                       }
                       """;

        var syntaxTreeTestFacade = SyntaxTreeTestFacade.Create(input);
        InvocationExpressionSyntax invocation = syntaxTreeTestFacade.GetInvocationExpressionByName(CmdletBaseSyntaxInfoParser.WriteObjectMethodName).Single();
        SolutionCompilationContextItem solutionCompilationContext = syntaxTreeTestFacade.CreateCompilationContextItem("GetCats");
        bool isParsed = _cmdletWriteObjectMethodParser.TryParseFirstArgumentType(solutionCompilationContext, invocation, out ITypeSymbol? result);

        isParsed.Should().BeTrue();
        result.Should().NotBeNull();
        result!.GetNameWithContainingParent().Should().Be("SecondType");
    }
}