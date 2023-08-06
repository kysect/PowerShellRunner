namespace Kysect.PowerShellRunner.CodeGeneration.Parsing.Enums;

// TODO: replace int with type from enum. Or use Int64
public record EnumMemberDeclarationSyntaxParseResult(string Name, int? Value);