using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    partial class Emitter
    {
        public override void VisitImplicitObjectCreationExpression(ImplicitObjectCreationExpressionSyntax node)
        {
            var symbol = SemanticModel.GetSymbolInfo(node).Symbol;
            //预先处理 object obj = new();
            if (symbol is { ContainingType: ITypeSymbol { SpecialType: SpecialType.System_Object } })
            {
                WriteLeadingTrivia(node);
                Write("{}");
                WriteTrailingTrivia(node);
                return;
            }
            
            //尝试系统拦截
            if (symbol != null && symbol.IsSystemNamespace() &&
                SystemInterceptorMap.TryGetInterceptor(symbol.ContainingType.ToString(),
                    out var systemInterceptor))
            {
                WriteLeadingTrivia(node);
                systemInterceptor.Emit(this, node, symbol);
                return;
            }

            //尝试拦截器
            if (TryGetInterceptor(symbol, out var interceptor))
            {
                interceptor!.Emit(this, node, symbol!);
                return;
            }

            //特殊处理new RxEntity()
            if (TryEmitNewRxEntity(node, symbol!.ContainingType))
                return;
            //特殊处理new PixUI.Route()
            if (TryEmitNewRoute(node, symbol!.ContainingType))
                return;

            //以下正常处理
            VisitToken(node.NewKeyword);
            Write(' ');
            WriteTypeSymbol(symbol!.ContainingType, !node.InSameSourceFile(symbol.ContainingType));

            // arguments
            Write('(');
            VisitSeparatedList(node.ArgumentList.Arguments);
            Write(')');

            // initializer
            Visit(node.Initializer);
        }
    }
}