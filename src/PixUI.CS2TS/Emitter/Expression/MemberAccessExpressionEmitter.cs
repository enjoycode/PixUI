using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    partial class Emitter
    {
        public override void VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
        {
            var symbol = SemanticModel.GetSymbolInfo(node).Symbol!;

            //先判断是否System命名空间的成员
            if (symbol.IsSystemNamespace())
            {
                if (TryEmitNullable(node, symbol))
                    return;

                if (SystemInterceptorMap.TryGetInterceptor(symbol.ContainingType.ToString(),
                        out var systemInterceptor))
                {
                    systemInterceptor.Emit(this, node, symbol);
                    return;
                }
            }
            //再判断是否有拦截器
            else if (TryGetInterceptor(symbol, out var interceptor))
            {
                interceptor!.Emit(this, node, symbol);
                return;
            }

            var expressionSymbol = SemanticModel.GetSymbolInfo(node.Expression).Symbol;

            //特殊处理CanvasKit导出的枚举(因为只导出了类型)
            if (TryEmitCanvasKitEnumMember(node, expressionSymbol))
                return;

            //暂这里忽略命名空间，eg: namespace1.Namespace2.XXX
            if (expressionSymbol is not INamespaceSymbol)
            {
                Visit(node.Expression);
                VisitToken(node.OperatorToken); //TODO: not supported pointer
            }

            // eg: list.Count convert to list.length
            if (!TryRenameCollectionCountProperty(node, symbol))
                Visit(node.Name);
        }

        private bool TryEmitNullable(MemberAccessExpressionSyntax node, ISymbol symbol)
        {
            var name = node.Name.Identifier.Text;
            if (name is not ("Value" or "HasValue") ||
                !symbol.ContainingType.IsNullableType(TypeOfNullable))
                return false;

            Visit(node.Expression);
            // 暂不写入！，前端ts关闭严格null模式
            // if (name == "Value" && node.Parent is not AssignmentExpressionSyntax)
            //     emitter.Write('!');
            return true;
        }

        private bool TryRenameCollectionCountProperty(MemberAccessExpressionSyntax node, ISymbol symbol)
        {
            if (node.Name.Identifier.Text == "Count" && symbol is IPropertySymbol propertySymbol)
            {
                var type = propertySymbol.ContainingType;
                var isCollection = IsCollectionType(type);
                if (isCollection)
                {
                    Write("length");
                    VisitTrailingTrivia(node.Name.Identifier);
                    return true;
                }
            }

            return false;
        }

        private bool TryEmitCanvasKitEnumMember(MemberAccessExpressionSyntax node, ISymbol? expressionSymbol)
        {
            if (expressionSymbol is INamedTypeSymbol { TypeKind: TypeKind.Enum } typeSymbol &&
                typeSymbol.ContainingNamespace.Name == Emitter.PixUIProjectName)
            {
                var tsTypeAttribute = typeSymbol.TryGetAttribute(TypeOfTSTypeAttribute);
                if (tsTypeAttribute == null) return false;

                var tsTypeName = tsTypeAttribute.ConstructorArguments[0].Value!.ToString();
                if (!tsTypeName.StartsWith("CanvasKit.")) return false;

                Write(tsTypeName);
                Write('.');
                Write(node.Name.Identifier.Text);
                return true;
            }

            return false;
        }
    }
}