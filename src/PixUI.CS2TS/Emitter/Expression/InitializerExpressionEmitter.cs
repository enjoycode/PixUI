using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    partial class Emitter
    {
        public override void VisitInitializerExpression(InitializerExpressionSyntax node)
        {
            var kind = node.Kind();
            var isArray = kind == SyntaxKind.ArrayInitializerExpression;
            var isObject = kind == SyntaxKind.ObjectInitializerExpression;

            //特殊处理Dictionary初始化
            if (kind == SyntaxKind.CollectionInitializerExpression)
            {
                var typeInfo = SemanticModel.GetTypeInfo(node.Parent!);
                if (IsDictionayType(typeInfo.Type))
                {
                    EmitDictionaryInitializer(node);
                    return;
                }
            }

            if (!isArray)
                Write(".Init(");

            var children = node.Parent!.ChildNodes().ToList();
            var index = children.IndexOf(node);
            if (index > 0)
                WriteTrailingTrivia(children[index - 1]);

            VisitLeadingTrivia(node.OpenBraceToken);
            Write(isObject ? '{' : '[');
            VisitTrailingTrivia(node.OpenBraceToken);

            var seps = node.Expressions.GetSeparators().ToArray();
            for (var i = 0; i < node.Expressions.Count; i++)
            {
                var expression = node.Expressions[i];

                WriteLeadingTrivia(expression);

                if (expression is AssignmentExpressionSyntax assignment)
                {
                    //Do not Visit assignment.Left
                    var memberName = assignment.Left.ToString();
                    TryRenameSymbol(ModelExtensions.GetSymbolInfo(SemanticModel, assignment.Left).Symbol!, ref memberName);
                    Write(memberName);
                    Write(": ");

                    //这里同样需要隐式转换或结构体Clone
                    var valueTypeInfo = ModelExtensions.GetTypeInfo(SemanticModel, assignment.Right);
                    TryImplictConversionOrStructClone(valueTypeInfo, assignment.Right);
                }
                else
                {
                    Visit(expression);
                }

                if (i < seps.Length)
                {
                    var sepToken = node.Expressions.GetSeparator(i);
                    VisitToken(sepToken);
                }
            }

            VisitLeadingTrivia(node.CloseBraceToken);
            Write(isObject ? '}' : ']');
            if (!isArray)
                Write(')');
            VisitTrailingTrivia(node.CloseBraceToken);
        }

        private void EmitDictionaryInitializer(InitializerExpressionSyntax node)
        {
            Write(".Init(");
            
            VisitLeadingTrivia(node.OpenBraceToken);
            Write('[');
            VisitTrailingTrivia(node.OpenBraceToken);
            
            var seps = node.Expressions.GetSeparators().ToArray();
            for (var i = 0; i < node.Expressions.Count; i++)
            {
                if (node.Expressions[i] is not InitializerExpressionSyntax initializerExpression)
                    throw new NotImplementedException();

                WriteLeadingTrivia(initializerExpression);
                
                VisitLeadingTrivia(initializerExpression.OpenBraceToken);
                Write('[');
                VisitSeparatedList(initializerExpression.Expressions);
                Write(']');
                VisitTrailingTrivia(initializerExpression.CloseBraceToken);
                
                if (i < seps.Length)
                {
                    var sepToken = node.Expressions.GetSeparator(i);
                    VisitToken(sepToken);
                }
            }
            
            VisitLeadingTrivia(node.CloseBraceToken);
            Write( ']');
            Write(')');
            VisitTrailingTrivia(node.CloseBraceToken);
        }
    }
}