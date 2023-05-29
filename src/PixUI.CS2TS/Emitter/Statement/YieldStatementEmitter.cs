using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
   partial class Emitter
   {
      public override void VisitYieldStatement(YieldStatementSyntax node)
      {
         WriteLeadingTrivia(node);

         if (node.ReturnOrBreakKeyword.Kind() == SyntaxKind.BreakKeyword)
         {
            Write("return");
         }
         else
         {
            Write("yield ");
            Visit(node.Expression);
         }
         
         VisitToken(node.SemicolonToken);
      }
   }
}