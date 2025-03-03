using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;

namespace PixUI.Dynamic.Design;

partial class DesignController
{
    public void Write(Utf8JsonWriter writer)
    {
        writer.WriteStartObject();

        //Background
        WriteBackground(writer);

        //State
        WriteStates(writer);

        //Root
        writer.WritePropertyName("Root");
        WriteWidget(writer, RootElement);

        writer.WriteEndObject();
    }

    private void WriteBackground(Utf8JsonWriter writer)
    {
        if (Background == null) return;

        writer.WritePropertyName(nameof(Background));
        JsonSerializer.Serialize(writer, Background);
    }

    private void WriteStates(Utf8JsonWriter writer)
    {
        var states = StatesController.DataSource;
        if (states == null || states.Count == 0)
            return;

        writer.WritePropertyName("State");
        writer.WriteStartObject();

        foreach (var state in states)
        {
            writer.WritePropertyName(state.Name);
            writer.WriteStartObject();

            //Type
            writer.WriteString(nameof(DynamicState.Type), state.Type.ToString());
            //AllowNull
            if (state.Type != DynamicStateType.DataTable)
                writer.WriteBoolean(nameof(DynamicState.AllowNull), state.AllowNull);
            //Value
            if (state.Value == null)
            {
                writer.WriteNull(nameof(DynamicState.Value));
            }
            else
            {
                writer.WritePropertyName(nameof(DynamicState.Value));
                state.Value.WriteTo(writer);
            }

            writer.WriteEndObject();
        }

        writer.WriteEndObject();
    }

    private static void WriteWidget(Utf8JsonWriter writer, DesignElement element)
    {
        var meta = element.Meta;
        var data = element.Data;
        if (meta == null)
        {
            //element is a placeholder.
            writer.WriteStartObject();
            writer.WriteNull("Type");
            writer.WriteNumber("Width", element.W);
            writer.WriteNumber("Height", element.H);
            writer.WriteEndObject();
            return;
        }

        writer.WriteStartObject();
        // Type
        writer.WriteString("Type", meta.Name);

        // Properties
        if (data.Properties != null)
        {
            foreach (var property in data.Properties)
            {
                var propMeta = meta.GetPropertyMeta(property.Name);
                writer.WritePropertyName(property.Name);
                property.Value.Write(writer, propMeta);
            }
        }

        // Events
        if (data.Events is { Count: > 0 })
        {
            writer.WritePropertyName("Events");
            writer.WriteStartObject();
            foreach (var eventValue in data.Events)
            {
                writer.WritePropertyName(eventValue.Name);
                writer.WriteStartObject();
                writer.WriteString("Handler", eventValue.Action.ActionName);
                eventValue.Action.WriteProperties(writer);
                writer.WriteEndObject();
            }

            writer.WriteEndObject();
        }

        // Slots
        if (element is { IsContainer: true, Child: not null })
        {
            var childs = GetAllChildrenElements(element);
            var slots = childs.GroupBy(c => c.SlotName);
            foreach (var group in slots)
            {
                var slot = meta.GetSlot(group.Key);
                writer.WritePropertyName(slot.PropertyName);

                if (slot.ContainerType == ContainerType.MultiChild)
                {
                    writer.WriteStartArray();
                    foreach (var childElement in group)
                    {
                        WriteWidget(writer, childElement);
                    }

                    writer.WriteEndArray();
                }
                else
                {
                    WriteWidget(writer, group.First());
                }
            }
        }

        writer.WriteEndObject();
    }

    public static IEnumerable<DesignElement> GetAllChildrenElements(DesignElement parentElement)
    {
        if (parentElement.Meta == null)
            return Array.Empty<DesignElement>();

        var list = new List<DesignElement>();
        var start = parentElement.Meta.IsReversedWrapElement ? parentElement : parentElement.Child;

        start?.VisitChildren(child =>
        {
            var childElement = GetChildElement(child);
            if (childElement != null)
                list.Add(childElement);

            return false;
        });

        return list;
    }

    private static DesignElement? GetChildElement(Widget child)
    {
        if (child is DesignElement designElement)
            return designElement;

        DesignElement? next = null;
        child.VisitChildren(nextChild =>
        {
            next = nextChild as DesignElement;
            return true;
        });

        return next;
    }

    public void Load(byte[] json)
    {
#if DEBUG
        var ts = Stopwatch.GetTimestamp();
#endif

        var parent = (SingleChildWidget)RootElement.Parent!;
        DesignElement? rootElement = null;
        var reader = new Utf8JsonReader(json);
        while (reader.Read())
        {
            if (reader.TokenType != JsonTokenType.PropertyName) continue;

            var propName = reader.GetString();
            switch (propName)
            {
                case "Background":
                    ReadBackground(ref reader);
                    break;
                case "State":
                    ReadStates(ref reader);
                    break;
                case "Root":
                    rootElement = (DesignElement)ReadWidget(ref reader, string.Empty);
                    break;
            }
        }

        if (rootElement != null)
        {
            parent.Child = rootElement;
            RootElement = rootElement;
            parent.Relayout();
        }

        Select(RootElement); // always select root element

#if DEBUG
        Log.Debug($"加载耗时: {Stopwatch.GetElapsedTime(ts).TotalMilliseconds}ms");
#endif
    }

    private void ReadBackground(ref Utf8JsonReader reader)
    {
        Background = JsonSerializer.Deserialize<DynamicBackground>(ref reader);
    }

    private void ReadStates(ref Utf8JsonReader reader)
    {
        var states = new List<DynamicState>();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject) break;
            if (reader.TokenType != JsonTokenType.PropertyName) continue;

            var propName = reader.GetString()!;
            ReadState(ref reader, propName, states);
        }

        StatesController.DataSource = states;
    }

    private static void ReadState(ref Utf8JsonReader reader, string name, List<DynamicState> states)
    {
        var state = new DynamicState { Name = name };
        reader.Read(); //{
        reader.Read(); //Type prop
        reader.Read(); //Type value
        state.Type = Enum.Parse<DynamicStateType>(reader.GetString()!);
        if (state.Type == DynamicStateType.DataTable)
        {
            reader.Read(); //Value prop
            var peekReader = reader;
            if (!(peekReader.Read() && peekReader.TokenType == JsonTokenType.Null))
            {
                var ds = DesignSettings.MakeTableState!();
                ds.ReadFrom(ref reader);
                state.Value = ds;
            }
            else
            {
                reader.Read(); //Value null
            }
        }
        else
        {
            //AllowNull
            reader.Read(); //AllowNull prop
            reader.Read(); //AllowNull value
            state.AllowNull = reader.GetBoolean();

            //Value
            reader.Read(); //Value prop
            var peekReader = reader;
            if (!(peekReader.Read() && peekReader.TokenType == JsonTokenType.Null))
            {
                var vs = DesignSettings.MakeValueState!();
                vs.ReadFrom(ref reader, state);
                state.Value = vs;
            }
            else
            {
                reader.Read(); //Value null
            }
        }

        reader.Read(); //}

        states.Add(state);
    }

    private Widget ReadWidget(ref Utf8JsonReader reader, string slotName)
    {
        Widget result = null!;
        DesignElement element = null!;
        DynamicWidgetMeta meta = null!;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject) break;
            if (reader.TokenType != JsonTokenType.PropertyName) continue;

            var propName = reader.GetString()!;
            if (propName == "Type")
            {
                reader.Read();
                var type = reader.GetString();
                if (string.IsNullOrEmpty(type))
                {
                    //element is a placeholder
                    reader.Read();
                    reader.Read();
                    var width = reader.GetSingle();
                    reader.Read();
                    reader.Read();
                    var height = reader.GetSingle();
                    result = element = new DesignElement(this, slotName) { Width = width, Height = height };
                    continue;
                }

                meta = DynamicWidgetManager.GetByName(type);
                if (meta.IsReversedWrapElement)
                {
                    result = meta.CreateInstance();
                    element = new DesignElement(this, meta, slotName);
                    meta.Slots![0].SetChild(result, element);
                }
                else
                {
                    result = element = new DesignElement(this, slotName);
                    element.ChangeMeta(meta, false);
                    element.Child = meta.CreateInstance();
                }
            }
            else if (propName == "Events")
            {
                ReadEvents(ref reader, element);
            }
            else if (meta.IsSlot(propName, out var childSlot))
            {
                if (childSlot!.ContainerType == ContainerType.MultiChild)
                {
                    ReadWidgetArray(ref reader, element.Target!, childSlot);
                }
                else if (childSlot.ContainerType == ContainerType.SingleChildReversed)
                {
                    var child = ReadWidget(ref reader, childSlot.PropertyName);
                    element.Child = child;
                }
                else
                {
                    var child = ReadWidget(ref reader, childSlot.PropertyName);
                    childSlot.SetChild(element.Target!, child);
                }
            }
            else
            {
                var prop = new PropertyValue { Name = propName };
                var propMeta = meta.GetPropertyMeta(prop.Name);
                prop.Value = DynamicValue.Read(ref reader, propMeta);

                element.Data.AddPropertyValue(prop);
                element.SetPropertyValue(prop);
            }
        }

        return result;
    }

    private static void ReadEvents(ref Utf8JsonReader reader, DesignElement element)
    {
        reader.Read(); //{

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject) break;

            var eventName = reader.GetString()!;
            var eventAction = ReadEventAction(ref reader);
            element.Data.SetEventValue(eventName, eventAction);
        }
    }

    private static IEventAction ReadEventAction(ref Utf8JsonReader reader)
    {
        reader.Read(); //{
        reader.Read(); // Handler prop
        Debug.Assert(reader.GetString() == "Handler");
        reader.Read(); // Handler value
        var handler = reader.GetString()!;
        //根据类型创建实例
        var res = DynamicWidgetManager.EventActionManager.Create(handler);
        res.ReadProperties(ref reader);
        return res;
    }

    private void ReadWidgetArray(ref Utf8JsonReader reader, Widget parent, ContainerSlot childrenSlot)
    {
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray) break;
            if (reader.TokenType != JsonTokenType.StartObject) continue;

            var child = ReadWidget(ref reader, childrenSlot.PropertyName);
            childrenSlot.AddChild(parent, child);
        }
    }
}