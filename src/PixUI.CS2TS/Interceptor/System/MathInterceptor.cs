using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    internal sealed class MathInterceptor : ITSInterceptor
    {
        void ITSInterceptor.Emit(Emitter emitter, SyntaxNode node, ISymbol symbol)
        {
            if (node is InvocationExpressionSyntax invocation)
            {
                InterceptInvocation(emitter, invocation, symbol);
            }
            else if (node is MemberAccessExpressionSyntax memberAccess)
            {
                emitter.Write("Math.");
                emitter.Write(memberAccess.Name.Identifier.Text);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private static void InterceptInvocation(Emitter emitter, InvocationExpressionSyntax node, ISymbol symbol)
        {
            //TODO:处理Math.Round(num, 3)有指定小数位
            var memberAccess = (MemberAccessExpressionSyntax)node.Expression;
            var methodName = memberAccess.Name.Identifier.Text;
            if (methodName != "Clamp")
                emitter.Write("Math.");

            if (methodName == "Truncate")
                emitter.Write("trunc");
            else if (methodName == "Ceiling")
                emitter.Write("ceil");
            else
                emitter.Write(methodName.ToLower());

            emitter.Write('(');
            emitter.Visit(node.ArgumentList);
            emitter.Write(')');
        }
    }
}