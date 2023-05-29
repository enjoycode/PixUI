using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    internal sealed class WeakReferenceInterceptor : ITSInterceptor
    {
        void ITSInterceptor.Emit(Emitter emitter, SyntaxNode node, ISymbol symbol)
        {
            if (node is MemberAccessExpressionSyntax memberAccess)
                InterceptMemberAccess(emitter, memberAccess, symbol);
            else if (node is IdentifierNameSyntax identifierName)
                InterceptIndetifierName(emitter, identifierName, symbol);
            else if (node is ObjectCreationExpressionSyntax creation)
                InterceptCreation(emitter, creation, (IMethodSymbol)symbol);
            else
                throw new NotImplementedException();
        }

        private static void InterceptCreation(Emitter emitter, ObjectCreationExpressionSyntax node,
            IMethodSymbol symbol)
        {
            emitter.Write("new WeakRef<any>(");
            emitter.Visit(node.ArgumentList!.Arguments[0]);
            emitter.Write(')');
        }

        private static void InterceptIndetifierName(Emitter emitter, IdentifierNameSyntax node,
            ISymbol symbol)
        {
            emitter.Write("WeakRef<any>");
        }

        private static void InterceptMemberAccess(Emitter emitter,
            MemberAccessExpressionSyntax node, ISymbol symbol)
        {
            if (node.Name.Identifier.Text == "Target")
            {
                emitter.Visit(node.Expression);
                emitter.Write(".deref()");
            }
            else if (node.Name.Identifier.Text == "IsAlive")
            {
                emitter.Visit(node.Expression);
                emitter.Write(".deref() !== undefined");
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}