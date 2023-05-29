using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    partial class Emitter
    {
        public override void VisitIndexerDeclaration(IndexerDeclarationSyntax node)
        {
            //分别判断是否有TSIndexerGetAtAttribute or TSIndexerSetAtAttribute
            if (node.AccessorList == null) return;

            foreach (var accessor in node.AccessorList.Accessors)
            {
                if (accessor.Keyword.Kind() == SyntaxKind.GetKeyword)
                {
                    //TODO: 处理转换GetAt
                }
                else if (accessor.Keyword.Kind() == SyntaxKind.SetKeyword)
                {
                    var attribute = SyntaxExtensions.TryGetAttribute(accessor.AttributeLists, IsTSIndexerSetToMethodAttribute);
                    if (attribute == null) return;
                    
                    WriteLeadingTrivia(node);
                    WriteModifiers(accessor.Modifiers.Any() ? accessor.Modifiers : node.Modifiers);
                    Write("void SetAt(");
                    Visit(node.ParameterList.Parameters[0]);
                    Write(", value");
                    if (!ToJavaScript)
                    {
                        Write(": ");
                        Visit(node.Type);
                    }
                    Write(")");
                    
                    if (accessor.ExpressionBody != null)
                    {
                        Write(" { ");
                        Visit(accessor.ExpressionBody.Expression);
                        Write("; }");
                    }
                    else
                    {
                        Write('\n');
                        Visit(accessor.Body);
                    }
                }
            }
            
        }
    }
}