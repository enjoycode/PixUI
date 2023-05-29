using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    partial class Emitter
    {
        public override void VisitQualifiedName(QualifiedNameSyntax node)
        {
            //暂这里忽略命名空间，eg: namespace1.Namespace2.XXX
            if (SemanticModel.GetSymbolInfo(node.Left).Symbol is not INamespaceSymbol)
            {
                Visit(node.Left);
                Write('.');
            }

            Visit(node.Right);
        }
    }
}