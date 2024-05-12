using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.CommonLib.ProgressTracking;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;

namespace Kysect.PowerShellRunner.CodeGeneration.Compilation;

public class SolutionCompilationContextFactory
{
    private readonly IProgressTrackerFactory _progressTrackerFactory;
    private readonly ILogger _logger;

    public SolutionCompilationContextFactory(IProgressTrackerFactory progressTrackerFactory, ILogger logger)
    {
        _progressTrackerFactory = progressTrackerFactory.ThrowIfNull();
        _logger = logger.ThrowIfNull();
    }

    public SolutionCompilationContext Create(CSharpCompilation comp, IReadOnlyCollection<SyntaxTree> syntaxTrees)
    {
        var syntaxTreesCollection = new CollectionProgressTracker<SyntaxTree>(_progressTrackerFactory, syntaxTrees);

        CollectionProgressTracker<SolutionCompilationContextItem> solutionCompilationContextItems = syntaxTreesCollection
            .SelectManyParallel("Resolving type symbols for parsed syntax trees", t => ParseContextItems(comp, t));

        _logger.LogInformation("Creating type inheriting map");
        var solutionCompilationTypeSearcher = SolutionCompilationTypeInheritancesSearcher.CreateInstance(solutionCompilationContextItems.Values);
        return new SolutionCompilationContext(comp, solutionCompilationContextItems.Values, solutionCompilationTypeSearcher);
    }

    private IReadOnlyCollection<SolutionCompilationContextItem> ParseContextItems(CSharpCompilation compilation, SyntaxTree tree)
    {
        var result = new List<SolutionCompilationContextItem>();

        CompilationUnitSyntax root = tree.GetRoot().To<CompilationUnitSyntax>();
        SemanticModel sm = compilation.GetSemanticModel(tree, true);

        foreach (BaseTypeDeclarationSyntax typeDeclarationSyntax in root.DescendantNodes().OfType<BaseTypeDeclarationSyntax>())
        {
            INamedTypeSymbol? namedTypeSymbol = sm.GetDeclaredSymbol(typeDeclarationSyntax);
            if (namedTypeSymbol is not null)
                result.Add(new SolutionCompilationContextItem(sm, typeDeclarationSyntax, namedTypeSymbol));
        }

        return result;
    }
}