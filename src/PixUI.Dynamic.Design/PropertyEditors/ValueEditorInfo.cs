using System;

namespace PixUI.Dynamic.Design;

public sealed class ValueEditorInfo
{
    public ValueEditorInfo(string name, bool isDefault, Type valueType,
        Func<DesignElement, DynamicPropertyMeta, State> propertyStateMaker,
        Func<State, Widget> editorMaker)
    {
        Name = name;
        IsDefault = isDefault;
        ValueType = valueType;
        PropertyStateMaker = propertyStateMaker;
        EditorMaker = editorMaker;
    }

    public readonly string Name;
    public bool IsDefault { get; internal set; }
    public readonly Type ValueType;
    public readonly Func<DesignElement, DynamicPropertyMeta, State> PropertyStateMaker;
    public readonly Func<State, Widget> EditorMaker;
}