using System;

namespace PixUI.Dynamic.Design;

public sealed class DateEditor: ValueEditorBase
{
    public DateEditor(State<DateTime?> date, DesignElement element) : base(element)
    {
        Child = new DatePicker(date);
    }
}