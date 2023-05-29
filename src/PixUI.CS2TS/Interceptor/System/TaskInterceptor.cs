using System;
using System.Xml;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    internal sealed class TaskInterceptor : ITSInterceptor
    {
        // private readonly string[] supportedMethods = {
        //     "Delay", "WhenAny"
        // };

        public void Emit(Emitter emitter, SyntaxNode node, ISymbol symbol)
        {
            if (node is InvocationExpressionSyntax invocation)
            {
                InterceptInvocation(emitter, invocation, symbol);
            }
            else if (node is IdentifierNameSyntax)
            {
                emitter.Write("Promise<void>");
            }
            else if (node is GenericNameSyntax genericName)
            {
                emitter.Write("Promise");
                emitter.Visit(genericName.TypeArgumentList);
            }
            else if (node is MemberAccessExpressionSyntax memberAccess)
            {
                if (memberAccess.Name.Identifier.Text == "CompletedTask")
                    emitter.Write("Promise.resolve()");
                else
                    throw new NotImplementedException(node.ToString());
            }
            else
            {
                throw new NotImplementedException(node.ToString());
            }
        }

        private static void InterceptInvocation(Emitter emitter, InvocationExpressionSyntax node, ISymbol symbol)
        {
            var memberAccess = (MemberAccessExpressionSyntax)node.Expression;
            var methodName = memberAccess.Name.Identifier.Text;

            if (methodName == "Delay")
                EmitTaskDelay(emitter, node);
            else if (methodName == "Run")
                EmitTaskRun(emitter, node);
            else if (methodName == "WhenAny")
                EmitTaskWhenAny(emitter, node);
            else
                throw new NotImplementedException($"不支持Task.{methodName} at File: {node.SyntaxTree.FilePath}");
        }

        private static void EmitTaskDelay(Emitter emitter, InvocationExpressionSyntax node)
        {
            emitter.Write("new Promise<void>($resolve => setTimeout(() => $resolve(),");
            emitter.Visit(node.ArgumentList);
            emitter.Write("))");
        }

        private static void EmitTaskRun(Emitter emitter, InvocationExpressionSyntax node)
        {
            //目前生成的代码仍旧在UI线程内，将来考虑使用WebWorker来处理
            emitter.Write("new Promise<void>($resolve => {\n");
            
            var expression = node.ArgumentList.Arguments[0].Expression;
            if (expression is LambdaExpressionSyntax lambdaExpression)
            {
                if (lambdaExpression.Block != null)
                {
                    foreach (var st in lambdaExpression.Block.Statements)
                    {
                        emitter.Visit(st);
                    }
                }
                else
                {
                    emitter.WriteLeadingTrivia(node);
                    emitter.Write('\t');
                    emitter.Visit(lambdaExpression.Body);
                    emitter.Write(";\n");
                }
            }
            else
            {
                emitter.WriteLeadingTrivia(node);
                emitter.Write('\t');
                emitter.Visit(expression);
                emitter.Write("();\n");
            }
            
            emitter.WriteLeadingTrivia(node);
            emitter.Write('\t');
            emitter.Write("$resolve();\n");
            emitter.WriteLeadingTrivia(node);
            emitter.Write("})");
        }

        private static void EmitTaskWhenAny(Emitter emitter, InvocationExpressionSyntax node)
        {
            var isArrayArg = false;
            if (node.ArgumentList.Arguments.Count == 1)
            {
                var singleArg = node.ArgumentList.Arguments[0].Expression;
                var argType = emitter.SemanticModel.GetTypeInfo(singleArg).Type!;
                isArrayArg = argType is IArrayTypeSymbol || emitter.IsCollectionType((INamedTypeSymbol)argType);
            }

            emitter.Write("Promise.any(");
            if (!isArrayArg) emitter.Write('[');
            emitter.Visit(node.ArgumentList);
            if (!isArrayArg) emitter.Write(']');
            emitter.Write(')');
        }
    }
}