using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    partial class Emitter
    {
        public override void VisitReturnStatement(ReturnStatementSyntax node)
        {
            //dispose block using
            var autoBlock = AutoDisposeBeforeReturn(node);

            VisitToken(node.ReturnKeyword);
            if (node.Expression != null)
            {
                //因为ts不支持 return [此处换行] someValue;所以需要移除之间的换行符
                var pos = GetCurrentOutputPosition();
                RemoveNewLineBefore(pos);
                pos = GetCurrentOutputPosition();
                Write(' ');
                Visit(node.Expression);
                RemoveNewLineAfter(pos);
            }
            VisitToken(node.SemicolonToken);

            if (autoBlock)
            {
                WriteLeadingWhitespaceOnly(node.Parent!);
                Write("}\n"); 
            }
        }
    }
}