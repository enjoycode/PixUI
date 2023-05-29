using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    partial class Emitter
    {
        public override void VisitCheckedStatement(CheckedStatementSyntax node)
        {
            Visit(node.Block);
        }
    }
}