using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    internal sealed class TSPropertyToGetSetInterceptor : ITSInterceptor
    {
        internal static readonly TSPropertyToGetSetInterceptor Default = new();

        private TSPropertyToGetSetInterceptor() { }

        void ITSInterceptor.Emit(Emitter emitter, SyntaxNode node, ISymbol symbol)
        {
            if (node is MemberAccessExpressionSyntax memberAccess)
            {
                InterceptPropertyGetter(emitter, memberAccess);
            }
            else if (node is AssignmentExpressionSyntax assignment)
            {
                InteceptPropertySetter(emitter, assignment);
            }
            else
            {
                throw new NotSupportedException(node.GetType().ToString());
            }
        }

        private void InterceptPropertyGetter(Emitter emitter,
            MemberAccessExpressionSyntax memberAccess)
        {
            emitter.Visit(memberAccess.Expression);
            emitter.Write(".get");
            emitter.Write(memberAccess.Name.Identifier.Text);
            emitter.Write("()");
            emitter.VisitLeadingTrivia(memberAccess.Name.Identifier);
        }

        private void InteceptPropertySetter(Emitter emitter,
            AssignmentExpressionSyntax assignment)
        {
            var memberAccess = (MemberAccessExpressionSyntax)assignment.Left;

            emitter.Visit(memberAccess.Expression);
            emitter.Write(".set");
            emitter.Write(memberAccess.Name.Identifier.Text);
            emitter.Write('(');
            if (assignment.OperatorToken.Kind() != SyntaxKind.EqualsToken)
                throw new NotImplementedException();
            emitter.Visit(assignment.Right); //TODO: *** fix +=等
            emitter.Write(')');

            emitter.WriteTrailingTrivia(assignment.Right);
        }
    }
}