using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    partial class Emitter
    {
        public override void VisitVariableDeclarator(VariableDeclaratorSyntax node)
        {
            var parent = (VariableDeclarationSyntax)node.Parent!;
            if (parent.Parent is FieldDeclarationSyntax fieldDeclaration)
            {
                WriteModifiers(fieldDeclaration.Modifiers);
            }

            Write(node.Identifier.Text);
            if (!ToJavaScript && parent.Type is not IdentifierNameSyntax { IsVar: true })
            {
                // 不再需要，已关闭ts的严格null检查
                // if (node.Initializer != null && node.Initializer.Value.Kind() ==
                //     SyntaxKind.SuppressNullableWarningExpression)
                // {
                //     //definite assignment assertion
                //     emitter.Write('!');
                // }

                Write(": ");
                DisableVisitLeadingTrivia();
                Visit(parent.Type);
                EnableVisitLeadingTrivia();
            }

            if (node.Initializer != null)
            {
                // 忽略 xx = null!
                if (node.Initializer.Value is PostfixUnaryExpressionSyntax postfixUnary &&
                    postfixUnary.Operand.Kind() == SyntaxKind.NullLiteralExpression) return;

                Write(" = ");

                //判断是否需要隐式转换类型
                var typeInfo = SemanticModel.GetTypeInfo(node.Initializer.Value);
                TryImplictConversionOrStructClone(typeInfo, node.Initializer.Value);
            }
            else
            {
                //尝试写入非空值类型的默认值，暂简单排除readonly field declaration(肯定由构造初始化值)
                if (parent.Parent is FieldDeclarationSyntax field && field.HasReadOnlyModifier()) return;
                TryWriteDefaultValueForValueType(parent.Type, node);
            }
        }
    }
}