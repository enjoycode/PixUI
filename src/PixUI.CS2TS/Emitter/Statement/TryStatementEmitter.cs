using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    partial class Emitter
    {
        public override void VisitTryStatement(TryStatementSyntax node)
        {
            if (node.Catches.Count > 1)
                throw new NotSupportedException("Try statement can only has one catch cause");
            if (node.Catches.Count == 1)
            {
                var catchClauseType = node.Catches[0].Declaration!.Type.ToString();
                if (catchClauseType != "Exception" && catchClauseType != "System.Exception")
                    throw new NotSupportedException("CatchClause only accept System.Exception");
            }

            VisitToken(node.TryKeyword);
            Visit(node.Block);
            if (node.Catches.Count == 1)
            {
                var catchClause = node.Catches[0];
                var catchDeclaration = catchClause.Declaration!;
                VisitToken(catchClause.CatchKeyword);
                VisitToken(catchDeclaration.OpenParenToken);
                Write(catchDeclaration.Identifier.Text);
                if (!ToJavaScript)
                    Write(": any");
                VisitToken(catchDeclaration.CloseParenToken);

                Visit(catchClause.Block);
            }

            Visit(node.Finally);
        }
    }
}