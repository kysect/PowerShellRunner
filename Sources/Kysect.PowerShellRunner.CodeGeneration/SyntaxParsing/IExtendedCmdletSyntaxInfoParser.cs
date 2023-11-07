namespace Kysect.PowerShellRunner.CodeGeneration.SyntaxParsing;

public interface IExtendedCmdletSyntaxInfoParser<T> where T : CmdletBaseSyntaxInfo
{
    bool Acceptable(CmdletBaseSyntaxInfo baseInfo);
    T Parse(CmdletBaseSyntaxInfo baseInfo);
}