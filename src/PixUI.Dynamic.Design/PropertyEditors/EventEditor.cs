using System;
using System.Collections.Generic;

namespace PixUI.Dynamic.Design;

public delegate Widget EventEditorCreator(DesignElement element, DynamicEventMeta eventMeta, IEventAction eventAction);

public sealed class EventEditor : Widget
{
    private static readonly Dictionary<string, EventEditorCreator> _editors = new();

    public static void Register(string actionName, EventEditorCreator creator)
    {
        _editors.Add(actionName, creator);
    }

    public static bool TryGetEditor(string actionName, out EventEditorCreator creator)
    {
        if (_editors.TryGetValue(actionName, out var c))
        {
            creator = c;
            return true;
        }

        creator = null!;
        return false;
    }

    public EventEditor(DesignElement element, DynamicEventMeta eventMeta)
    {
        _element = element;
        _eventMeta = eventMeta;

        _actionName = new RxProxy<string>(() =>
            element.Data.TryGetEventValue(eventMeta.Name, out var value) ? value.Action.ActionName : "...");
        _editButton = new Button(_actionName) { Width = 1000, OnTap = _ => OnEdit() };
        _editButton.Parent = this;
        _deleteButton = new Button(icon: MaterialIcons.Clear)
        {
            Style = ButtonStyle.Transparent,
            Width = _buttonSize,
            Height = _buttonSize,
            OnTap = _ => OnDelete()
        };
        _deleteButton.Parent = this;
    }

    private readonly DesignElement _element;
    private readonly DynamicEventMeta _eventMeta;

    private static readonly State<float> _buttonSize = 20f;
    private readonly State<string> _actionName;
    private readonly Button _editButton;
    private readonly Button _deleteButton;

    private async void OnEdit()
    {
        var dlg = DesignSettings.GetEventEditor?.Invoke(_element, _eventMeta);
        if (dlg == null) return;
        var res = await dlg.ShowAsync();
        if (res == DialogResult.OK)
            _actionName.NotifyValueChanged();
    }

    private void OnDelete()
    {
        _actionName.NotifyValueChanged();
    }

    #region ====Widget Overrides====

    public override void VisitChildren(Func<Widget, bool> action)
    {
        if (action(_editButton)) return;
        action(_deleteButton);
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        var maxSize = CacheAndGetMaxSize(availableWidth, availableHeight);

        _editButton.Layout(maxSize.Width - _buttonSize.Value * 2, maxSize.Height);
        _editButton.SetPosition(0, 0);
        SetSize(maxSize.Width, _editButton.H);

        _deleteButton.Layout(_buttonSize.Value, _buttonSize.Value);
        _deleteButton.SetPosition(maxSize.Width - _buttonSize.Value, (H - _deleteButton.H) / 2f);
    }

    #endregion
}