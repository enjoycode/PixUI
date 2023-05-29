using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    partial class Emitter
    {
        public override void VisitSimpleLambdaExpression(SimpleLambdaExpressionSyntax node)
        {
            VisitToken(node.AsyncKeyword);
            Visit(node.Parameter);

            var position = GetCurrentOutputPosition();
            RemoveNewLineBefore(position);
            position = GetCurrentOutputPosition();
            
            DisableVisitLeadingTrivia();
            Write(' ');
            VisitToken(node.ArrowToken);
            EnableVisitLeadingTrivia();

            var isAnonymousObjectCreation = node.ExpressionBody is AnonymousObjectCreationExpressionSyntax;
            if (isAnonymousObjectCreation)
                Write("{ return ");

            Visit(node.ExpressionBody ?? node.Body);
            
            if (isAnonymousObjectCreation)
                Write('}');

            RemoveNewLineAfter(position);
        }

        public override void VisitParenthesizedLambdaExpression(ParenthesizedLambdaExpressionSyntax node)
        {
            VisitToken(node.AsyncKeyword);
            Visit(node.ParameterList);
            
            var position = GetCurrentOutputPosition();
            RemoveNewLineBefore(position);
            position = GetCurrentOutputPosition();
            
            DisableVisitLeadingTrivia();
            Write(' ');
            VisitToken(node.ArrowToken);
            EnableVisitLeadingTrivia();
            
            var isAnonymousObjectCreation = node.ExpressionBody is AnonymousObjectCreationExpressionSyntax;
            if (isAnonymousObjectCreation)
                Write("{ return ");
            
            Visit(node.ExpressionBody ?? node.Body);
            
            if (isAnonymousObjectCreation)
                Write('}');
            
            RemoveNewLineAfter(position);
        }
    }
}