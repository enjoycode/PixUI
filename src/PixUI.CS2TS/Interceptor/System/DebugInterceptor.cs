using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    internal sealed class DebugInterceptor : ITSInterceptor
    {
        void ITSInterceptor.Emit(Emitter emitter, SyntaxNode node, ISymbol symbol)
        {
            if (node is InvocationExpressionSyntax invocation)
            {
                InterceptInvocation(emitter, invocation, symbol);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        
        private void InterceptInvocation(Emitter emitter, InvocationExpressionSyntax node,
            ISymbol symbol)
        {
            if (symbol.Name != "Assert" )
                throw new EmitException($"Not supported: Debug.{symbol.Name}()", node.Span);

            emitter.Write("console.assert(");
            emitter.Visit(node.ArgumentList);
            emitter.Write(')');
        }
        
    }
}