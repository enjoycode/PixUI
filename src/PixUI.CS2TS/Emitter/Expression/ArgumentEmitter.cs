using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;

namespace PixUI.CS2TS
{
    partial class Emitter
    {
        public override void VisitArgument(ArgumentSyntax node)
        {
            if (node.NameColon != null)
                throw new EmitException("Named argument not supported", node.Span);

            //先尝试处理ref or out参数
            if (TryEmitRefOrOutArg(node)) return;

            var typeInfo = SemanticModel.GetTypeInfo(node.Expression);
            //判断是否需要转换charCode为String
            var charCodeToString = false;
            if (CharCodeToString && typeInfo.Type != null && typeInfo.Type.ToString() == "char")
            {
                charCodeToString = true;
                Write("String.fromCharCode(");
            }

            //判断是否需要隐式类型转换
            TryImplictConversionOrStructClone(typeInfo, node.Expression);

            if (charCodeToString)
                Write(')');
        }

        private bool TryEmitRefOrOutArg(ArgumentSyntax node)
        {
            var isRefArg = node.RefKindKeyword.Kind() == SyntaxKind.RefKeyword;
            var isOutArg = node.RefKindKeyword.Kind() == SyntaxKind.OutKeyword;
            if (!(isRefArg || isOutArg)) return false;
            
            var symbol = SemanticModel.GetSymbolInfo(node.Expression).Symbol;
            if (symbol is IParameterSymbol { RefKind: RefKind.Ref or RefKind.Out })
            {
                //ref至参数且参数同样是ref的，则不需要转换了
                Write(node.Expression.ToString()); //直接写入参数名称
            }
            else
            {
                SyntaxNode exp = node.Expression;
                //如果out var xxx先在当前Block内注入变量
                if (isOutArg && node.Expression is DeclarationExpressionSyntax declaration)
                {
                    var statement = $"let {declaration.Designation}: any"; //暂转换为any类型
                    InjectBlockStatement(statement);
                    exp = declaration.Designation;
                }

                AddUsedModule("System");
                Write("new System.");
                Write(isRefArg ? "Ref" : "Out");
                Write("(()=>");
                Visit(exp);
                Write(", ");
                Write("$v=>");
                Visit(exp);
                Write("=$v)");
            }

            return true;
        }
    }
}