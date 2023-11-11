using Kysect.PowerShellRunner.CodeGeneration.SyntaxParsing;

namespace Kysect.PowerShellRunner.CodeGeneration.SemanticParsing;

public interface IExtendedCmdletSemanticInfoParser<in TSyntax, out TSemantic>
    where TSyntax : CmdletBaseSyntaxInfo
    where TSemantic : CmdletBaseSemanticInfo
{
    TSemantic Parse(TSyntax extendedSyntaxInfo, CmdletBaseSemanticInfo baseInfo);
}