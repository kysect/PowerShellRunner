using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.PowerShellRunner.CodeGeneration.Common;
using Kysect.PowerShellRunner.CodeGeneration.Compilation;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Kysect.PowerShellRunner.CodeGeneration.SyntaxParsing;

public class CmdletBaseSyntaxInfoParser
{
    // TODO: replace with references to type member
    public const string PsCmdletBaseTypeName = "PSCmdletBase";
    public const string WriteObjectMethodName = "WriteObject";

    private readonly CmdletAttributeSyntaxParser _cmdletAttributeSyntaxParser;
    private readonly ILogger _logger;

    public CmdletBaseSyntaxInfoParser(ILogger logger)
    {
        _logger = logger;
        _cmdletAttributeSyntaxParser = new CmdletAttributeSyntaxParser();
    }

    public bool CanBeParsedAsCmdlet(SolutionCompilationContextItem compilationContextItem)
    {
        compilationContextItem.ThrowIfNull();

        if (!compilationContextItem.Symbol.IsInheritedOf(PsCmdletBaseTypeName))
            return false;

        if (!_cmdletAttributeSyntaxParser.HasCmdletAttributes(compilationContextItem.Syntax))
            return false;

        return true;
    }

    public CmdletBaseSyntaxInfo ExtractSyntaxInfo(SolutionCompilationContextItem compilationContextItem)
    {
        compilationContextItem.ThrowIfNull();

        _logger.LogDebug("Extracting syntax info from {TypeName}", compilationContextItem.Symbol.Name);
        CmdletAttributeSyntax cmdletAttributeSyntax = _cmdletAttributeSyntaxParser.ExtractCmdletAttribute(compilationContextItem.Syntax);
        IReadOnlyCollection<PropertyDeclarationSyntax> properties = GetAllParameterProperties(compilationContextItem);
        IReadOnlyCollection<InvocationExpressionSyntax> writeObjectInvocations = GetAlWriteObjectInvocations(compilationContextItem);

        return new CmdletBaseSyntaxInfo(compilationContextItem, cmdletAttributeSyntax, properties, writeObjectInvocations);
    }

    public IReadOnlyCollection<PropertyDeclarationSyntax> GetAllParameterProperties(SolutionCompilationContextItem compilationContextItem)
    {
        compilationContextItem.ThrowIfNull();

        return compilationContextItem
            .Syntax
            .DescendantNodes()
            .OfType<PropertyDeclarationSyntax>()
            .Where(propertyDeclaration => propertyDeclaration.HasAttribute("Parameter"))
            .ToList();
    }

    public IReadOnlyCollection<InvocationExpressionSyntax> GetAlWriteObjectInvocations(SolutionCompilationContextItem compilationContextItem)
    {
        compilationContextItem.ThrowIfNull();

        return compilationContextItem
            .Syntax
            .GetInvocationExpressionByName(WriteObjectMethodName);
    }
}