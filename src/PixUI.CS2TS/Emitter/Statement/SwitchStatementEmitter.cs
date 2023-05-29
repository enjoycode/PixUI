using System;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    partial class Emitter
    {
        public override void VisitSwitchStatement(SwitchStatementSyntax node)
        {
            var hasPattern = node.Sections
                .SelectMany(t => t.Labels)
                .Any(l => l is CasePatternSwitchLabelSyntax);
            if (hasPattern)
            {
                EmitToPattern(node);
                return;
            }

            VisitToken(node.SwitchKeyword);
            VisitToken(node.OpenParenToken);
            Visit(node.Expression);
            VisitToken(node.CloseParenToken);
            VisitToken(node.OpenBraceToken);

            foreach (var section in node.Sections)
            {
                Visit(section);
            }

            VisitToken(node.CloseBraceToken);
        }

        private void EmitToPattern(SwitchStatementSyntax node)
        {
            WriteLeadingTrivia(node);

            Write("match(");
            Visit(node.Expression);
            Write(")\n");

            //var hasDefault = false;
            foreach (var section in node.Sections)
            {
                if (section.Labels.Count > 1)
                    throw new NotImplementedException("SwitchPattern with multi label");

                WriteLeadingWhitespaceOnly(node);
                Write('\t');

                var label = section.Labels[0];
                if (label is CasePatternSwitchLabelSyntax casePattern)
                {
                    if (casePattern.Pattern is not DeclarationPatternSyntax declaration)
                        throw new NotImplementedException();
                    if (casePattern.WhenClause != null)
                        throw new NotImplementedException();

                    //使用.when不使用.with(instanceOf(XXX))是方便重用WriteIsExpression逻辑
                    Write(".when(t => ");
                    WriteIsExpression("t", declaration.Type);
                    Write(", (");
                    Visit(declaration.Designation);
                    Write(": ");
                    Visit(declaration.Type);
                    Write(") => {");
                    EmitSectionStatements(section);
                    Write("})\n");
                }
                else if (label is CaseSwitchLabelSyntax caseNormal)
                {
                    //TODO:
                }
                else if (label is DefaultSwitchLabelSyntax defaultLabel)
                {
                    //hasDefault = true;
                    Write(".otherwise(() => {");
                    EmitSectionStatements(section);
                    Write("})\n");
                }
                else
                {
                    throw new NotSupportedException($"Switch with label: {label.GetType()}");
                }
            }
        }

        private void EmitSectionStatements(SwitchSectionSyntax section)
        {
            for (var i = 0; i < section.Statements.Count; i++)
            {
                var statement = section.Statements[i];

                if (i == section.Statements.Count - 1 && statement is BreakStatementSyntax)
                    break;

                if (i == 0 && statement is BlockSyntax block)
                {
                    new BlockEmitter(true).Emit(this, block);
                }
                else
                {
                    Visit(statement);
                }
            }
        }
    }
}