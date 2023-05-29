using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    internal sealed class StringInterceptor : ITSInterceptor
    {
        void ITSInterceptor.Emit(Emitter emitter, SyntaxNode node, ISymbol symbol)
        {
            switch (node)
            {
                case InvocationExpressionSyntax invocation:
                    InterceptInvocation(emitter, invocation, symbol);
                    break;
                case MemberAccessExpressionSyntax memberAccess:
                    InterceptMemberAccess(emitter, memberAccess, symbol);
                    break;
                case ObjectCreationExpressionSyntax objectCreation:
                    InterceptCreation(emitter, objectCreation, (IMethodSymbol)symbol);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private static void InterceptCreation(Emitter emitter, ObjectCreationExpressionSyntax node,
            IMethodSymbol symbol)
        {
            //TODO: Only support new string(char, count) now
            if (symbol.Parameters.Length != 2) throw new NotImplementedException();
            if (symbol.Parameters[0].Type.ToString() != "char" ||
                symbol.Parameters[1].ToString() != "int")
                throw new NotImplementedException();

            var arg1 = node.ArgumentList!.Arguments[0];
            var arg2 = node.ArgumentList!.Arguments[1];
            if (arg1.Expression is LiteralExpressionSyntax)
            {
                //Don't Visit, will convert to char code, emitter.Visit(arg1);
                emitter.Write(arg1.ToString());
            }
            else
            {
                emitter.Write("String.fromCharCode("); //char转换为js字符串
                emitter.Visit(arg1);
                emitter.Write(')');
            }

            emitter.Write(".repeat(");
            emitter.Visit(arg2);
            emitter.Write(')');
        }

        private static void InterceptInvocation(Emitter emitter, InvocationExpressionSyntax node, ISymbol symbol)
        {
            //TODO:验证不支持的转换方法
            if (TryEmitIsNullOrEmptyOrWhiteSpace(emitter, node))
                return;
            
            // var methodSymbol = (IMethodSymbol)symbol;
            emitter.Visit(node.Expression);
            emitter.VisitToken(node.ArgumentList.OpenParenToken);
            //需要特殊处理char参数(因会被转为number)，所以需要通过String.fromCharCode()重新转为js字符串
            //var preCharCodeToString = emitter.CharCodeToString;
            emitter.CharCodeToString = true;
            emitter.VisitSeparatedList(node.ArgumentList.Arguments);
            emitter.CharCodeToString = false; //preCharCodeToString;
            emitter.VisitToken(node.ArgumentList.CloseParenToken);
        }

        private static bool TryEmitIsNullOrEmptyOrWhiteSpace(Emitter emitter, InvocationExpressionSyntax node)
        {
            if (node.Expression is not MemberAccessExpressionSyntax memberAccess) return false;
            var name = memberAccess.Name.Identifier.Text;
            if (name != "IsNullOrEmpty" && name != "IsNullOrWhiteSpace") return false;

            emitter.AddUsedModule("System");
            emitter.WriteLeadingTrivia(node);
            emitter.Write("System.");
            emitter.Write(name);
            emitter.Write('(');
            emitter.Visit(node.ArgumentList.Arguments[0]);
            emitter.Write(')');
            emitter.WriteTrailingTrivia(node);
            return true;
        }

        private static void InterceptMemberAccess(Emitter emitter, MemberAccessExpressionSyntax node, ISymbol symbol)
        {
            if (node.Name.Identifier.Text == "Empty" /*&& (node.Expression.ToString() == "string")*/)
            {
                emitter.Write("''");
                return;
            }

            emitter.Visit(node.Expression);
            emitter.Write('.');

            var name = node.Name.Identifier.Text;
            if (name == "ToLower")
            {
                emitter.Write("toLowerCase");
                return;
            }

            if (name == "StartsWith")
            {
                //TODO: only one argument supported now
                var invocation = (InvocationExpressionSyntax)node.Parent!;
                if (invocation.ArgumentList.Arguments.Count > 1)
                    throw new NotSupportedException();
            }

            //TODO: 除特殊为暂全部转为lowerCamelCase格式
            if (name != "Insert" && name != "Remove")
            {
                emitter.Write(char.ToLower(name[0]));
                emitter.Write(name.Substring(1));
            }
            else
            {
                emitter.Visit(node.Name);
            }
        }
    }
}