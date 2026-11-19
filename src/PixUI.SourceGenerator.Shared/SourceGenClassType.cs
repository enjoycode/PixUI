using System;

namespace PixUI;

[Flags]
public enum SourceGenClassType
{
    None = 0,
    StyleObject = 1,
    StylableWidget = 2,
    BindableWidget = 4,
}