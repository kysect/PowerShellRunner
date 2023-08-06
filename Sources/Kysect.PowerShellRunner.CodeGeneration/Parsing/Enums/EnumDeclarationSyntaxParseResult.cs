using System.Collections.Generic;

namespace Kysect.PowerShellRunner.CodeGeneration.Parsing.Enums;

public class EnumDeclarationSyntaxParseResult
{
    // TODO: add enum underling type info
    public string Name { get; }
    public IReadOnlyList<EnumMemberDeclarationSyntaxParseResult> Members { get; }

    public EnumDeclarationSyntaxParseResult(string name, IReadOnlyList<EnumMemberDeclarationSyntaxParseResult> members)
    {
        Name = name;
        Members = members;
    }
}