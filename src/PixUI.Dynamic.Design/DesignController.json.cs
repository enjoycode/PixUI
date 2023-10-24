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
        if (meta == null) return; //skip 

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

        // Slots
        if (element.IsContainer && element.Child != null)
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
                    ReadBackgournd(ref reader);
                    break;
                case "State":
                    ReadStates(ref reader);
                    break;
                case "Root":
                    rootElement = (DesignElement)ReadWidget(ref reader, null, string.Empty);
                    break;
            }
        }

        if (rootElement != null)
        {
            parent.Child = rootElement;
            RootElement = rootElement;
            parent.Invalidate(InvalidAction.Relayout);
        }

        Select(RootElement); // always select root element

#if DEBUG
        Log.Debug($"加载耗时: {Stopwatch.GetElapsedTime(ts).TotalMilliseconds}ms");
#endif
    }

    private void ReadBackgournd(ref Utf8JsonReader reader)
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

    private static void ReadState(ref Utf8JsonReader reader, string name, IList<DynamicState> states)
    {
        reader.Read(); //{
        reader.Read(); //Type prop
        reader.Read();
        var type = Enum.Parse<DynamicStateType>(reader.GetString()!);
        reader.Read(); //Value prop

        if (type == DynamicStateType.DataSet)
        {
            var ds = DesignSettings.MakeDataSetSettings!();
            ds.ReadFrom(ref reader);
            var state = new DynamicState() { Name = name, Type = type, Value = ds };
            states.Add(state);
        }
        else
        {
            throw new NotImplementedException();
        }

        reader.Read(); //}
    }

    private Widget ReadWidget(ref Utf8JsonReader reader, DynamicWidgetMeta? parentMeta, string slotName)
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
                meta = DynamicWidgetManager.GetByName(reader.GetString()!);
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
                throw new NotImplementedException();
            }
            else if (meta.IsSlot(propName, out var childSlot))
            {
                if (childSlot!.ContainerType == ContainerType.MultiChild)
                {
                    ReadWidgetArray(ref reader, meta, element.Target!, childSlot);
                }
                else if (childSlot.ContainerType == ContainerType.SingleChildReversed)
                {
                    var child = ReadWidget(ref reader, meta, childSlot!.PropertyName);
                    element.Child = child;
                }
                else
                {
                    var child = ReadWidget(ref reader, meta, childSlot!.PropertyName);
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

    private void ReadWidgetArray(ref Utf8JsonReader reader, DynamicWidgetMeta parentMeta, Widget parent,
        ContainerSlot childrenSlot)
    {
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray) break;
            if (reader.TokenType != JsonTokenType.StartObject) continue;

            var child = ReadWidget(ref reader, parentMeta, childrenSlot.PropertyName);
            childrenSlot.AddChild(parent, child);
        }
    }
}