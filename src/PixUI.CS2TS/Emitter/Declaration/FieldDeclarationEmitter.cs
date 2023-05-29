using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    partial class Emitter
    {
        public override void VisitFieldDeclaration(FieldDeclarationSyntax node)
        {
            WriteLeadingTrivia(node);

            Visit(node.Declaration);

            VisitToken(node.SemicolonToken);
        }
    }
}