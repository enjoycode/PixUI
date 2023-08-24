using System;
using System.Diagnostics;
using System.Text.Json;

namespace PixUI.Dynamic.Design;

partial class DesignController
{
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
        element.Child = meta.MakeInstance(data.CtorArgs!);
    }

    private static void ReadProperties(DesignElement element, ref Utf8JsonReader reader)
    {
        var meta = element.Meta!;
        var data = element.Data;

        if (element.Target == null)
            element.Child = meta.MakeDefaultInstance();

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
            element.Child = element.Meta!.MakeDefaultInstance();

        var childElement = ReadView(ref reader);
        element.AddChild(childElement);
    }
}