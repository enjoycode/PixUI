using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    partial class Emitter
    {
        public override void VisitDelegateDeclaration(DelegateDeclarationSyntax node)
        {
            if (node.IsTSType(out _))
                return;

            WriteLeadingTrivia(node);

            var needExport = node.Modifiers.Any(m =>
                m.Kind() == SyntaxKind.PublicKeyword || m.Kind() == SyntaxKind.InternalKeyword);
            if (needExport)
                Write("export ");

            Write("type ");
            
            var name = node.Identifier.Text;
            TryRenameDeclaration(node.AttributeLists, ref name);
            //TODO: 添加检查类型名称 Translator.AddType();
            Write(name);
            
            WriteTypeParameters(node.TypeParameterList, node.ConstraintClauses);

            Write(" = ");

            Write('(');
            VisitSeparatedList(node.ParameterList.Parameters);
            Write(") => ");
            Visit(node.ReturnType);

            VisitToken(node.SemicolonToken);
        }
    }
}