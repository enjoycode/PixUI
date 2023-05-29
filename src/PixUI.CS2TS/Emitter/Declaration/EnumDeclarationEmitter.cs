using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    partial class Emitter
    {
        public override void VisitEnumDeclaration(EnumDeclarationSyntax node)
        {
            if (node.IsTSType(out _))
                return;

            var export = node.NeedExport(out var isPublic);
            if (isPublic)
                AddPublicType(node);
            if (export)
                Write("export ");

            if (ToJavaScript)
                EnumToJavaScript(node);
            else
                EnumToTypeScript(node);
        }

        private void EnumToJavaScript(EnumDeclarationSyntax node)
        {
            var name = node.Identifier.Text;
            Write($"var {name};\n");
            Write($"(function({name}) {{\n");

            var preValue = 0;
            for (var i = 0; i < node.Members.Count; i++)
            {
                var member = node.Members[i];
                var memeberName = node.Identifier.Text;
                var memberValue = member.EqualsValue != null
                    ? int.Parse(member.EqualsValue.Value.ToString())
                    : preValue + 1;
                preValue = memberValue;
                Write($"    {name}[{name}[\"{memeberName}\"] = {memberValue}] = \"{memeberName}\";");

                if (i != node.Members.Count - 1)
                    Write('\n');
            }

            Write($"}})({name} || ({name} = {{}}));\n");
        }

        private void EnumToTypeScript(EnumDeclarationSyntax node)
        {
            // enum name
            Write("enum ");
            VisitToken(node.Identifier);

            // members
            VisitToken(node.OpenBraceToken);

            for (var i = 0; i < node.Members.Count; i++)
            {
                var member = node.Members[i];
                VisitToken(member.Identifier);
                if (member.EqualsValue != null)
                {
                    Write(" = ");
                    Visit(member.EqualsValue.Value);
                }

                if (i != node.Members.Count - 1)
                    Write(",\n");
            }

            VisitToken(node.CloseBraceToken);
        }
    }
}