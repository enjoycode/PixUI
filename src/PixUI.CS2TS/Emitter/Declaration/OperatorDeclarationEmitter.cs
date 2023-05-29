using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    partial class Emitter
    {
        public override void VisitOperatorDeclaration(OperatorDeclarationSyntax node)
        {
            //TODO:判断相同操作符有无重载，有则抛不支持异常

            WriteLeadingTrivia(node);
            WriteModifiers(node.Modifiers);

            // method name
            Write(GetOpName(node.OperatorToken));

            // parameters
            Write('(');
            VisitSeparatedList(node.ParameterList.Parameters);
            Write(')');

            // return type
            // var isReturnVoid = node.IsVoidReturnType();
            // if (!isReturnVoid)
            // {
            Write(": ");
            DisableVisitLeadingTrivia();
            Visit(node.ReturnType);
            EnableVisitLeadingTrivia();
            // }
            // else if (node.Parent is InterfaceDeclarationSyntax || node.HasAbstractModifier())
            // {
            //     emitter.Write(": void");
            // }

            // body
            if (node.Body != null)
            {
                Visit(node.Body);
                return;
            }

            //TypeScript不支持ExpressionBody
            if (node.ExpressionBody != null)
            {
                Write(" { ");
                if ( /*!isReturnVoid &&*/
                    node.ExpressionBody.Expression is not ThrowExpressionSyntax)
                    Write("return ");
                Visit(node.ExpressionBody!.Expression);
                Write("; }");
                VisitTrailingTrivia(node.SemicolonToken);
            }
        }

        private static string GetOpName(SyntaxToken opToken)
        {
            return opToken.Text switch
            {
                "+" => "op_Addition",
                "-" => "op_Subtraction",
                "*" => "op_Multiply",
                "/" => "op_Division",

                "==" => "op_Equality",
                "!=" => "op_Inequality",
                ">" => "op_GreaterThan",
                ">=" => "op_GreaterThanOrEqual",
                "<" => "op_LessThan",
                "<=" => "op_LessThanOrEqual",
                _ => throw new NotSupportedException("Not supported override operator: " +
                                                     opToken.Text)
            };
        }
    }
}