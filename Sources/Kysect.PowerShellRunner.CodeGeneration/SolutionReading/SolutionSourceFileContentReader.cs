using Kysect.CommonLib.ProgressTracking;
using Kysect.DotnetProjectSystem.Parsing;
using Kysect.DotnetProjectSystem.Traversing;
using Microsoft.Extensions.Logging;
using System.IO.Abstractions;

namespace Kysect.PowerShellRunner.CodeGeneration.SolutionReading;

public class SolutionSourceFileContentReader : ISolutionSourceFileContentReader
{
    private readonly DotnetSolutionParser _solutionStructureParser;
    private readonly DotnetSolutionSourceFileFinder _sourceFileFinder;
    private readonly IProgressTrackerFactory _progressTrackerFactory;
    private readonly ISolutionProjectFilter _projectFilter;
    private readonly ISolutionSourceFileFilter _sourceFileFilter;
    private readonly IFileSystem _fileSystem;
    private readonly ILogger _logger;

    public SolutionSourceFileContentReader(
        DotnetSolutionParser solutionStructureParser,
        DotnetSolutionSourceFileFinder sourceFileFinder,
        IProgressTrackerFactory progressTrackerFactory,
        ISolutionProjectFilter projectFilter,
        ILogger logger,
        ISolutionSourceFileFilter sourceFileFilter, IFileSystem fileSystem)
    {
        _solutionStructureParser = solutionStructureParser;
        _sourceFileFinder = sourceFileFinder;
        _progressTrackerFactory = progressTrackerFactory;
        _projectFilter = projectFilter;
        _logger = logger;
        _sourceFileFilter = sourceFileFilter;
        _fileSystem = fileSystem;
    }

    public IReadOnlyCollection<string> ReadFileContents(string solutionPath)
    {
        _logger.LogInformation("Reading files content from solution {solutionFilePath}", solutionPath);

        DotnetSolutionDescriptor dotnetSolutionDescriptor = _solutionStructureParser.Parse(solutionPath);
        DotnetSolutionPaths dotnetSolutionPaths = _sourceFileFinder.FindSourceFiles(dotnetSolutionDescriptor);

        _logger.LogInformation("Filtering acceptable source code files");

        var filePaths = dotnetSolutionPaths
            .ProjectPaths
            .Where(projectPaths => _projectFilter.Acceptable(projectPaths.ProjectFileFullPath))
            .SelectMany(projectPaths => projectPaths.SourceFileFullPaths)
            .Where(filePath => _sourceFileFilter.Acceptable(filePath))
            .ToList();

        _logger.LogInformation("Found {Count} files. Start reading", filePaths.Count);

        CollectionProgressTracker<string> readFiles = new CollectionProgressTracker<string>(_progressTrackerFactory, filePaths)
            .SelectParallel("Reading source code from files", _fileSystem.File.ReadAllText);

        return readFiles.Values;
    }
}