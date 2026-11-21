using System;

namespace PixUI;

[Flags]
public enum SourceGenClassType
{
    None = 0,
    StylableWidget = 1,
    BindableWidget = 2,
    StyleObject = 4,
    BaseStyleObject = 8,
}