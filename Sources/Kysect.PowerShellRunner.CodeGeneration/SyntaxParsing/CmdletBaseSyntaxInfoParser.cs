using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.PowerShellRunner.CodeGeneration.Common;
using Kysect.PowerShellRunner.CodeGeneration.Compilation;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Kysect.PowerShellRunner.CodeGeneration.SyntaxParsing;

public class CmdletBaseSyntaxInfoParser
{
    // TODO: replace with references to type member
    public const string PsCmdletBaseTypeName = "PSCmdletBase";
    public const string WriteObjectMethodName = "WriteObject";

    private readonly CmdletBaseInheritorAttributeSyntaxParser _cmdletBaseInheritorAttributeSyntaxParser;

    public CmdletBaseSyntaxInfoParser()
    {
        _cmdletBaseInheritorAttributeSyntaxParser = new CmdletBaseInheritorAttributeSyntaxParser();
    }

    public bool CanBeParsedAsCmdlet(SolutionCompilationContextItem compilationContextItem)
    {
        compilationContextItem.ThrowIfNull();

        if (!compilationContextItem.Symbol.IsInheritedOf(PsCmdletBaseTypeName))
            return false;

        if (!_cmdletBaseInheritorAttributeSyntaxParser.HasCmdletAttributes(compilationContextItem.Syntax))
            return false;

        return true;
    }

    public CmdletBaseSyntaxInfo ExtractSyntaxInfo(SolutionCompilationContextItem compilationContextItem)
    {
        compilationContextItem.ThrowIfNull();

        CmdletBaseInheritorCmdletAttributeSyntax cmdletAttributeSyntax = _cmdletBaseInheritorAttributeSyntaxParser.ExtractCmdletAttribute(compilationContextItem.Syntax);
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