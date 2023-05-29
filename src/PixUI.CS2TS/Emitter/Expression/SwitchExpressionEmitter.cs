using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    partial class Emitter
    {
        public override void VisitSwitchExpression(SwitchExpressionSyntax node)
        {
            //emitter.WriteLeadingTrivia(node.GoverningExpression);

            Write("match(");
            Visit(node.GoverningExpression);
            Write(")\n");

            foreach (var arm in node.Arms)
            {
                if (arm.WhenClause != null)
                    throw new NotImplementedException("SwitchExpression with when");

                WriteLeadingWhitespaceOnly(arm);
                if (arm.Pattern is DiscardPatternSyntax)
                {
                    Write(".otherwise(() => ");
                    if (arm.Expression is ThrowExpressionSyntax)
                        Write('{');
                    //DisableVisitTrailingTrivia();
                    Visit(arm.Expression);
                    //EnableVisitTrailingTrivia();
                    if (arm.Expression is ThrowExpressionSyntax)
                        Write('}');
                    Write(")");
                }
                else if (arm.Pattern is ConstantPatternSyntax constant)
                {
                    Write(".with(");
                    Visit(constant.Expression);
                    Write(", () => ");
                    Visit(arm.Expression);
                    Write(")\n");
                }
                else
                {
                    throw new NotImplementedException(
                        $"SwitchExpression with {arm.Pattern.GetType()} at File:{node.SyntaxTree.FilePath}");
                }
            }
        }
    }
}