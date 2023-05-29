using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    partial class Emitter
    {
        public override void VisitAnonymousObjectCreationExpression(AnonymousObjectCreationExpressionSyntax node)
        {
            VisitToken(node.OpenBraceToken);
            VisitSeparatedList(node.Initializers);
            VisitToken(node.CloseBraceToken);
        }
        
        public override void VisitAnonymousObjectMemberDeclarator(AnonymousObjectMemberDeclaratorSyntax node)
        {
            if (node.NameEquals != null)
            {
                WriteLeadingTrivia(node.NameEquals);
                Write(node.NameEquals.Name.Identifier.Text);
                Write(": ");
            }
                
            Visit(node.Expression);
        }

    }
}