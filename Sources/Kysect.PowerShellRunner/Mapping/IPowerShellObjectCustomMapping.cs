namespace Kysect.PowerShellRunner.Mapping;

public interface IPowerShellObjectCustomMapping
{
    public string SourceType { get; }
    public object Map(object sourceValue);
}