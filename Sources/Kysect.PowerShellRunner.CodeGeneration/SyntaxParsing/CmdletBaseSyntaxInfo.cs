using Kysect.PowerShellRunner.CodeGeneration.Compilation;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace Kysect.PowerShellRunner.CodeGeneration.SyntaxParsing;

public class CmdletBaseSyntaxInfo
{
    public SolutionCompilationContextItem SolutionCompilationContextItem { get; }
    public CmdletBaseInheritorCmdletAttributeSyntax CmdletAttribute { get; }
    public IReadOnlyCollection<PropertyDeclarationSyntax> ParameterProperties { get; }
    public IReadOnlyCollection<InvocationExpressionSyntax> WriteObjectMethodInvocations { get; }

    public CmdletBaseSyntaxInfo(
        SolutionCompilationContextItem solutionCompilationContextItem,
        CmdletBaseInheritorCmdletAttributeSyntax cmdletAttribute,
        IReadOnlyCollection<PropertyDeclarationSyntax> parameterProperties,
        IReadOnlyCollection<InvocationExpressionSyntax> writeObjectMethodInvocations)
    {
        SolutionCompilationContextItem = solutionCompilationContextItem;
        CmdletAttribute = cmdletAttribute;
        ParameterProperties = parameterProperties;
        WriteObjectMethodInvocations = writeObjectMethodInvocations;
    }
}