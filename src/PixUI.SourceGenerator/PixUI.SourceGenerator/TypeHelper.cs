using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI;

internal static class TypeHelper
{
    private const string Namespace = "PixUI";

    /// <summary>
    /// Checks whether the Node is annotated with attribute and maps syntax context to the specific node type (ClassDeclarationSyntax).
    /// </summary>
    /// <param name="context">Syntax context, based on CreateSyntaxProvider predicate</param>
    /// <returns>The specific cast and whether the attribute was found.</returns>
    internal static (ClassDeclarationSyntax, SourceGenClassType classType) GetClassDeclarationForSourceGen(
        GeneratorSyntaxContext context)
    {
        var classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;

        // Go through all attributes of the class.
        foreach (var attributeListSyntax in classDeclarationSyntax.AttributeLists)
        foreach (var attributeSyntax in attributeListSyntax.Attributes)
        {
            if (context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol is not IMethodSymbol attributeSymbol)
                continue; // if we can't get the symbol, ignore it

            var attributeName = attributeSymbol.ContainingType.ToDisplayString();

            // Check the full name of the attribute.
            if (attributeName == $"{Namespace}.{nameof(Attributes.StyleObjectAttribute)}")
                return (classDeclarationSyntax, SourceGenClassType.StyleObject);
            if (attributeName == $"{Namespace}.{nameof(Attributes.StylableAttribute)}")
                return (classDeclarationSyntax, SourceGenClassType.StylableWidget);
        }

        return (classDeclarationSyntax, SourceGenClassType.None);
    }

    /// <summary>
    /// 收集 classDeclaration 下所有标记了 StylePropertyAttribute 的属性，并解析其命名参数，
    /// 转换为 StylePropertyInfo。仅依赖语法节点（无 SemanticModel）。
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
                if (semanticModel.GetDeclaredSymbol(property) is not IPropertySymbol propertySymbol) 
                    continue;

                string? defaultValue = null;
                var defaultValueProxy = false;
                string? validator = null;
                string? listener = null;

                if (attribute.ArgumentList is not null)
                {
                    foreach (var argument in attribute.ArgumentList.Arguments)
                    {
                        var name = argument.NameColon?.Name.ToString();
                        if (name == "defaultValue")
                            defaultValue = GetStringValue(argument.Expression);
                        else if (name == "defaultValueProxy")
                            defaultValueProxy = GetBoolValue(argument.Expression);
                        else if (name == "validator")
                            validator = GetStringValue(argument.Expression);
                        else if (name == "listener")
                            listener = GetStringValue(argument.Expression);
                    }
                }

                yield return new StylePropertyInfo(propertySymbol, defaultValue, defaultValueProxy,
                    validator, listener);
            }
        }
    }

    private static bool IsStylePropertyAttribute(AttributeSyntax attribute, SemanticModel semanticModel)
    {
        if (semanticModel.GetSymbolInfo(attribute).Symbol is not IMethodSymbol attributeSymbol)
            return false;

        return attributeSymbol.ContainingType.ToDisplayString() ==
               $"{Namespace}.{nameof(Attributes.StylePropertyAttribute)}";
    }

    private static string? GetStringValue(ExpressionSyntax expression)
    {
        if (expression is LiteralExpressionSyntax literal && literal.Token.Value is string value)
            return value;

        return null;
    }

    private static bool GetBoolValue(ExpressionSyntax expression)
    {
        if (expression is LiteralExpressionSyntax literal && literal.Token.Value is bool value)
            return value;

        return false;
    }
}

internal readonly struct StylePropertyInfo
{
    public StylePropertyInfo(IPropertySymbol propertySymbol, string? defaultValue = null,
        bool defaultValueProxy = false, string? validator = null, string? listener = null)
    {
        PropertySymbol = propertySymbol;
        DefaultValue = defaultValue ?? "default";
        DefaultValueProxy = defaultValueProxy;
        Validator = validator;
        Listener = listener switch
        {
            "Relayout" => "Widget.RelayoutOnStyleChanged",
            null or "Repaint" => "Widget.RepaintOnStyleChanged",
            _ => listener
        };
    }

    public IPropertySymbol PropertySymbol { get; }
    public string DefaultValue { get; }
    public bool DefaultValueProxy { get; }
    public string? Validator { get; }
    public string Listener { get; }
}