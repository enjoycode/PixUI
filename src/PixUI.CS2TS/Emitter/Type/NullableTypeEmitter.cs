using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    partial class Emitter
    {
        public override void VisitNullableType(NullableTypeSyntax node)
        {
            if (node.ElementType is PredefinedTypeSyntax predefined &&
                predefined.Keyword.Kind() == SyntaxKind.ObjectKeyword)
            {
                WriteLeadingTrivia(node);
                Write("any");
                WriteTrailingTrivia(node);
                return;
            }
            
            Write("Nullable<");
            DisableVisitLeadingTrivia();
            Visit(node.ElementType);
            EnableVisitLeadingTrivia();
            Write('>');

            WriteTrailingTrivia(node);
        }
    }
}