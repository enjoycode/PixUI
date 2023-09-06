using System;

namespace PixUI.Dynamic.Design;

public static class DesignSettings
{
    public static Func<DynamicState, Dialog>? GetDataSetEditor;

    public static Func<IDynamicStateValue>? MakeDataSetSettings;
}