using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    internal sealed class EqualityComparerInterceptor : ITSInterceptor
    {
        public void Emit(Emitter emitter, SyntaxNode node, ISymbol symbol)
        {
            if (node is InvocationExpressionSyntax invocation)
            {
                if (symbol.Name == "Equals")
                {
                    emitter.AddUsedModule("System");
                    
                    emitter.Write("System.Equals(");
                    emitter.Visit(invocation.ArgumentList.Arguments[0]);
                    emitter.Write(',');
                    emitter.Visit(invocation.ArgumentList.Arguments[1]);
                    emitter.Write(')');
                }
                else
                {
                    throw new NotImplementedException(symbol.Name);
                }
            }
            else
            {
                throw new NotImplementedException(nameof(EqualityComparerInterceptor));
            }
        }
    }
}