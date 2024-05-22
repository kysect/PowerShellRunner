using Kysect.CommonLib.ProgressTracking;
using Kysect.DotnetProjectSystem.Parsing;
using Kysect.DotnetProjectSystem.Traversing;
using Kysect.PowerShellRunner.CodeGeneration.Compilation;
using Kysect.PowerShellRunner.CodeGeneration.SemanticParsing;
using Kysect.PowerShellRunner.CodeGeneration.SolutionReading;
using Kysect.PowerShellRunner.CodeGeneration.SyntaxParsing;
using Kysect.PowerShellRunner.CodeGeneration.UsedModelSearching;
using Microsoft.Extensions.Logging;
using System.IO.Abstractions;

namespace Kysect.PowerShellRunner.CodeGeneration.SchemaGenerating;

public class PowerShellSemanticSchemaGeneratorFactory
{
    private readonly IProgressTrackerFactory _progressTrackerFactory;
    private readonly IFileSystem _fileSystem;
    private readonly ILogger _logger;

    public PowerShellSemanticSchemaGeneratorFactory(IProgressTrackerFactory progressTrackerFactory, IFileSystem fileSystem, ILogger logger)
    {
        _progressTrackerFactory = progressTrackerFactory;
        _fileSystem = fileSystem;
        _logger = logger;
    }

    public PowerShellSemanticSchemaGenerator<TSyntax, TSemantic> Create<TSyntax, TSemantic>(
        IExtendedCmdletSyntaxInfoParser<TSyntax> cmdletBaseInheritorSyntaxParser,
        IExtendedCmdletSemanticInfoParser<TSyntax, TSemantic> cmdletBaseInheritorSemanticParser,
        ISharpCompilationProviderFactory compilationProviderFactory,
        IRoslynSimpleModelBaseTypeFilter simpleModelBaseTypeFilter,
        IRoslynSimpleModelPropertyFilter simpleModelPropertyFilter,
        ISolutionProjectFilter solutionProjectFilter,
        ISolutionSourceFileFilter solutionSourceFileFilter)
        where TSyntax : CmdletBaseSyntaxInfo
        where TSemantic : CmdletBaseSemanticInfo
    {
        var modelSemanticDescriptorFactory = new RoslynSimpleModelSemanticDescriptorFactory(simpleModelBaseTypeFilter, simpleModelPropertyFilter);

        var solutionStructureParser = new DotnetSolutionParser(_fileSystem, _logger);
        var sourceFileFinder = new DotnetSolutionSourceFileFinder(_fileSystem, _logger);
        var sourceFileContentReader = new SolutionSourceFileContentReader(solutionStructureParser, sourceFileFinder, _progressTrackerFactory, solutionProjectFilter, _logger, solutionSourceFileFilter, _fileSystem);

        return new PowerShellSemanticSchemaGenerator<TSyntax, TSemantic>(
            sourceFileContentReader,
            compilationProviderFactory,
            modelSemanticDescriptorFactory,
            cmdletBaseInheritorSyntaxParser,
            cmdletBaseInheritorSemanticParser,
            _progressTrackerFactory,
            _logger);
    }
}