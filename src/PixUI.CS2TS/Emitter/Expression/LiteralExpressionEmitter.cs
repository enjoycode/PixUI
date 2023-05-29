using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    partial class Emitter
    {
        public override void VisitLiteralExpression(LiteralExpressionSyntax node)
        {
            var kind = node.Kind();
            switch (kind)
            {
                case SyntaxKind.NumericLiteralExpression:
                {
                    var number = node.Token.Text;
                    if (!number.StartsWith("0x"))
                    {
                        var lastChar = number[^1];
                        number = lastChar switch
                        {
                            'f' or 'F' or 'd' or 'D' => number[..^1],
                            'l' or 'L' => number[..^1] + "n",
                            _ => number
                        };
                    }

                    Write(number);
                    break;
                }
                case SyntaxKind.DefaultLiteralExpression:
                case SyntaxKind.NullLiteralExpression:
                    Write("null"); //TODO:
                    break;
                case SyntaxKind.CharacterLiteralExpression:
                    int charCode = node.Token.ValueText[0];
                    Write(charCode.ToString()); //直接转换为编码值
                    break;
                case SyntaxKind.StringLiteralExpression:
                {
                    var content = node.Token.ToString();
                    if (content.StartsWith('@'))
                    {
                        Write('`');
                        Write(node.Token.ValueText);
                        Write('`');
                    }
                    else
                    {
                        Write(content);
                    }

                    break;
                }
                default:
                    Write(node.ToString());
                    break;
            }

            VisitTrailingTrivia(node.Token);
        }
    }
}