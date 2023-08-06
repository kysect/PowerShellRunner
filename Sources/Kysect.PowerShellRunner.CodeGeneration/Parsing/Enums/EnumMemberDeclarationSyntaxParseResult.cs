using System;

namespace Kysect.PowerShellRunner.CodeGeneration.Parsing.Enums;

// TODO: replace int with type from enum. Or use Int64
public class EnumMemberDeclarationSyntaxParseResult : IEquatable<EnumMemberDeclarationSyntaxParseResult>
{
    public string Name { get; }
    public int? Value { get; }

    public EnumMemberDeclarationSyntaxParseResult(string name, int? value)
    {
        Name = name;
        Value = value;
    }

    public bool Equals(EnumMemberDeclarationSyntaxParseResult other)
    {
        if (other is null)
            return false;

        return Name == other.Name
               && Value == other.Value;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as EnumMemberDeclarationSyntaxParseResult);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Value);
    }
}