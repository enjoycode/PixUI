using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI;

internal static class TypeHelper
{
    private const string Namespace = "PixUI";

    /// <summary>
    /// Checks whether the Node is annotated with attribute and maps syntax context
    /// to the specific node type (ClassDeclarationSyntax).
    /// </summary>
    /// <param name="context">Syntax context, based on CreateSyntaxProvider predicate</param>
    /// <returns>The specific cast and whether the attribute was found.</returns>
    internal static SourceGenClassInfo GetClassDeclarationForSourceGen(GeneratorSyntaxContext context)
    {
        var classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;

        // Go through all attributes of the class.
        var genClassType = SourceGenClassType.None;
        string? styleType = null;
        string? widgetType = null;
        foreach (var attributeListSyntax in classDeclarationSyntax.AttributeLists)
        foreach (var attributeSyntax in attributeListSyntax.Attributes)
        {
            if (ModelExtensions.GetSymbolInfo(context.SemanticModel, attributeSyntax).Symbol is not IMethodSymbol
                attributeSymbol)
                continue; // if we can't get the symbol, ignore it

            var attributeName = attributeSymbol.ContainingType.ToDisplayString();

            // Check the full name of the attribute.

            if (attributeName == $"{Namespace}.{nameof(SourceGenAttributes.BindableWidgetAttribute)}")
            {
                genClassType |= SourceGenClassType.BindableWidget;
            }
            else if (attributeName.StartsWith($"{Namespace}.{nameof(SourceGenAttributes.StylableWidgetAttribute)}<"))
            {
                styleType = GetGenericType(attributeSymbol);
                genClassType |= SourceGenClassType.StylableWidget;
            }
            else if (attributeName.StartsWith($"{Namespace}.{nameof(SourceGenAttributes.StyleObjectAttribute)}<"))
            {
                widgetType = GetGenericType(attributeSymbol);
                genClassType = SourceGenClassType.StyleObject;
            }
            else if (attributeName == $"{Namespace}.{nameof(SourceGenAttributes.BaseStyleObjectAttribute)}")
            {
                GetBaseStyleObjectTypes(attributeSyntax, out widgetType, out styleType);
                genClassType = SourceGenClassType.BaseStyleObject;
            }
        }

        return new(classDeclarationSyntax, genClassType, widgetType, styleType);
    }

    private static string GetGenericType(IMethodSymbol attributeSymbol)
    {
        var containingType = attributeSymbol.ContainingType;
        return containingType is { TypeArguments.Length: > 0 }
            ? containingType.TypeArguments[0].Name
            : throw new Exception("Not a GenericType");
    }

    private static void GetBaseStyleObjectTypes(AttributeSyntax attribute,
        out string? widgetType, out string? styleType)
    {
        string? w = null;
        string? s = null;
        if (attribute.ArgumentList is not null)
        {
            for (var i = 0; i < attribute.ArgumentList.Arguments.Count; i++)
            {
                var argument = attribute.ArgumentList.Arguments[i];
                if (i == 0) w = GetLiteralStringValue(argument.Expression);
                if (i == 1) s = GetLiteralStringValue(argument.Expression);
            }
        }

        widgetType = w;
        styleType = s;
    }

    internal static IEnumerable<BindablePropertyInfo> GetBindableProperties(
        ClassDeclarationSyntax classDeclaration, SemanticModel semanticModel)
    {
        foreach (var property in classDeclaration.Members.OfType<PropertyDeclarationSyntax>())
        {
            foreach (var attributeList in property.AttributeLists)
            foreach (var attribute in attributeList.Attributes)
            {
                if (!IsBindablePropertyAttribute(attribute, semanticModel))
                    continue;
                if (ModelExtensions.GetDeclaredSymbol(semanticModel, property) is not IPropertySymbol propertySymbol)
                    continue;

                string? defaultValue = null;
                string? validator = null;
                string? listener = null;
                var modifiers = GetModifiersOfProperty(property);

                if (attribute.ArgumentList is not null)
                {
                    for (var i = 0; i < attribute.ArgumentList.Arguments.Count; i++)
                    {
                        var argument = attribute.ArgumentList.Arguments[i];
                        var name = argument.NameColon?.Name.ToString();
                        if (!string.IsNullOrEmpty(name))
                        {
                            if (name == "defaultValue")
                                defaultValue = GetLiteralStringValue(argument.Expression);
                            if (name == "validator")
                                validator = GetLiteralStringValue(argument.Expression);
                            if (name == "listener")
                                listener = GetLiteralStringValue(argument.Expression);
                        }
                        else
                        {
                            if (i == 0)
                                defaultValue = GetLiteralStringValue(argument.Expression);
                            if (i == 1)
                                validator = GetLiteralStringValue(argument.Expression);
                            if (i == 2)
                                listener = GetLiteralStringValue(argument.Expression);
                        }
                    }
                }

                yield return new BindablePropertyInfo(propertySymbol, modifiers,
                    defaultValue, validator, listener);
            }
        }
    }

    private static string GetModifiersOfProperty(PropertyDeclarationSyntax propertyDeclaration)
    {
        SyntaxKind[] accessModifiers =
        [
            SyntaxKind.PublicKeyword,
            SyntaxKind.PrivateKeyword,
            SyntaxKind.ProtectedKeyword,
            SyntaxKind.InternalKeyword
        ];
        var modifiers = propertyDeclaration.Modifiers
            .Where(t => accessModifiers.Contains(t.Kind()))
            .Select(t => t.Text);

        return string.Join(" ", modifiers);
    }

    /// <summary>
    /// 是否标为[BindableProperty]
    /// </summary>
    private static bool IsBindablePropertyAttribute(AttributeSyntax attribute, SemanticModel semanticModel)
    {
        if (ModelExtensions.GetSymbolInfo(semanticModel, attribute).Symbol is not IMethodSymbol attributeSymbol)
            return false;

        var attributeName = attributeSymbol.ContainingType.ToDisplayString();
        return attributeName == $"{Namespace}.{nameof(SourceGenAttributes.BindablePropertyAttribute)}";
    }

    private static string? GetLiteralStringValue(ExpressionSyntax expression) =>
        expression is LiteralExpressionSyntax { Token.Value: string value } ? value : null;
}

internal readonly struct SourceGenClassInfo
{
    public SourceGenClassInfo(ClassDeclarationSyntax classDeclaration,
        SourceGenClassType genType, string? widgetType = null, string? styleType = null)
    {
        ClassDeclaration = classDeclaration;
        GenType = genType;
        WidgetType = widgetType;
        StyleType = styleType;
    }

    public ClassDeclarationSyntax ClassDeclaration { get; }
    public SourceGenClassType GenType { get; }
    public string? WidgetType { get; }
    public string? StyleType { get; }
}

internal readonly struct BindablePropertyInfo
{
    public BindablePropertyInfo(IPropertySymbol propertySymbol, string modifiers,
        string? defaultValue = null, string? validator = null, string? listener = null)
    {
        PropertySymbol = propertySymbol;
        Modifiers = modifiers;
        DefaultValue = defaultValue ?? "default";
        Validator = validator;
        Listener = string.IsNullOrEmpty(listener) ? "Repaint" : listener!;
    }

    public IPropertySymbol PropertySymbol { get; }
    public string Modifiers { get; }
    public string DefaultValue { get; }
    public string? Validator { get; }
    public string Listener { get; }
}