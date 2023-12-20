using System.Collections.Generic;
using System.Linq;

namespace PixUI.Dynamic.Design;

public sealed class StringArrayEditor : ValueEditorBase
{
    public StringArrayEditor(State<string[]?> state, DesignElement element) : base(element)
    {
        _state = state;

        Child = new Button("...") { Width = float.MaxValue, OnTap = OnTap };
    }

    private readonly State<string[]?> _state;

    private async void OnTap(PointerEvent _)
    {
        //编辑副本
        var list = new List<string>();
        if (_state.Value is { Length: > 0 })
            list.AddRange(_state.Value.ToList());

        var dlg = new StringArrayDialog(list);
        var dlgResult = await dlg.ShowAsync();
        if (dlgResult != DialogResult.OK) return;

        _state.Value = list.ToArray();
    }
}