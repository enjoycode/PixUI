using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    partial class Emitter
    {
        public override void VisitArrayType(ArrayTypeSyntax node)
        {
            //需要排除 SomeMethod(params int[] args)特例
            if (!(node.Parent is ParameterSyntax parameterSyntax &&
                  parameterSyntax.Modifiers.Any(m => m.Kind() == SyntaxKind.ParamsKeyword)))
            {
                var jsArrayType = GetJsNativeArrayType(node);
                if (jsArrayType != null)
                {
                    Write(jsArrayType);
                    return;
                }
            }

            Visit(node.ElementType);
            Write("[]");
        }

        private static string? GetJsNativeArrayType(ArrayTypeSyntax node)
        {
            if (node.ElementType is not PredefinedTypeSyntax predefinedType)
                return null;

            var jsArrayType = predefinedType.Keyword.Kind() switch
            {
                SyntaxKind.ByteKeyword => "Uint8Array",
                SyntaxKind.SByteKeyword => "Int8Array",
                SyntaxKind.ShortKeyword => "Int16Array",
                SyntaxKind.UShortKeyword => "Uint16Array",
                SyntaxKind.CharKeyword => "Uint16Array",
                SyntaxKind.IntKeyword => "Int32Array",
                SyntaxKind.UIntKeyword => "Uint32Array",
                SyntaxKind.FloatKeyword => "Float32Array",
                SyntaxKind.DoubleKeyword => "Float64Array",
                _ => null
            };
            return jsArrayType;
        }

        internal static string? GetJsNativeArrayType(ITypeSymbol typeSymbol)
        {
            if (typeSymbol is not IArrayTypeSymbol arrayTypeSymbol) return null;

            return GetJsNativeArrayTypeByElementType(arrayTypeSymbol.ElementType);
        }

        internal static string? GetJsNativeArrayTypeByElementType(ITypeSymbol elementType)
        {
            var jsArrayType = elementType.SpecialType switch
            {
                SpecialType.System_Byte => "Uint8Array",
                SpecialType.System_SByte => "Int8Array",
                SpecialType.System_Int16 => "Int16Array",
                SpecialType.System_UInt16 => "Uint16Array",
                SpecialType.System_Int32 => "Int32Array",
                SpecialType.System_UInt32 => "Uint32Array",
                SpecialType.System_Single => "Float32Array",
                SpecialType.System_Double => "Float64Array",
                _ => null
            };
            return jsArrayType;
        }
    }
}