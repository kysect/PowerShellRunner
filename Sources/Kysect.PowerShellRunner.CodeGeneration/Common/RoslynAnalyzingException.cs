using System;

namespace Kysect.RoslynUtils.Common;

public class RoslynAnalyzingException : Exception
{
    public RoslynAnalyzingException(string message) : base(message)
    {
    }
}