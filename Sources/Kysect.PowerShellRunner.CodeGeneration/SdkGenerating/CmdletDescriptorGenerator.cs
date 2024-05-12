using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.PowerShellRunner.Abstractions.Cmdlets;
using Kysect.PowerShellRunner.Abstractions.Parameters;
using Kysect.PowerShellRunner.CodeGeneration.SchemaGenerating;
using Kysect.PowerShellRunner.CodeGeneration.UsedModelSearching;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kysect.PowerShellRunner.CodeGeneration.SdkGenerating;

public class CmdletDescriptorGenerator
{
    private const string CmdletInterfaceName = nameof(IPowerShellCmdlet);
    private const string CmdletInterfacePropertyName = nameof(IPowerShellCmdlet.CmdletName);
    // KB: type does not affect, nameof return only type name
    private const string PowerShellParameterInterfaceName = nameof(IPowerShellCmdletParameter<object>);

    public ClassDeclarationSyntax Generate(PowerShellCmdletDescriptor cmdletBaseInheritor)
    {
        cmdletBaseInheritor.ThrowIfNull();

        SimpleBaseTypeSyntax interfaceImplementation = PrepareInterfaceImplementation(cmdletBaseInheritor);
        PropertyDeclarationSyntax propertyWithCmdletAlias = PreparePropertyWithCmdletAlias(cmdletBaseInheritor);
        MemberDeclarationSyntax[] parameterProperties = cmdletBaseInheritor.PropertyDeclarations
            .Select(PrepareParameterProperty)
            .Cast<MemberDeclarationSyntax>()
            .ToArray();

        ClassDeclarationSyntax classDeclaration = SyntaxFactory.ClassDeclaration(cmdletBaseInheritor.CmdletAttributeValues.GetClassName())
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
            .AddBaseListTypes(interfaceImplementation)
            .AddMembers(propertyWithCmdletAlias)
            .AddMembers(parameterProperties);

        return classDeclaration;
    }

    private static SimpleBaseTypeSyntax PrepareInterfaceImplementation(PowerShellCmdletDescriptor cmdletBaseInheritor)
    {
        if (cmdletBaseInheritor.MainReturnType is not null)
        {
            string? returnType = cmdletBaseInheritor.MainReturnType;

            return SyntaxFactory.SimpleBaseType(
                SyntaxFactory.GenericName(SyntaxFactory.Identifier(CmdletInterfaceName))
                    .AddTypeArgumentListArguments(SyntaxFactory.IdentifierName(returnType)));
        }
        else
        {
            return SyntaxFactory.SimpleBaseType(SyntaxFactory.IdentifierName(CmdletInterfaceName));
        }
    }

    private static PropertyDeclarationSyntax PreparePropertyWithCmdletAlias(PowerShellCmdletDescriptor cmdletBaseInheritor)
    {
        string powerShellAlias = cmdletBaseInheritor.CmdletAttributeValues.GetPowerShellAlias();

        return SyntaxFactory.PropertyDeclaration(SyntaxFactory.IdentifierName("String"), SyntaxFactory.Identifier(CmdletInterfacePropertyName))
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
            .WithExpressionBody(
                SyntaxFactory.ArrowExpressionClause(
                    SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(powerShellAlias))))
            .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
    }

    private static PropertyDeclarationSyntax PrepareParameterProperty(SimpleModelPropertyDescriptor parameter)
    {
        return SyntaxFactory.PropertyDeclaration(
                SyntaxFactory.GenericName(SyntaxFactory.Identifier(PowerShellParameterInterfaceName)).AddTypeArgumentListArguments(SyntaxFactory.IdentifierName(parameter.Type)),
                SyntaxFactory.Identifier(parameter.Name))
            .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
            .WithGetSetAccessorList();
    }
}