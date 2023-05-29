using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    partial class Emitter
    {
        public override void VisitStructDeclaration(StructDeclarationSyntax node)
        {
            if (node.IsTSType(out _))
                return;
            
            //检查成员重载
            CheckTypeMemberOverloads(node);

            var export = node.NeedExport(out var isPublic);
            if (isPublic)
                AddPublicType(node);
            if (export)
                Write("export ");

            // struct name
            Write("class "); //转换为class
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

            //如果实现了标为[TSInterfaceOf]的接口，则生成特殊成员
            TryWriteInterfaceOfMeta(node);

            foreach (var member in node.Members)
            {
                Visit(member);
            }

            //TODO: 考虑使用Object.create生成不存在的Clone()

            VisitToken(node.CloseBraceToken);
        }
    }
}