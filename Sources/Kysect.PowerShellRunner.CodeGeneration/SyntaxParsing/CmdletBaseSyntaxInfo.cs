using Kysect.PowerShellRunner.CodeGeneration.Compilation;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kysect.PowerShellRunner.CodeGeneration.SyntaxParsing;

public class CmdletBaseSyntaxInfo
{
    public SolutionCompilationContextItem SolutionCompilationContextItem { get; }
    public CmdletAttributeSyntax CmdletAttribute { get; }
    public IReadOnlyCollection<PropertyDeclarationSyntax> ParameterProperties { get; }
    public IReadOnlyCollection<InvocationExpressionSyntax> WriteObjectMethodInvocations { get; }

    public CmdletBaseSyntaxInfo(
        SolutionCompilationContextItem solutionCompilationContextItem,
        CmdletAttributeSyntax cmdletAttribute,
        IReadOnlyCollection<PropertyDeclarationSyntax> parameterProperties,
        IReadOnlyCollection<InvocationExpressionSyntax> writeObjectMethodInvocations)
    {
        SolutionCompilationContextItem = solutionCompilationContextItem;
        CmdletAttribute = cmdletAttribute;
        ParameterProperties = parameterProperties;
        WriteObjectMethodInvocations = writeObjectMethodInvocations;
    }
}