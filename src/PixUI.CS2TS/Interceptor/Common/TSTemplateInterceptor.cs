using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    internal sealed class TSTemplateInterceptor : ITSInterceptor
    {
        private readonly string _template;

        public TSTemplateInterceptor(string template)
        {
            _template = template;
        }

        void ITSInterceptor.Emit(Emitter emitter, SyntaxNode node, ISymbol symbol)
        {
            switch (node)
            {
                case BaseObjectCreationExpressionSyntax objectCreation:
                    InterceptObjectCreation(emitter, objectCreation);
                    break;
                case InvocationExpressionSyntax invocation:
                    InterceptInvocation(emitter, invocation, symbol);
                    break;
                case MemberAccessExpressionSyntax memberAccess:
                    InterceptMemberAccess(emitter, memberAccess);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void InterceptObjectCreation(Emitter emitter,
            BaseObjectCreationExpressionSyntax node)
        {
            var argList = node.ArgumentList;
            var argsLen = argList == null ? 0 : argList.Arguments.Count;
            argsLen++;
            var args = new object[argsLen];
            if (argList != null)
            {
                for (var i = 0; i < argList.Arguments.Count; i++)
                {
                    emitter.UseTempOutput();
                    emitter.Visit(argList.Arguments[i]);
                    args[i + 1] = emitter.GetTempOutput();
                }
            }

            var output = string.Format(_template, args);
            emitter.Write(output);
        }

        private void InterceptInvocation(Emitter emitter, InvocationExpressionSyntax node,
            ISymbol symbol)
        {
            //TODO: 默认值及可选参数处理，以下参数数量会与模版不匹配
            var argList = node.ArgumentList;
            var args = new object[argList.Arguments.Count + 1];

            //先处理Expression
            emitter.UseTempOutput();
            if (node.Expression is MemberAccessExpressionSyntax memberAccess)
                emitter.Visit(memberAccess.Expression);
            else if (node.Expression is IdentifierNameSyntax identifierName)
                emitter.Visit(identifierName);
            else
                throw new NotImplementedException(
                    $"{nameof(TSTemplateInterceptor)}.{node.Expression.GetType()}");
            args[0] = emitter.GetTempOutput();

            //再处理参数列表
            for (var i = 0; i < argList.Arguments.Count; i++)
            {
                emitter.UseTempOutput();
                emitter.Visit(argList.Arguments[i]);
                args[i + 1] = emitter.GetTempOutput();
            }

            var output = string.Format(_template, args);
            emitter.Write(output);
        }

        private void InterceptMemberAccess(Emitter emitter, MemberAccessExpressionSyntax node)
        {
            emitter.UseTempOutput();
            emitter.Visit(node.Expression);
            var arg = emitter.GetTempOutput();

            var output = string.Format(_template, arg);
            emitter.Write(output);
        }
    }
}