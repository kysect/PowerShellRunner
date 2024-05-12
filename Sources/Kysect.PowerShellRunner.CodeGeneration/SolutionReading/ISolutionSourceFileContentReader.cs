namespace Kysect.PowerShellRunner.CodeGeneration.SolutionReading;

public interface ISolutionSourceFileContentReader
{
    IReadOnlyCollection<string> ReadFileContents(string solutionPath);
}