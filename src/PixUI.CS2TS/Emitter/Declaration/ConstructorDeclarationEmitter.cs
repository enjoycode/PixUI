using System;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    partial class Emitter
    {
        public override void VisitConstructorDeclaration(ConstructorDeclarationSyntax node)
        {
            WriteLeadingTrivia(node);
            WriteModifiers(node.Modifiers);
            // 暂不支持: 判断是否通过TSRenameAttribute标记为工厂方法
            // var name = node.Identifier.Text;
            // var toFactory = TryRenameDeclaration(node.AttributeLists, ref name);
            // Write(toFactory ? $"static {name}" : "constructor");
            Write("constructor");

            // parameters
            VisitToken(node.ParameterList.OpenParenToken);
            VisitSeparatedList(node.ParameterList.Parameters);
            VisitToken(node.ParameterList.CloseParenToken);

            if (node.ExpressionBody != null)
                throw new NotImplementedException();

            // body
            if (node.Initializer != null)
                WriteTrailingTrivia(node.Initializer);
            VisitToken(node.Body!.OpenBraceToken);
            EnterBlock(node.Body);

            // try super call for class
            var typeDeclaration = (TypeDeclarationSyntax)node.Parent!;
            if (typeDeclaration is ClassDeclarationSyntax classDeclaration)
                EmitSuperCall(classDeclaration, node);

            // body
            foreach (var statement in node.Body!.Statements)
            {
                Visit(statement);
            }

            LeaveBlock(node.Body.Statements.Count > 0 &&
                       node.Body.Statements.Last() is ReturnStatementSyntax);
            VisitToken(node.Body!.CloseBraceToken);
        }

        private void EmitSuperCall(ClassDeclarationSyntax parent, ConstructorDeclarationSyntax node)
        {
            var baseClass = parent.GetBaseType(SemanticModel);
            if (baseClass == null) return;

            if (node.Initializer != null)
            {
                if (node.Initializer.ThisOrBaseKeyword.Text == "this")
                    throw new NotSupportedException();

                WriteLeadingWhitespaceOnly(node);
                Write("\tsuper(");
                VisitSeparatedList(node.Initializer.ArgumentList.Arguments);
                Write(");\n");
            }
            else
            {
                WriteLeadingWhitespaceOnly(node);
                Write("\tsuper();\n");
            }
        }
    }
}