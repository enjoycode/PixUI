using System;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    partial class Emitter
    {
        public override void VisitIfStatement(IfStatementSyntax node)
        {
            // //先判断是否有Pattern
            // var hasPattern = node.DescendantNodes()
            //     .Any(t => t is IsPatternExpressionSyntax);
            // if (hasPattern)
            // {
            //     EmitToPattern(emitter, node);
            //     return;
            // }

            VisitToken(node.IfKeyword);
            VisitToken(node.OpenParenToken);
            Visit(node.Condition);
            VisitToken(node.CloseParenToken);

            if (InjectIsPatternForIfStatement != null)
            {
                var inject = GetInject(); //eg: const dog = obj;

                if (node.Statement is BlockSyntax block)
                {
                    new BlockEmitter(inject).Emit(this, block);
                }
                else
                {
                    WriteLeadingWhitespaceOnly(node);
                    Write("{\n");
                    WriteLeadingWhitespaceOnly(node);
                    Write('\t');
                    Write(inject);
                    Write('\n');
                    Visit(node.Statement);
                    WriteLeadingWhitespaceOnly(node);
                    Write("}\n");

                    InjectIsPatternForIfStatement = null; //must reset it
                }
            }
            else
            {
                Visit(node.Statement);
            }

            Visit(node.Else);
        }

        private string GetInject()
        {
            UseTempOutput();

            var declaration = (DeclarationPatternSyntax)InjectIsPatternForIfStatement!.Pattern;
            Write("const ");
            Visit(declaration.Designation);
            Write(" = ");
            Visit(InjectIsPatternForIfStatement!.Expression);
            Write(';');

            return GetTempOutput();
        }
    }
}