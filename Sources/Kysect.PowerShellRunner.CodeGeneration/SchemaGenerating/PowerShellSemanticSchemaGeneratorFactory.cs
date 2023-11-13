﻿using Kysect.CommonLib.ProgressTracking;
using Kysect.DotnetSlnParser;
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
    private readonly ILogger _logger;

    public PowerShellSemanticSchemaGeneratorFactory(IProgressTrackerFactory progressTrackerFactory, ILogger logger)
    {
        _progressTrackerFactory = progressTrackerFactory;
        _logger = logger;
    }

    public PowerShellSemanticSchemaGenerator<TSyntax, TSemantic> Create<TSyntax, TSemantic>(
        IExtendedCmdletSyntaxInfoParser<TSyntax> cmdletBaseInheritorSyntaxParser,
        IExtendedCmdletSemanticInfoParser<TSyntax, TSemantic> cmdletBaseInheritorSemanticParser,
        IRoslynSimpleModelBaseTypeFilter simpleModelBaseTypeFilter,
        IRoslynSimpleModelPropertyFilter simpleModelPropertyFilter,
        ISolutionProjectFilter solutionProjectFilter,
        ISolutionSourceFileFilter solutionSourceFileFilter)
        where TSyntax : CmdletBaseSyntaxInfo
        where TSemantic : CmdletBaseSemanticInfo
    {
        var modelSemanticDescriptorFactory = new RoslynSimpleModelSemanticDescriptorFactory(simpleModelBaseTypeFilter, simpleModelPropertyFilter);

        var fileSystem = new FileSystem();
        var solutionStructureParser = new DotnetSolutionStructureParser(fileSystem, _logger);
        var sourceFileFinder = new DotnetSolutionSourceFileFinder(fileSystem, _logger);
        var sourceFileContentReader = new SolutionSourceFileContentReader(solutionStructureParser, sourceFileFinder, _progressTrackerFactory, solutionProjectFilter, _logger, solutionSourceFileFilter, fileSystem);

        return new PowerShellSemanticSchemaGenerator<TSyntax, TSemantic>(sourceFileContentReader,
            modelSemanticDescriptorFactory,
            cmdletBaseInheritorSyntaxParser,
            cmdletBaseInheritorSemanticParser,
            _progressTrackerFactory,
            _logger);
    }
}