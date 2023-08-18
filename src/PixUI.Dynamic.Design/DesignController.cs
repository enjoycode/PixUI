using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;

namespace PixUI.Dynamic.Design;

public sealed class DesignController
{
    /// <summary>
    /// 设计画布缩放百分比
    /// </summary>
    public readonly State<int> Zoom = 100;

    public DesignElement RootElement { get; internal set; } = null!;

    /// <summary>
    /// 当前工具箱选择的项
    /// </summary>
    public DynamicWidgetMeta? CurrentToolboxItem { get; internal set; }

    #region ====DesignElement Selection====

    private readonly List<DesignElement> _selection = new();

    public event Action? SelectionChanged;

    public DesignElement? FirstSelected => _selection.Count > 0 ? _selection[0] : null;

    internal void Select(DesignElement element)
    {
        if (_selection.Count == 1 && ReferenceEquals(_selection[0], element)) return;

        _selection.ForEach(o => o.IsSelected = false);
        _selection.Clear();

        _selection.Add(element);
        element.IsSelected = true;

        OnSelectionChanged();
    }

    internal void OnSelectionChanged()
    {
        SelectionChanged?.Invoke();
        RootElement.Invalidate(InvalidAction.Repaint); //暂在这里全部刷新，待实现选择装饰器后移除
    }

    #endregion

    #region ====Json Serialization====

    public void Load(byte[] json)
    {
#if DEBUG
        var ts = Stopwatch.GetTimestamp();
#endif
        DesignElement? rootElement = null;
        var reader = new Utf8JsonReader(json);
        while (reader.Read())
        {
            if (reader.TokenType != JsonTokenType.PropertyName) continue;

            var propName = reader.GetString();
            switch (propName)
            {
                case "View":
                    rootElement = ReadView(ref reader);
                    break;
            }
        }

        if (rootElement != null)
        {
            var parent = (SingleChildWidget)RootElement.Parent!;
            parent.Child = rootElement;
            RootElement = rootElement;
            parent.Invalidate(InvalidAction.Relayout);
        }

        Select(RootElement); // always select root element

#if DEBUG
        Log.Debug($"加载耗时: {Stopwatch.GetElapsedTime(ts).TotalMilliseconds}ms");
#endif
    }

    private DesignElement ReadView(ref Utf8JsonReader reader)
    {
        var element = new DesignElement(this);

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject) break;
            if (reader.TokenType != JsonTokenType.PropertyName) continue;

            var propName = reader.GetString()!;
            switch (propName)
            {
                case nameof(DynamicWidgetData.Type):
                    reader.Read();
                    var meta = DynamicWidgetManager.GetByName(reader.GetString()!);
                    element.ChangeMeta(meta, false);
                    break;
                case nameof(DynamicWidgetData.CtorArgs):
                    ReadCtorArgs(element, ref reader);
                    break;
                case nameof(DynamicWidgetData.Properties):
                    ReadProperties(element, ref reader);
                    break;
                case "Child":
                    ReadChild(element, ref reader);
                    break;
                case "Children":
                    throw new NotImplementedException();
                    break;
            }
        }

        return element;
    }

    private static void ReadCtorArgs(DesignElement element, ref Utf8JsonReader reader)
    {
        var meta = element.Meta!;
        var data = element.Data;
        if (meta.CtorArgs == null || meta.CtorArgs.Length == 0) throw new InvalidOperationException();

        var args = new DynamicValue[meta.CtorArgs.Length];
        reader.Read(); //[
        for (var i = 0; i < args.Length; i++)
        {
            args[i] = DynamicValue.Read(ref reader, meta.CtorArgs[i].Value);
        }

        reader.Read(); //]

        data.CtorArgs = args;
        element.ChangeChild(null, meta.MakeInstance(data.CtorArgs!));
    }

    private static void ReadProperties(DesignElement element, ref Utf8JsonReader reader)
    {
        var meta = element.Meta!;
        var data = element.Data;

        if (element.Target == null)
            element.ChangeChild(null, meta.MakeDefaultInstance());

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject) break;
            if (reader.TokenType != JsonTokenType.PropertyName) continue;

            var prop = new PropertyValue { Name = reader.GetString()! };
            var propMeta = meta.GetPropertyMeta(prop.Name);
            prop.Value = DynamicValue.Read(ref reader, propMeta.Value);

            data.AddPropertyValue(prop);
            element.SetPropertyValue(prop);
        }
    }

    private void ReadChild(DesignElement element, ref Utf8JsonReader reader)
    {
        if (element.Target == null)
            element.ChangeChild(null, element.Meta!.MakeDefaultInstance());

        var childElement = ReadView(ref reader);
        element.AddChild(childElement);
    }

    #endregion
}