using System;

namespace Kysect.PowerShellRunner.CodeGeneration.Common;

public class RoslynAnalyzingException : Exception
{
    public RoslynAnalyzingException(string message) : base(message)
    {
    }
}