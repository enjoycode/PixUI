using System;

namespace PixUI.Dynamic.Design;

internal sealed class ValueEditorInfo
{
    public ValueEditorInfo(string name, bool isDefault, Type valueType,
        Func<State, DesignElement, Widget> editorMaker,
        Func<PropertyEditor, State> propertyStateMaker
    )
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
    public readonly Func<PropertyEditor, State> PropertyStateMaker;
    public readonly Func<State, DesignElement, Widget> EditorMaker;
}