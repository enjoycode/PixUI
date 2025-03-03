using System;

namespace PixUI.Dynamic.Design;

public static class DesignSettings
{
    public static Func<DesignElement, DynamicEventMeta, Dialog>? GetEventEditor;
    
    public static Func<DesignController, DynamicState, Dialog>? GetDataSourceStateEditor;

    public static Func<IDynamicDataSourceState>? MakeDataSourceState;

    public static Func<DynamicState, Dialog>? GetValueStateEditor;

    public static Func<IDynamicValueState>? MakeValueState;
}