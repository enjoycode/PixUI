using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    partial class Emitter
    {
        public override void VisitInterpolatedStringExpression(InterpolatedStringExpressionSyntax node)
        {
            WriteLeadingTrivia(node);
            Write('`');

            foreach (var item in node.Contents)
            {
                Visit(item);
            }

            Write('`');
            WriteTrailingTrivia(node);
        }
    }
}