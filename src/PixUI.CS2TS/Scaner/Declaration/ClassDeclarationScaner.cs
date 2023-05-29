using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    partial class Scaner
    {
        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            //忽略嵌套的类
            if (node.Parent is not ClassDeclarationSyntax)
            {
                AddTypeInfo(node.Identifier.Text, ScanedType.Class, 
                    node.TypeParameterList?.Parameters.Count ?? 0, node);
            }
            
            //TODO:处理构造重载
            
            //TODO:处理方法重载

            base.VisitClassDeclaration(node);
        }
    }
}