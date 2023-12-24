using FluentAssertions;
using Kysect.CommonLib.DependencyInjection.Logging;
using Kysect.CommonLib.ProgressTracking;
using Kysect.PowerShellRunner.CodeGeneration.Common;
using Kysect.PowerShellRunner.CodeGeneration.Compilation;
using Kysect.PowerShellRunner.Tests.CodeGeneration.Tools;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;

namespace Kysect.PowerShellRunner.Tests.CodeGeneration;

public class RoslynSyntaxNodeExtensionsTests
{
    private readonly SolutionCompilationContextFactory _solutionCompilationContextFactory = new(new EmptyProgressTrackerFactory(), DefaultLoggerConfiguration.CreateConsoleLogger());

    [Test]
    public void GetInvocationExpressionByName_ReturnExpectedResult()
    {
        var input = """
                    public class C
                    {
                        public void First() { }
                        public void Second()
                        {
                            First();
                            First();
                        }
                    }
                    """;

        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(input);
        CSharpCompilation compilation = TestCompilationFactory.CreateCompilation(syntaxTree);
        SolutionCompilationContext compilationContext = _solutionCompilationContextFactory.Create(compilation, new[] { syntaxTree });

        IReadOnlyCollection<InvocationExpressionSyntax> firstInvocations = compilationContext.Items.Single().Syntax.GetInvocationExpressionByName("First");

        firstInvocations.Should().HaveCount(2);
    }
}