using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    partial class Emitter
    {
        public override void VisitBinaryExpression(BinaryExpressionSyntax node)
        {
            //判断不支持的表达式 someNullable ?? throw new Exception()
            if (node.Kind() == SyntaxKind.CoalesceExpression &&
                (node.Left is ThrowExpressionSyntax || node.Right is ThrowExpressionSyntax))
                throw new NotSupportedException($"不支持CoalesceExpression抛异常 at File: {node.SyntaxTree.FilePath}");
            
            var opKind = node.OperatorToken.Kind();
            //特殊处理 obj is string
            if (opKind == SyntaxKind.IsKeyword)
            {
                WriteIsExpression(node.Left, node.Right);
                return;
            }

            //判断是否Override的操作符
            var symbol = SemanticModel.GetSymbolInfo(node).Symbol;
            if (symbol is IMethodSymbol { MethodKind: MethodKind.UserDefinedOperator } methodSymbol
                && node.Right.Kind() != SyntaxKind.NullLiteralExpression &&
                node.Left.Kind() != SyntaxKind.NullLiteralExpression)
            {
                EmitUserDefinedOperator(node, opKind, methodSymbol);
                return;
            }

            Visit(node.Left);
            VisitToken(node.OperatorToken);
            Visit(node.Right);
        }

        private void EmitUserDefinedOperator(BinaryExpressionSyntax node, SyntaxKind opKind, IMethodSymbol symbol)
        {
            //特殊处理 == 或 !=, TODO:另考虑判断两者是否Nullable,非Nullable不需要特殊处理
            var isEquality = opKind == SyntaxKind.EqualsEqualsToken || opKind == SyntaxKind.ExclamationEqualsToken;
            if (isEquality)
            {
                AddUsedModule("System");
                Write(opKind == SyntaxKind.EqualsEqualsToken
                    ? "System.OpEquality"
                    : "System.OpInequality");
            }
            else
            {
                WriteTypeSymbol(symbol.ContainingType, !node.InSameSourceFile(symbol));
                Write('.');

                Write(symbol.Name); //eg: op_Subtraction
            }

            Write('(');
            DisableVisitTrailingTrivia();
            Visit(node.Left);
            EnableVisitTrailingTrivia();
            Write(", ");
            Visit(node.Right);
            Write(')');
        }
    }
}