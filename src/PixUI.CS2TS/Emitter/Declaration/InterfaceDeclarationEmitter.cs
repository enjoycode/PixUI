using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    partial class Emitter
    {
        public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
        {
            if (ToJavaScript) return;

            var export = node.NeedExport(out var isPublic);
            if (isPublic)
                AddPublicType(node);
            if (export)
                Write("export ");

            // interface name
            Write("interface ");
            var name = node.Identifier.Text;
            TryRenameDeclaration(node.AttributeLists, ref name);
            //TODO: 添加检查类型名称 Translator.AddType();
            VisitLeadingTrivia(node.Identifier);
            Write(name);
            VisitTrailingTrivia(node.Identifier);

            // generic type parameters
            WriteTypeParameters(node.TypeParameterList, node.ConstraintClauses);

            // extends and implements
            WriteBaseList(node.BaseList, node);

            // trailing trivia with some case
            if (node.ConstraintClauses.Any())
                WriteTrailingTrivia(node.ConstraintClauses.Last());

            // members
            VisitToken(node.OpenBraceToken);

            foreach (var member in node.Members)
            {
                Visit(member);
            }

            VisitToken(node.CloseBraceToken);

            //如果有[TSInterfaceOf]特性，生成相关的代码
            TryEmitInterfaceOf(node, name /*maybe renamed*/);
        }

        private void TryEmitInterfaceOf(InterfaceDeclarationSyntax node, string name)
        {
            if (!node.IsTSInterfaceOf()) return;

            Write("export function IsInterfaceOf");
            Write(name);
            Write("(obj: any): obj is ");
            Write(name);
            //需要写入范型参数，暂简单写入any类型
            if (node.TypeParameterList != null)
            {
                Write('<');
                for (var i = 0; i < node.TypeParameterList.Parameters.Count; i++)
                {
                    if (i != 0) Write(", ");
                    Write("any");
                }
                Write('>');
            }
            
            Write(" {\n");

            Write(
                "\treturn typeof obj === \"object\" && obj !== null && !Array.isArray(obj) && ");
            Write("'$meta_");

            var symbol = SemanticModel.GetDeclaredSymbol(node);
            var rootNamespace = symbol!.GetRootNamespace();
            if (rootNamespace != null)
            {
                AddUsedModule(rootNamespace.Name);
                Write(rootNamespace.Name);
                Write('_');
            }

            Write(name);
            Write("' in obj.constructor;\n}\n");
        }
    }
}