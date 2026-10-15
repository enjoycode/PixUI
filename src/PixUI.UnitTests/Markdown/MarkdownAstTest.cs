using System;
using System.IO;
using System.Linq;
using System.Text;
using Markdig;
using Markdig.Extensions.Tables;
using Markdig.Renderers.Normalize;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using NUnit.Framework;

namespace PixUI.UnitTests.Markdown;

public class MarkdownAstTest
{
    [Test(Description = "修改AST测试")]
    public void ModifyAstTest()
    {
        var pipeline = new MarkdownPipelineBuilder().UsePipeTables().Build();

        var doc = Markdig.Markdown.Parse("**粗体**正常", pipeline);
        var firstBold = doc.Descendants<EmphasisInline>().First();
        var literal = firstBold.Descendants<LiteralInline>().First();
        literal.ReplaceBy(new LiteralInline(literal.Content + "附加"));

        // Table
        var table = new Table();
        doc.Add(table);
        table.ColumnDefinitions.Add(new TableColumnDefinition() { Alignment = TableColumnAlign.Left });
        table.ColumnDefinitions.Add(new TableColumnDefinition() { Alignment = TableColumnAlign.Center });
        table.ColumnDefinitions.Add(new TableColumnDefinition() { Alignment = TableColumnAlign.Right });

        var rowHeader = new TableRow() { IsHeader = true };
        table.Add(rowHeader);
        for (int i = 0; i < table.ColumnDefinitions.Count; i++)
        {
            var cell = new TableCell();
            var cellText = new LiteralInline($"Header {i}");
            var cellContainer = new ContainerInline();
            cellContainer.AppendChild(cellText);
            var para = new ParagraphBlock() { Inline = cellContainer };
            cell.Add(para);
            rowHeader.Add(cell);
        }

        var row = new TableRow();
        table.Add(row);
        for (int i = 0; i < table.ColumnDefinitions.Count; i++)
        {
            var cell = new TableCell();
            var cellText = new LiteralInline($"Cell {i}");
            var cellContainer = new ContainerInline();
            cellContainer.AppendChild(cellText);
            var para = new ParagraphBlock() { Inline = cellContainer };
            cell.Add(para);
            row.Add(cell);
        }
        
        //Write to new markdown
        var sb = new StringBuilder();
        using var writer = new StringWriter(sb);
        var normalizeRender = new NormalizeRenderer(writer, null);
        pipeline.Setup(normalizeRender);
        normalizeRender.Render(doc);
        Console.WriteLine(sb.ToString());

        Console.WriteLine("\n====Html====");
        Console.WriteLine(doc.ToHtml());
    }
}