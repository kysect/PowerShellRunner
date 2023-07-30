namespace Kysect.PowerShellRunner.Core.Mapping;

public interface IPowerShellObjectCustomMapping
{
    public string SourceType { get; }
    public object Map(object sourceValue);
}