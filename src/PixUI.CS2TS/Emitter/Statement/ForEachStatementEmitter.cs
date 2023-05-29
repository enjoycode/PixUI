using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    partial class Emitter
    {
        public override void VisitForEachStatement(ForEachStatementSyntax node)
        {
            WriteLeadingTrivia(node);

            Write("for (const ");
            Write(node.Identifier.Text);
            Write(" of ");
            Visit(node.Expression);
            VisitToken(node.CloseParenToken);

            Visit(node.Statement);
        }
    }
}