using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.PowerShellRunner.CodeGeneration.UsedModelSearching;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kysect.PowerShellRunner.CodeGeneration.SdkGenerating;

public class SimpleModelGenerator
{
    public ClassDeclarationSyntax Generate(SimpleModelDescriptor simpleModelDescriptor)
    {
        simpleModelDescriptor.ThrowIfNull();

        var properties = simpleModelDescriptor
            .PublicProperties
            .Select(p => GeneratePropertyDeclaration(p))
            .ToList();

        ClassDeclarationSyntax classDeclarationSyntax = SyntaxFactory.ClassDeclaration(simpleModelDescriptor.Name)
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

        if (simpleModelDescriptor.BaseType is not null)
            classDeclarationSyntax = classDeclarationSyntax.AddBaseListTypes(SyntaxFactory.SimpleBaseType(SyntaxFactory.IdentifierName(simpleModelDescriptor.BaseType)));

        return classDeclarationSyntax
            .AddMembers(properties.ToArray());
    }

    public PropertyDeclarationSyntax GeneratePropertyDeclaration(SimpleModelPropertyDescriptor propertyDescriptor)
    {
        propertyDescriptor.ThrowIfNull();

        return SyntaxFactory.PropertyDeclaration(
                SyntaxFactory.IdentifierName(propertyDescriptor.Type),
                SyntaxFactory.Identifier(propertyDescriptor.Name))
            .AddModifiers(
                SyntaxFactory.Token(SyntaxKind.PublicKeyword))
            .WithGetSetAccessorList();
    }
}