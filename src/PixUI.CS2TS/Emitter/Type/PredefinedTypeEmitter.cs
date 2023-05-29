using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    partial class Emitter
    {
        public override void VisitPredefinedType(PredefinedTypeSyntax node)
        {
            //TODO: others
            switch (node.Keyword.Kind())
            {
                case SyntaxKind.StringKeyword:
                    Write("string");
                    break;
                case SyntaxKind.CharKeyword: //注意char转为number
                case SyntaxKind.ByteKeyword:
                case SyntaxKind.SByteKeyword:
                case SyntaxKind.ShortKeyword:
                case SyntaxKind.UShortKeyword:
                case SyntaxKind.IntKeyword:
                case SyntaxKind.UIntKeyword:
                case SyntaxKind.FloatKeyword:
                case SyntaxKind.DoubleKeyword:
                    Write("number");
                    break;
                case SyntaxKind.LongKeyword:
                case SyntaxKind.ULongKeyword:
                    Write("bigint");
                    break;
                case SyntaxKind.BoolKeyword:
                    Write("boolean");
                    break;
                case SyntaxKind.ObjectKeyword:
                    Write("any");
                    break;
                case SyntaxKind.VoidKeyword:
                    Write("void");
                    break;
                default:
                    Write("any");
                    break;
            }
        }
    }
}