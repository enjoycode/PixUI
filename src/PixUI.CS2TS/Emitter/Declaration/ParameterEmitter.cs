using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    partial class Emitter
    {
        public override void VisitParameter(ParameterSyntax node)
        {
            var isParams = node.Modifiers.Any(m => m.Kind() == SyntaxKind.ParamsKeyword);
            if (isParams)
                Write("...");

            Write(node.Identifier.Text);

            //需要特殊处理抽象方法的默认参数以及接口方法 eg: abstract void SomeMethod(SomeType? para = null);
            var ignoreDefault = node.Default != null &&
                                node.Parent?.Parent is MethodDeclarationSyntax methodDeclaration &&
                                (methodDeclaration.HasAbstractModifier() ||
                                 methodDeclaration.Parent is InterfaceDeclarationSyntax);
            if (!ToJavaScript && ignoreDefault /* &&
                node.Default!.Value is LiteralExpressionSyntax literal &&
                literal.Kind() == SyntaxKind.NullLiteralExpression*/)
                Write('?');

            if (!ToJavaScript && node.Type != null)
            {
                Write(": ");
                //如果是ref或out参数，转换为System.Ref or System.Out
                var isRef = node.Modifiers.Any(m => m.Kind() == SyntaxKind.RefKeyword);
                var isOut = node.Modifiers.Any(m => m.Kind() == SyntaxKind.OutKeyword);
                if (isRef || isOut)
                {
                    AddUsedModule("System");
                    Write(isRef ? "System.Ref<" : "System.Out<");
                }

                //如果标记为TSTypeAttribute，转换为相应的类型
                if (node.IsTSType(out var tsType))
                    Write(tsType!);
                else
                    Visit(node.Type);

                if (isRef || isOut)
                    Write('>');
            }

            if (node.Default != null && !ignoreDefault)
                Visit(node.Default);
        }
    }
}