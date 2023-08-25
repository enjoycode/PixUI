using System;

namespace PixUI.Dynamic.Design;

internal sealed class ValueEditorInfo
{
    public ValueEditorInfo(string name, bool isDefault, Type valueType,
        Func<State, Widget> editorMaker,
        // Func<DesignElement, DynamicCtorArgMeta, int, State> ctorArgStateMaker,
        Func<DesignElement, DynamicPropertyMeta, State> propertyStateMaker
    )
    {
        Name = name;
        IsDefault = isDefault;
        ValueType = valueType;
        PropertyStateMaker = propertyStateMaker;
        // CtorArgStateMaker = ctorArgStateMaker;
        EditorMaker = editorMaker;
    }

    public readonly string Name;
    public bool IsDefault { get; internal set; }
    public readonly Type ValueType;
    // public readonly Func<DesignElement, DynamicCtorArgMeta, int, State> CtorArgStateMaker;
    public readonly Func<DesignElement, DynamicPropertyMeta, State> PropertyStateMaker;
    public readonly Func<State, Widget> EditorMaker;
}