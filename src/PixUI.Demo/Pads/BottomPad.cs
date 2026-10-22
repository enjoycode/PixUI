using Microsoft.CodeAnalysis;

namespace PixUI.Demo;

internal sealed class BottomPad : View
{
    public BottomPad(PlaygroundContext context)
    {
        Child = new DataGrid<Diagnostic>(context.ProblemsController)
            .AddIconColumn(string.Empty,
                p => p.Severity == DiagnosticSeverity.Error ? MaterialIcons.Dangerous : MaterialIcons.Report,
                26,
                (p, _) => p.Severity == DiagnosticSeverity.Error ? _errorCellStyle : _warnCellStyle)
            .AddTextColumn("Position", GetLocation, 180)
            .AddTextColumn("Message", p => p.GetMessage())
            .AddButtonColumn("Goto",
                (p, _) => new Button(icon: MaterialIcons.NextPlan)
                {
                    Style = ButtonStyle.Transparent,
                    Shape = ButtonShape.Pills,
                    FontSize = 20,
                    OnTap = _ => context.GotoProblem(p)
                }, 80);
    }

    private static string GetLocation(Diagnostic diagnostic)
    {
        var span = diagnostic.Location.GetMappedLineSpan();
        return
            $"[{span.StartLinePosition.Line + 1}, {span.StartLinePosition.Character}] - [{span.EndLinePosition.Line + 1}, {span.EndLinePosition.Character}]";
    }

    private readonly CellStyle _errorCellStyle = new() { TextColor = Colors.Red };
    private readonly CellStyle _warnCellStyle = new() { TextColor = 0xFFFFC763 };
}