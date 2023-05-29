using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    partial class Emitter
    {
        public override void VisitCastExpression(CastExpressionSyntax node)
        {
            //特殊处理整型转换 eg: (int)someNumber
            if (node.Type is PredefinedTypeSyntax predefinedType)
            {
                //排除枚举值
                var symbol = SemanticModel.GetSymbolInfo(node.Expression).Symbol;
                if (symbol == null || symbol.ContainingType.TypeKind != TypeKind.Enum)
                {
                    var kind = predefinedType.Keyword.Kind();
                    switch (kind)
                    {
                        case SyntaxKind.CharKeyword:
                        case SyntaxKind.ByteKeyword:
                        case SyntaxKind.SByteKeyword:
                            CastToInteger(node, "0xFF");
                            return;
                        case SyntaxKind.ShortKeyword:
                        case SyntaxKind.UShortKeyword:
                            CastToInteger(node, "0xFFFF");
                            return;
                        case SyntaxKind.IntKeyword:
                        case SyntaxKind.UIntKeyword:
                            CastToInteger(node, "0xFFFFFFFF");
                            return;
                        case SyntaxKind.LongKeyword:
                        case SyntaxKind.ULongKeyword:
                            CastToBigInt(node);
                            return;
                    }
                }
            }
            
            if (ToJavaScript)
            {
                Visit(node.Expression);
                return;
            }

            Write('<');
            Visit(node.Type);
            // Write('>');
            //TODO:判断是否需要<unknown>
            Write("><unknown>"); //<any>为了消除一些警告
            Visit(node.Expression);
        }

        private void CastToInteger(CastExpressionSyntax node, string mask)
        {
            Write("(Math.floor(");
            Visit(node.Expression);
            Write(") & ");
            Write(mask);
            Write(')');
        }

        private void CastToBigInt(CastExpressionSyntax node)
        {
            Write("BigInt(");
            //如果是数值需要先转换为整数, TODO: decimal需要先转换为字符串
            Visit(node.Expression);
            var type = SemanticModel.GetTypeInfo(node.Expression).Type!;
            if (type.SpecialType == SpecialType.System_Double || type.SpecialType == SpecialType.System_Single)
            {
                Write(".toFixed(0)");
            }
            Write(')');
        }
    }
}