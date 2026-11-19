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
    internal static SourceGenClassInfo GetClassDeclarationForSourceGen(
        GeneratorSyntaxContext context)
    {
        var classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;

        // Go through all attributes of the class.
        foreach (var attributeListSyntax in classDeclarationSyntax.AttributeLists)
        foreach (var attributeSyntax in attributeListSyntax.Attributes)
        {
            if (ModelExtensions.GetSymbolInfo(context.SemanticModel, attributeSyntax).Symbol is not IMethodSymbol
                attributeSymbol)
                continue; // if we can't get the symbol, ignore it

            var attributeName = attributeSymbol.ContainingType.ToDisplayString();

            // Check the full name of the attribute.
            if (attributeName == $"{Namespace}.{nameof(Attributes.StyleObjectAttribute)}")
                return new(classDeclarationSyntax, SourceGenClassType.StyleObject);
            if (attributeName.StartsWith($"{Namespace}.{nameof(Attributes.StylableWidgetAttribute)}<"))
                return new(classDeclarationSyntax, SourceGenClassType.StylableWidget, GetStyleType(attributeSymbol));
        }

        return new(classDeclarationSyntax, SourceGenClassType.None);
    }

    internal static string GetStyleType(IMethodSymbol attributeSymbol)
    {
        var containingType = attributeSymbol.ContainingType;
        return containingType is { TypeArguments.Length: > 0 }
            ? containingType.TypeArguments[0].Name
            : throw new Exception("Not a StylableWidgetAttribute");
    }

    /// <summary>
    /// 收集 classDeclaration 下所有标记了 StylePropertyAttribute 的属性，并解析其命名参数，
    /// 转换为 StylePropertyInfo。
    /// </summary>
    internal static IEnumerable<StylePropertyInfo> GetStyleProperties(ClassDeclarationSyntax classDeclaration,
        SemanticModel semanticModel)
    {
        foreach (var property in classDeclaration.Members.OfType<PropertyDeclarationSyntax>())
        {
            foreach (var attributeList in property.AttributeLists)
            foreach (var attribute in attributeList.Attributes)
            {
                if (!IsStylePropertyAttribute(attribute, semanticModel))
                    continue;
                if (ModelExtensions.GetDeclaredSymbol(semanticModel, property) is not IPropertySymbol propertySymbol)
                    continue;

                string? defaultValue = null;
                var defaultValueProxy = false;
                string? validator = null;
                string? listener = null;
                var modifiers = GetModifiersOfProperty(property);

                if (attribute.ArgumentList is not null)
                {
                    foreach (var argument in attribute.ArgumentList.Arguments)
                    {
                        var name = argument.NameColon?.Name.ToString();
                        if (name == "defaultValue")
                            defaultValue = GetLiteralStringValue(argument.Expression);
                        else if (name == "defaultValueProxy")
                            defaultValueProxy = GetLiteralBoolValue(argument.Expression);
                        else if (name == "validator")
                            validator = GetLiteralStringValue(argument.Expression);
                        else if (name == "listener")
                            listener = GetLiteralStringValue(argument.Expression);
                    }
                }

                yield return new StylePropertyInfo(propertySymbol, modifiers,
                    defaultValue, defaultValueProxy, validator, listener);
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

    private static bool IsStylePropertyAttribute(AttributeSyntax attribute, SemanticModel semanticModel)
    {
        if (ModelExtensions.GetSymbolInfo(semanticModel, attribute).Symbol is not IMethodSymbol attributeSymbol)
            return false;

        return attributeSymbol.ContainingType.ToDisplayString() ==
               $"{Namespace}.{nameof(Attributes.StylePropertyAttribute)}";
    }

    private static string? GetLiteralStringValue(ExpressionSyntax expression)
    {
        if (expression is LiteralExpressionSyntax literal && literal.Token.Value is string value)
            return value;

        return null;
    }

    private static bool GetLiteralBoolValue(ExpressionSyntax expression)
    {
        if (expression is LiteralExpressionSyntax literal && literal.Token.Value is bool value)
            return value;

        return false;
    }
}

internal readonly struct SourceGenClassInfo
{
    public SourceGenClassInfo(ClassDeclarationSyntax classDeclaration,
        SourceGenClassType genType, string? styleType = null)
    {
        ClassDeclaration = classDeclaration;
        GenType = genType;
        StyleType = styleType;
    }

    public ClassDeclarationSyntax ClassDeclaration { get; }
    public SourceGenClassType GenType { get; }
    public string? StyleType { get; }
}

internal readonly struct StylePropertyInfo
{
    public StylePropertyInfo(IPropertySymbol propertySymbol, string modifiers,
        string? defaultValue = null, bool defaultValueProxy = false,
        string? validator = null, string? listener = null)
    {
        PropertySymbol = propertySymbol;
        Modifiers = modifiers;
        DefaultValue = defaultValue ?? "default";
        DefaultValueProxy = defaultValueProxy;
        Validator = validator;
        Listener = string.IsNullOrEmpty(listener) ? "Repaint" : listener!;
    }

    public IPropertySymbol PropertySymbol { get; }
    public string Modifiers { get; }
    public string DefaultValue { get; }
    public bool DefaultValueProxy { get; }
    public string? Validator { get; }
    public string Listener { get; }
}