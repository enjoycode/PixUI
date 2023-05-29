using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    partial class Emitter
    {
        public override void VisitLocalFunctionStatement(LocalFunctionStatementSyntax node)
        {
            WriteLeadingTrivia(node);
            Write("function ");
            VisitToken(node.Identifier);
            
            Visit(node.ParameterList);
            if (node.ExpressionBody != null)
                throw new NotImplementedException($"LocalFunctionStatement with ExpressionBody at File: {node.SyntaxTree.FilePath}");
            
            Visit(node.Body);
        }
    }
}