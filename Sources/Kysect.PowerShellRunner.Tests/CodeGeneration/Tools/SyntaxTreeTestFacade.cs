using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.CommonLib.DependencyInjection;
using Kysect.CommonLib.ProgressTracking;
using Kysect.PowerShellRunner.CodeGeneration.Common;
using Kysect.PowerShellRunner.CodeGeneration.Compilation;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kysect.PowerShellRunner.Tests.CodeGeneration.Tools;

public class SyntaxTreeTestFacade
{
    private readonly SolutionCompilationContextFactory _solutionCompilationContextFactory = new(new EmptyProgressTrackerFactory(), PredefinedLogger.CreateConsoleLogger());

    public SyntaxTree SyntaxTree { get; }
    public CSharpCompilation Compilation { get; }
    public SemanticModel SemanticModel { get; }

    public static SyntaxTreeTestFacade Create(string input)
    {
        input.ThrowIfNull();

        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(input);
        CSharpCompilation compilation = TestCompilationFactory.CreateCompilation(syntaxTree);
        return new SyntaxTreeTestFacade(syntaxTree, compilation);
    }

    public SyntaxTreeTestFacade(SyntaxTree syntaxTree, CSharpCompilation compilation)
    {
        SyntaxTree = syntaxTree.ThrowIfNull();
        Compilation = compilation.ThrowIfNull();
        SemanticModel = Compilation.GetSemanticModel(SyntaxTree, true);
    }

    public BaseTypeDeclarationSyntax GetTypeSyntax(string name)
    {
        CompilationUnitSyntax root = SyntaxTree.GetRoot().To<CompilationUnitSyntax>();
        return root.DescendantNodes().OfType<BaseTypeDeclarationSyntax>().Single(i => i.Identifier.Text == name);
    }

    public INamedTypeSymbol GetTypeSymbol(string name)
    {
        CompilationUnitSyntax root = SyntaxTree.GetRoot().To<CompilationUnitSyntax>();

        BaseTypeDeclarationSyntax classC = root.DescendantNodes().OfType<BaseTypeDeclarationSyntax>().Single(i => i.Identifier.Text == name);
        INamedTypeSymbol? classSymbol = SemanticModel.GetDeclaredSymbol(classC);

        return classSymbol.ThrowIfNull();
    }

    public SolutionCompilationContext CreateCompilationContext()
    {
        return _solutionCompilationContextFactory.Create(Compilation, new[] { SyntaxTree });
    }

    public SolutionCompilationContextItem CreateCompilationContextItem(string typeName)
    {
        SolutionCompilationContext solutionCompilationContext = _solutionCompilationContextFactory.Create(Compilation, new[] { SyntaxTree });
        return solutionCompilationContext.Items.Single(i => i.Symbol.Name == typeName);
    }

    public IReadOnlyCollection<InvocationExpressionSyntax> GetInvocationExpressionByName(string name)
    {
        name.ThrowIfNull();

        return SyntaxTree
            .GetRoot()
            .DescendantNodes()
            .OfType<BaseTypeDeclarationSyntax>()
            .SelectMany(t => t.GetInvocationExpressionByName(name))
            .ToList();
    }
}