using System;

namespace PixUI.Dynamic.Design;

public static class DesignSettings
{
    public static Func<DesignController, DynamicState, Dialog>? GetDataSetStateEditor;

    public static Func<IDynamicDataSetState>? MakeDataSetState;

    public static Func<DynamicState, Dialog>? GetValueStateEditor;

    public static Func<IDynamicValueState>? MakeValueState;
}