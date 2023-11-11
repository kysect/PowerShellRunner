namespace Kysect.PowerShellRunner.CodeGeneration.SemanticParsing;

public class CmdletAttributeValues
{
    public string Verb { get; }
    public string Noun { get; }

    public CmdletAttributeValues(string verb, string noun)
    {
        Verb = verb;
        Noun = noun;
    }

    public string GetClassName()
    {
        return $"{Verb}{Noun}";
    }

    public string GetPowerShellAlias()
    {
        return $"{Verb}-{Noun}";
    }

    public override string ToString()
    {
        return $"{Verb}-{Noun}";
    }
}