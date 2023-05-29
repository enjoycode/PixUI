using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    partial class Emitter
    {
        public override void VisitAssignmentExpression(AssignmentExpressionSyntax node)
        {
            //判断是否拦截TSPropertyToGetSet
            if (TryEmitPropertyToSet(node)) return;

            var leftSymbol = SemanticModel.GetSymbolInfo(node.Left).Symbol;
            var rightSymbol = SemanticModel.GetSymbolInfo(node.Right).Symbol;

            //判断是否需要转换 obj[idx] = xxx to: obj.SetAt(idx, xxx)
            if (TryEmitIndexerSet(node, leftSymbol, rightSymbol))
                return;
            //判断是否事件+=或者-=操作
            if (TryEmitEventAssignment(node, leftSymbol, rightSymbol))
                return;

            // 排除 _ = someGetValue();
            if (!(node.Kind() == SyntaxKind.SimpleAssignmentExpression &&
                  node.Left is IdentifierNameSyntax { Identifier: { Text: "_" } }))
            {
                Visit(node.Left);
                VisitToken(node.OperatorToken);
            }

            //right 隐式转换
            var typeInfo = SemanticModel.GetTypeInfo(node.Right);
            TryImplictConversionOrStructClone(typeInfo, node.Right);
        }

        private bool TryEmitEventAssignment(AssignmentExpressionSyntax node,
            ISymbol? leftSymbol, ISymbol? rightSymbol)
        {
            var kind = node.OperatorToken.Kind();
            if (kind != SyntaxKind.PlusEqualsToken && kind != SyntaxKind.MinusEqualsToken)
                return false;

            var isEvent = leftSymbol is IEventSymbol;
            if (isEvent)
            {
                Visit(node.Left);
                Write(kind == SyntaxKind.PlusEqualsToken ? ".Add(" : ".Remove(");
                IgnoreDelegateBind = true;
                Visit(node.Right);
                IgnoreDelegateBind = false;

                //判断是否静态方法,实例方法需要传入实例作为回调的this
                if (rightSymbol is { IsStatic: false } && node.Right is not LambdaExpressionSyntax)
                {
                    Write(", ");

                    //TODO: check other type of node.Right
                    if (node.Right is MemberAccessExpressionSyntax memberAccess)
                    {
                        if (memberAccess.Expression is ObjectCreationExpressionSyntax)
                            throw new NotSupportedException("Can't bind to ObjectCreation");

                        Visit(memberAccess.Expression);
                    }
                    else
                    {
                        Write("this");
                    }
                }

                Write(')');
                return true;
            }

            return false;
        }

        private bool TryEmitPropertyToSet(AssignmentExpressionSyntax node)
        {
            if (node.Left is MemberAccessExpressionSyntax memberAccess)
            {
                var symbolInfo = SemanticModel.GetSymbolInfo(memberAccess.Name);
                if (TryGetInterceptor(symbolInfo.Symbol, out var interceptor))
                {
                    interceptor!.Emit(this, node, symbolInfo.Symbol!);
                    return true;
                }
            }

            return false;
        }

        private bool TryEmitIndexerSet(AssignmentExpressionSyntax node, ISymbol? leftSymbol, ISymbol? rightSymbol)
        {
            if (leftSymbol is not IPropertySymbol { IsIndexer: true } propertySymbol) return false;

            var isDictionary = IsDictionayType(propertySymbol.ContainingType);
            var attribute = propertySymbol.SetMethod!.TryGetAttribute(TypeOfTSIndexerSetToMethodAttribute);
            if (!isDictionary && attribute == null) return false;

            var elementAccess = (ElementAccessExpressionSyntax)node.Left;
            Visit(elementAccess.Expression);
            Write(".SetAt(");
            Visit(elementAccess.ArgumentList.Arguments[0]);
            Write(',');
            Visit(node.Right);
            Write(")");

            WriteTrailingTrivia(node.Right);
            return true;
        }
    }
}