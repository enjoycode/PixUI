using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    partial class Emitter
    {
        public override void VisitTypeOfExpression(TypeOfExpressionSyntax node)
        {
            throw new NotSupportedException($"不支持typeof(xxx)表达式 at File: {node.SyntaxTree.FilePath}");
        }
    }
}