using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    internal sealed class ArrayInterceptor : ITSInterceptor
    {
        void ITSInterceptor.Emit(Emitter emitter, SyntaxNode node, ISymbol symbol)
        {
            switch (node)
            {
                case MemberAccessExpressionSyntax memberAccess:
                    InterceptMemberAccess(emitter, memberAccess, symbol);
                    break;
                case InvocationExpressionSyntax invocation:
                    InterceptInvocation(emitter, invocation, symbol);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private static void InterceptMemberAccess(Emitter emitter,
            MemberAccessExpressionSyntax node,
            ISymbol symbol)
        {
            if (node.Name.Identifier.Text == "Length")
            {
                emitter.Visit(node.Expression);
                emitter.Write(".length");
                return;
            }

            throw new EmitException($"Not supported: Array.{symbol.Name}", node.Span);
        }

        private static void InterceptInvocation(Emitter emitter, InvocationExpressionSyntax node,
            ISymbol symbol)
        {
            if (node.Expression is MemberAccessExpressionSyntax memberAccess)
            {
                if (memberAccess.Name.Identifier.Text == "Empty")
                {
                    var methodSymbol = (IMethodSymbol)symbol;
                    var jsArrayType = Emitter.GetJsNativeArrayTypeByElementType(methodSymbol.TypeArguments[0]);
                    emitter.Write(jsArrayType != null ? $"new {jsArrayType}()" : "[]");
                }
                else if (memberAccess.Name.Identifier.Text == "IndexOf")
                {
                    emitter.Visit(node.ArgumentList.Arguments[0]);
                    emitter.Write(".indexOf(");
                    emitter.Visit(node.ArgumentList.Arguments[1]);
                    emitter.Write(')');
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}