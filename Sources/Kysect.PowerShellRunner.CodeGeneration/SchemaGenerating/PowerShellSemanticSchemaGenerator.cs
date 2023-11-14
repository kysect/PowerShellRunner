using Kysect.CommonLib.ProgressTracking;
using Kysect.PowerShellRunner.CodeGeneration.Compilation;
using Kysect.PowerShellRunner.CodeGeneration.SemanticParsing;
using Kysect.PowerShellRunner.CodeGeneration.SolutionReading;
using Kysect.PowerShellRunner.CodeGeneration.SyntaxParsing;
using Kysect.PowerShellRunner.CodeGeneration.UsedModelSearching;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace Kysect.PowerShellRunner.CodeGeneration.SchemaGenerating;

public class PowerShellSemanticSchemaGenerator<TSyntax, TSemantic>
    where TSyntax : CmdletBaseSyntaxInfo
    where TSemantic : CmdletBaseSemanticInfo
{
    private readonly CmdletUsedModelSearcher _usedModelSearcher;
    private readonly IProgressTrackerFactory _progressTrackerFactory;
    private readonly ISolutionSourceFileContentReader _sourceFileContentReader;
    private readonly SolutionCompilationContextFactory _solutionCompilationContextFactory;
    private readonly CmdletBaseSyntaxInfoParser _baseSyntaxParser;
    private readonly CmdletBaseSemanticInfoParser _baseSemanticInfoParser;
    private readonly IExtendedCmdletSyntaxInfoParser<TSyntax> _extendedCmdletSyntaxInfoParser;
    private readonly IExtendedCmdletSemanticInfoParser<TSyntax, TSemantic> _extendedSemanticParser;
    private readonly ILogger _logger;

    public PowerShellSemanticSchemaGenerator(
        ISolutionSourceFileContentReader sourceFileContentReader,
        RoslynSimpleModelSemanticDescriptorFactory modelSemanticDescriptorFactory,
        IExtendedCmdletSyntaxInfoParser<TSyntax> extendedSyntaxParser,
        IExtendedCmdletSemanticInfoParser<TSyntax, TSemantic> extendedSemanticParser,
        IProgressTrackerFactory progressTrackerFactory,
        ILogger logger)
    {
        _sourceFileContentReader = sourceFileContentReader;
        _extendedCmdletSyntaxInfoParser = extendedSyntaxParser;
        _extendedSemanticParser = extendedSemanticParser;
        _progressTrackerFactory = progressTrackerFactory;
        _logger = logger;

        _solutionCompilationContextFactory = new SolutionCompilationContextFactory(_progressTrackerFactory, _logger);
        _usedModelSearcher = new CmdletUsedModelSearcher(logger, modelSemanticDescriptorFactory);
        _baseSyntaxParser = new CmdletBaseSyntaxInfoParser(logger);
        _baseSemanticInfoParser = new CmdletBaseSemanticInfoParser(_logger);
    }

    public PowerShellSemanticSchema<TSemantic> Execute(string solutionPath)
    {
        _logger.LogInformation("Start cmdlet description generating for {SolutionPath}", solutionPath);
        IReadOnlyCollection<string> fileContents = _sourceFileContentReader.ReadFileContents(solutionPath);

        var progressTrackCollection = new CollectionProgressTracker<string>(_progressTrackerFactory, fileContents);

        CollectionProgressTracker<SyntaxTree> syntaxTrees = progressTrackCollection
            .SelectParallel("Parse C# syntax trees from files", fileContent => CSharpSyntaxTree.ParseText(fileContent));

        _logger.LogInformation("Creating semantic model");
        CSharpCompilation sharpCompilation = SharpCompilationProviderBuilder
            .CreateForStandard("Analysis")
            .AddReferences(typeof(SwitchParameter).Assembly)
            .Build();
        CSharpCompilation compilation = sharpCompilation.AddSyntaxTrees(syntaxTrees.Values);

        _logger.LogInformation("Getting information about types in solution");
        SolutionCompilationContext solutionCompilationContext = _solutionCompilationContextFactory.Create(compilation, syntaxTrees.Values);

        IReadOnlyCollection<TSyntax> cmdletBaseInheritorSyntaxParseResults = new CollectionProgressTracker<SolutionCompilationContextItem>(_progressTrackerFactory, solutionCompilationContext.Items)
            .SelectParallel("Parse cmdlet syntax info", TryParseCmdletSyntax)
            .Values
            .WhereNotNull()
            .ToList();

        IReadOnlyCollection<TSemantic> cmdletTypeDescriptors = new CollectionProgressTracker<TSyntax>(_progressTrackerFactory, cmdletBaseInheritorSyntaxParseResults)
            .SelectParallel("Parsing semantic model", syntaxParseResult => TryParseCmdletSemantic(solutionCompilationContext.TypeInheritancesSearcher, syntaxParseResult))
            .Values
            .WhereNotNull()
            .ToList();

        RoslynDependentTypeSearcherResult allUsedModels = _usedModelSearcher.GetAllUsedModels(solutionCompilationContext, cmdletTypeDescriptors);
        return new PowerShellSemanticSchema<TSemantic>(cmdletTypeDescriptors, allUsedModels.Models, allUsedModels.Enums);
    }

    private TSyntax? TryParseCmdletSyntax(SolutionCompilationContextItem compilationContextItem)
    {
        if (!_baseSyntaxParser.CanBeParsedAsCmdlet(compilationContextItem))
            return null;

        CmdletBaseSyntaxInfo baseSyntaxInfo = _baseSyntaxParser.ExtractSyntaxInfo(compilationContextItem);
        if (!_extendedCmdletSyntaxInfoParser.Acceptable(baseSyntaxInfo))
            return null;

        return _extendedCmdletSyntaxInfoParser.Parse(baseSyntaxInfo);
    }

    private TSemantic? TryParseCmdletSemantic(SolutionCompilationTypeInheritancesSearcher typeInheritancesSearcher, TSyntax syntaxParseResult)
    {
        try
        {
            CmdletBaseSemanticInfo cmdletBaseSemanticInfo = _baseSemanticInfoParser.Parse(syntaxParseResult, typeInheritancesSearcher);
            TSemantic cmdletBaseInheritorSemanticParseResult = _extendedSemanticParser.Parse(syntaxParseResult, cmdletBaseSemanticInfo);
            return cmdletBaseInheritorSemanticParseResult;
        }
        catch (Exception e)
        {
            string typeName = syntaxParseResult.SolutionCompilationContextItem.Symbol.Name;
            _logger.LogError("Cannot parse semantic for {typeName}: {message}", typeName, e.Message);
            return null;
        }
    }
}