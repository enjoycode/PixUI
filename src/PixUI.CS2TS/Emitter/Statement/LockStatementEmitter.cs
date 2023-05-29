using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    partial class Emitter
    {
        public override void VisitLockStatement(LockStatementSyntax node)
        {
            Visit(node.Statement);
        }
    }
}