using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    partial class Emitter
    {
        public override void VisitIdentifierName(IdentifierNameSyntax node)
        {
            VisitLeadingTrivia(node.Identifier);

            var symbol = SemanticModel.GetSymbolInfo(node).Symbol!;

            //转换实例成员或静态成员
            if (symbol is IPropertySymbol or IFieldSymbol or IMethodSymbol or IEventSymbol)
            {
                if (symbol is not IMethodSymbol { MethodKind: MethodKind.LocalFunction })
                    TryWriteThisOrStaticMemberType(node, symbol);
            }
            //转换类型(添加包名称)
            else if (symbol is INamedTypeSymbol)
            {
                if (TryInterceptorSystem(node, symbol))
                    return;
                TryWritePackageName(node, symbol);
                TryWriteParentTypeOfInnerClass(node, symbol);
            }

            var name = symbol.Name; //可能using改了名称,不能用node.Identifier.Text;
            if (symbol is not ILocalSymbol && symbol is not IParameterSymbol)
                TryRenameSymbol(symbol, ref name);
            Write(name);
            if (symbol is IParameterSymbol { RefKind: RefKind.Ref or RefKind.Out })
                Write(".Value");

            //转换委托的this绑定
            if (!IgnoreDelegateBind && symbol is IMethodSymbol && !symbol.IsStatic)
            {
                var typeInfo = SemanticModel.GetTypeInfo(node);
                if (typeInfo.ConvertedType is { TypeKind: TypeKind.Delegate })
                {
                    EmitDelegateBind(node, symbol);
                }
            }

            VisitTrailingTrivia(node.Identifier);
        }

        /// <summary>
        /// 尝试系统类型的拦截
        /// </summary>
        private bool TryInterceptorSystem(IdentifierNameSyntax node, ISymbol symbol)
        {
            if (!symbol.IsSystemNamespace() || !SystemInterceptorMap.TryGetInterceptor(
                    symbol.ToString(), out var interceptor))
                return false;
            interceptor.Emit(this, node, symbol);
            return true;
        }

        private void EmitDelegateBind(IdentifierNameSyntax node, ISymbol symbol)
        {
            Write(".bind(");
            if (node.Parent is MemberAccessExpressionSyntax memberAccess)
            {
                if (memberAccess.Expression is ObjectCreationExpressionSyntax)
                    throw new NotSupportedException("Can't bind to ObjectCreation");

                Visit(memberAccess.Expression);
            }
            // else if (node.Parent is MemberBindingExpressionSyntax memberBinding)
            // {
            //     emitter.Visit(memberBinding.Name);
            // }
            else
            {
                Write("this");
            }

            Write(')');
        }
    }
}