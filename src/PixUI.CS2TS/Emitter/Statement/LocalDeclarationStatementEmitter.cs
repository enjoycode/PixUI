using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    partial class Emitter
    {
        public override void VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
        {
            if (node.UsingKeyword.Value != null)
            {
                VisitLeadingTrivia(node.UsingKeyword);
                //add using resource to current block
                AddUsingResourceToBlock(node.Declaration);
            }

            Visit(node.Declaration);
            VisitToken(node.SemicolonToken);
        }
    }
}