using System.Collections.Generic;

namespace Kysect.PowerShellRunner.CodeGeneration.Parsing.Models;

public record ModelDeclarationParseResult(string Name, string? BaseTypeName, IReadOnlyCollection<ModelDeclarationPropertyParseResult> Properties);