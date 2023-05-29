using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    partial class Emitter
    {
        public override void VisitVariableDeclaration(VariableDeclarationSyntax node)
        {
            WriteLeadingTrivia(node.Type);

            //由于TypeScript不支持多变量定义，暂转换为相同的多个定义
            if (node.Parent is not (FieldDeclarationSyntax or PropertyDeclarationSyntax))
                Write("let ");

            var multi = false;
            foreach (var variable in node.Variables)
            {
                if (multi)
                {
                    //可能上级为for(declaration内)
                    Write(node.Parent is ForStatementSyntax ? ", " : "; ");
                    if (node.Parent is not (FieldDeclarationSyntax or PropertyDeclarationSyntax or ForStatementSyntax))
                        Write("let ");
                }
                else multi = true;

                Visit(variable);
            }
        }
    }
}