namespace Kysect.PowerShellRunner.CodeGeneration.SolutionReading;

public interface ISolutionSourceFileFilter
{
    bool Acceptable(string filePath);
}