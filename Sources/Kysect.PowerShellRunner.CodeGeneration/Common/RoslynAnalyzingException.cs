namespace Kysect.PowerShellRunner.CodeGeneration.Common;

public class RoslynAnalyzingException : Exception
{
    public RoslynAnalyzingException(string message) : base(message)
    {
    }

    public RoslynAnalyzingException()
    {
    }

    public RoslynAnalyzingException(string message, Exception innerException) : base(message, innerException)
    {
    }
}