using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    public partial class Emitter
    {
        internal void VisitSeparatedList<T>(SeparatedSyntaxList<T> list) where T : SyntaxNode
        {
            foreach (var item in list.GetWithSeparators())
            {
                if (item.IsToken)
                    VisitToken(item.AsToken());
                else
                    Visit(item.AsNode());
            }
        }

        public override void VisitUsingDirective(UsingDirectiveSyntax node)
        {
            //do nothing now.
        }

        #region ====Declaration====

        public override void VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
        {
            //暂不处理Namespace
            foreach (var member in node.Members)
            {
                Visit(member);
            }
        }

        public override void VisitFileScopedNamespaceDeclaration(FileScopedNamespaceDeclarationSyntax node)
        {
            //暂不处理Namespace
            foreach (var member in node.Members)
            {
                Visit(member);
            }
        }

        public override void VisitTypeParameter(TypeParameterSyntax node)
        {
            Write(node.Identifier.Text);
        }

        public override void VisitConversionOperatorDeclaration( ConversionOperatorDeclarationSyntax node)
        {
            //TODO: 转为静态方法
        }

        #endregion

        #region ====Statement====

        public override void VisitUsingStatement(UsingStatementSyntax node)
        {
            throw new NotImplementedException("UsingStatement");
        }

        #endregion

        #region ====Expression====

        public override void VisitArgumentList(ArgumentListSyntax node) =>
            VisitSeparatedList(node.Arguments);

        public override void VisitBaseExpression(BaseExpressionSyntax node)
        {
            WriteLeadingTrivia(node);
            Write("super");
        }

        public override void VisitInterpolatedStringText(InterpolatedStringTextSyntax node) =>
            Write(node.TextToken.Text);

        public override void VisitInterpolation(InterpolationSyntax node)
        {
            Write("${");
            Visit(node.Expression);
            Write('}');
        }

        public override void VisitCheckedExpression(CheckedExpressionSyntax node)
        {
            Visit(node.Expression);
        }

        public override void VisitDefaultExpression(DefaultExpressionSyntax node)
            => throw new NotSupportedException("default(xxx) is not supported");

        public override void VisitTupleExpression(TupleExpressionSyntax node)
        {
            Write('[');
            VisitSeparatedList(node.Arguments);
            Write(']');
        }

        public override void VisitPostfixUnaryExpression(PostfixUnaryExpressionSyntax node)
        {
            if (node.Kind() == SyntaxKind.SuppressNullableWarningExpression && ToJavaScript)
            {
                node.Operand.Accept(this);
                WriteTrailingTrivia(node);
                return;
            }

            base.VisitPostfixUnaryExpression(node);
        }

        #endregion

        #region ====Token & Trivia====

        private bool _disableVisitTrailingTrivia;
        private bool _disableVisitLeadingTrivia;

        internal void DisableVisitLeadingTrivia() => _disableVisitLeadingTrivia = true;

        internal void EnableVisitLeadingTrivia() => _disableVisitLeadingTrivia = false;

        internal void DisableVisitTrailingTrivia() => _disableVisitTrailingTrivia = true;

        internal void EnableVisitTrailingTrivia() => _disableVisitTrailingTrivia = false;

        public override void VisitToken(SyntaxToken token)
        {
            VisitLeadingTrivia(token);
            Write(token.Text);
            VisitTrailingTrivia(token);
        }

        public override void VisitLeadingTrivia(SyntaxToken token)
        {
            if (!_disableVisitLeadingTrivia)
                base.VisitLeadingTrivia(token);
        }

        public override void VisitTrailingTrivia(SyntaxToken token)
        {
            if (!_disableVisitTrailingTrivia)
                base.VisitTrailingTrivia(token);
        }

        public override void VisitTrivia(SyntaxTrivia trivia)
        {
            switch (trivia.Kind())
            {
                case SyntaxKind.EndOfLineTrivia:
                    Write('\n');
                    break;
                case SyntaxKind.WhitespaceTrivia:
                    Write(' ', trivia.Span.Length);
                    break;
                case SyntaxKind.SingleLineCommentTrivia:
                    Write(trivia.ToString()); // @ts-ignore需要
                    break;
                case SyntaxKind.SingleLineDocumentationCommentTrivia:
                    break;
                case SyntaxKind.MultiLineCommentTrivia:
                case SyntaxKind.MultiLineDocumentationCommentTrivia:
                    break;
                case SyntaxKind.IfDirectiveTrivia:
                case SyntaxKind.ElseDirectiveTrivia:
                case SyntaxKind.EndIfDirectiveTrivia:
                case SyntaxKind.DisabledTextTrivia:
                    break;
                default:
                    //TODO:
                    //throw new Exception(trivia.Kind().ToString());
                    break;
            }
        }

        #endregion
    }
}