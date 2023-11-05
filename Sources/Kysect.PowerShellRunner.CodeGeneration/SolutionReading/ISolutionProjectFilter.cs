namespace Kysect.PowerShellRunner.CodeGeneration.SolutionReading;

public interface ISolutionProjectFilter
{
    bool Acceptable(string projectPath);
}