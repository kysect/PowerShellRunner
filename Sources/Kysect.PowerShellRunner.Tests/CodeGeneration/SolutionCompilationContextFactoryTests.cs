using FluentAssertions;
using Kysect.CommonLib.DependencyInjection.Logging;
using Kysect.CommonLib.ProgressTracking;
using Kysect.PowerShellRunner.CodeGeneration.Compilation;
using Kysect.PowerShellRunner.Tests.CodeGeneration.Tools;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;

namespace Kysect.PowerShellRunner.Tests.CodeGeneration;

public class SolutionCompilationContextFactoryTests
{
    private readonly SolutionCompilationContextFactory _solutionCompilationContextFactory = new(new EmptyProgressTrackerFactory(), DefaultLoggerConfiguration.CreateConsoleLogger());

    [Test]
    [TestCase("public class A { }")]
    [TestCase("public enum A { }")]
    [TestCase("public struct A { }")]
    [TestCase("public record A { }")]
    public void Create_ReturnItem(string typeDeclaration)
    {
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(typeDeclaration);
        CSharpCompilation compilation = TestCompilationFactory.CreateCompilation(syntaxTree);

        SolutionCompilationContext compilationContext = _solutionCompilationContextFactory.Create(compilation, new[] { syntaxTree });

        compilationContext.Items.Should().HaveCount(1);
    }
}