namespace Kysect.PowerShellRunner.Tools;

public class PowerShellIntegrationException : Exception
{
    public PowerShellIntegrationException(string message) : base(message)
    {
    }

    public PowerShellIntegrationException(string message, Exception exception) : base(message, exception)
    {

    }

    public PowerShellIntegrationException()
    {
    }
}