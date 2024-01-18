using System;
using AppKit;

namespace PixUI.Platform.Mac;

internal static class MacKeyCodeMap
{
    private static readonly Keys[] KeysMap =
    {
        /*0*/ Keys.A,
        /*1*/ Keys.S,
        /*2*/ Keys.D,
        /*3*/ Keys.F,
        /*4*/ Keys.H,
        /*5*/ Keys.G,
        /*6*/ Keys.Z,
        /*7*/ Keys.X,
        /*8*/ Keys.C,
        /*9*/ Keys.V,
        /*10*/ Keys.OemBackslash, //TODO:
        /*11*/ Keys.B,
        /*12*/ Keys.Q,
        /*13*/ Keys.W,
        /*14*/ Keys.E,
        /*15*/ Keys.R,
        /*16*/ Keys.Y,
        /*17*/ Keys.T,
        /*18*/ Keys.D1,
        /*19*/ Keys.D2,
        /*20*/ Keys.D3,
        /*21*/ Keys.D4,
        /*22*/ Keys.D6,
        /*23*/ Keys.D5,
        /*24*/ Keys.Oemplus,
        /*25*/ Keys.D9,
        /*26*/ Keys.D7,
        /*27*/ Keys.OemMinus,
        /*28*/ Keys.D8,
        /*29*/ Keys.D0,
        /*30*/ Keys.OemCloseBrackets,
        /*31*/ Keys.O,
        /*32*/ Keys.U,
        /*33*/ Keys.OemOpenBrackets,
        /*34*/ Keys.I,
        /*35*/ Keys.P,
        /*36*/ Keys.Return,
        /*37*/ Keys.L,
        /*38*/ Keys.J,
        /*39*/ Keys.Oemtilde,
        /*40*/ Keys.K,
        /*41*/ Keys.OemSemicolon,
        /*42*/ Keys.Oem7,
        /*43*/ Keys.Oemcomma,
        /*44*/ Keys.OemQuestion,
        /*45*/ Keys.N,
        /*46*/ Keys.M,
        /*47*/ Keys.OemPeriod,
        /*48*/ Keys.Tab,
        /*49*/ Keys.Space,
        /*50*/ Keys.Oem5,
        /*51*/ Keys.Back,
        /*52*/ Keys.Enter,
        /*53*/ Keys.Escape,
        /*54*/ Keys.RWin,
        /*55*/ Keys.LWin,
        /*56*/ Keys.LShiftKey,
        /*57*/ Keys.CapsLock,
        /*58*/ Keys.Menu, //LAlt
        /*59*/ Keys.LControlKey,
        /*60*/ Keys.RShiftKey,
        /*61*/ Keys.Menu, //RAlt
        /*62*/ Keys.RControlKey,
        /*63*/ Keys.RWin, //TODO:
        /*64*/ Keys.F17,
        /*65*/ Keys.Decimal, //KP_PERIOD
        /*66*/ Keys.None, //TODO
        /*67*/ Keys.Multiply,
        /*68*/ Keys.None, //TODO
        /*69*/ Keys.Add,
        /*70*/ Keys.None, //TODO
        /*71*/ Keys.NumLock, //TODO: NUMLOCKCLEAR
        /*72*/ Keys.VolumeUp,
        /*73*/ Keys.VolumeDown,
        /*74*/ Keys.VolumeMute,
        /*75*/ Keys.Divide,
        /*76*/ Keys.Enter,
        /*77*/ Keys.None, //TODO:
        /*78*/ Keys.OemMinus, //TODO:
        /*79*/ Keys.F18,
        /*80*/ Keys.F19,
        /*81*/ Keys.Oemplus, //TODO: KP_EQUALS
        /*82*/ Keys.NumPad0, //TODO:
        /*83*/ Keys.NumPad1,
        /*84*/ Keys.NumPad2,
        /*85*/ Keys.NumPad3,
        /*86*/ Keys.NumPad4,
        /*87*/ Keys.NumPad5,
        /*88*/ Keys.NumPad6,
        /*89*/ Keys.NumPad7,
        /*90*/ Keys.None,
        /*91*/ Keys.NumPad8,
        /*92*/ Keys.NumPad9,
        /*93*/ Keys.None, //TODO:INTERNATIONAL3, /* Cosmo_USB2ADB.c says "Yen (JIS)" */
        /*94*/ Keys.None, //TODO:INTERNATIONAL1, /* Cosmo_USB2ADB.c says "Ro (JIS)" */
        /*95*/ Keys.None, //TODO:/* Cosmo_USB2ADB.c says ", JIS only" */
        /*96*/ Keys.F5,
        /*97*/ Keys.F6,
        /*98*/ Keys.F7,
        /*99*/ Keys.F3,
        /*100*/ Keys.F8,
        /*101*/ Keys.F9,
        /*102*/ Keys.None, //TODO:/* Cosmo_USB2ADB.c says "Eisu" */
        /*103*/ Keys.F11,
        /*104*/ Keys.None, //TODO:/* Cosmo_USB2ADB.c says "Kana" */
        /*105*/ Keys.PrintScreen,
        /*106*/ Keys.F16,
        /*107*/ Keys.Scroll, //TODO: SCROLLLOCK
        /*108*/ Keys.None,
        /*109*/ Keys.F10,
        /*110*/ Keys.Apps,
        /*111*/ Keys.F12,
        /*112*/ Keys.None,
        /*113*/ Keys.Pause,
        /*114*/ Keys.Insert,
        /*115*/ Keys.Home,
        /*116*/ Keys.PageUp,
        /*117*/ Keys.Delete,
        /*118*/ Keys.F4,
        /*119*/ Keys.End,
        /*120*/ Keys.F2,
        /*121*/ Keys.PageDown,
        /*122*/ Keys.F1,
        /*123*/ Keys.Left,
        /*124*/ Keys.Right,
        /*125*/ Keys.Down,
        /*126*/ Keys.Up,
        /*127*/ Keys.Sleep //TODO:POWER
    };

    internal static Keys ConvertKeyCode(ushort keyCode, NSEventModifierMask modifierMask)
    {
        if (keyCode >= KeysMap.Length)
        {
            Console.WriteLine($"MacKeyCodeMap.ConvertKeyCode: out of range [{keyCode}]");
            return Keys.None;
        }

        var keyData = KeysMap[keyCode];

        if ((modifierMask & NSEventModifierMask.ShiftKeyMask) == NSEventModifierMask.ShiftKeyMask)
            keyData |= Keys.Shift;
        if ((modifierMask & NSEventModifierMask.ControlKeyMask) == NSEventModifierMask.ControlKeyMask)
            keyData |= Keys.Control;
        if ((modifierMask & NSEventModifierMask.AlternateKeyMask) == NSEventModifierMask.AlternateKeyMask)
            keyData |= Keys.Alt;
        if ((modifierMask & NSEventModifierMask.CommandKeyMask) == NSEventModifierMask.CommandKeyMask)
            keyData |= Keys.Meta;

        return keyData;
    }
}