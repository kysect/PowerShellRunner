using Kysect.CommonLib.BaseTypes.Extensions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;

namespace Kysect.PowerShellRunner.CodeGeneration.Parsing.Models;

public class ModelDeclarationParser
{
    private readonly ILogger _logger;
    // TODO: use it for resolving property type. Need to resolve namespaces and return this info
    // and use it for adding necessary using to generated model.
#pragma warning disable IDE0052 // Remove unread private members
    private readonly CSharpCompilation _compilation;
#pragma warning restore IDE0052 // Remove unread private members

    public ModelDeclarationParser(ILogger logger, CSharpCompilation compilation)
    {
        _logger = logger;
        _compilation = compilation;
    }

    public ModelDeclarationParseResult Parse(ClassDeclarationSyntax syntax)
    {
        syntax.ThrowIfNull();

        // TODO: support case when class is member of other class (inner)
        string modelName = syntax.Identifier.Text;

        _logger.LogInformation("Parse mode {modelName}", modelName);

        var properties = syntax
            .Members
            .OfType<PropertyDeclarationSyntax>()
            .Select(ParseProperty)
            .ToList();

        // TODO: implement base type parsing
        return new ModelDeclarationParseResult(modelName, null, properties);
    }

    private ModelDeclarationPropertyParseResult ParseProperty(PropertyDeclarationSyntax property)
    {
        string propertyName = property.Identifier.Text;
        TypeSyntax propertyType = property.Type;

        // TODO: rework
        return new ModelDeclarationPropertyParseResult(propertyName, propertyType.ToString());
    }
}