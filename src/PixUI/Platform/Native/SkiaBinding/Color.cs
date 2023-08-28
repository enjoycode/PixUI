#if !__WEB__
using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PixUI;

[JsonConverter(typeof(ColorJsonConverter))]
public readonly struct Color : IEquatable<Color>
{
    public static readonly Color Empty;

    public static Color FromArgb(byte alpha, byte red, byte green, byte blue) =>
        new(red, green, blue, alpha);

    private readonly uint color;

    public Color(uint value)
    {
        color = value;
    }

    public Color(byte red, byte green, byte blue, byte alpha = 255)
    {
        color = (uint)((alpha << 24) | (red << 16) | (green << 8) | blue);
    }

    public Color WithRed(byte red) => new Color(red, Green, Blue, Alpha);

    public Color WithGreen(byte green) => new Color(Red, green, Blue, Alpha);

    public Color WithBlue(byte blue) => new Color(Red, Green, blue, Alpha);

    public Color WithAlpha(byte alpha) => new Color(Red, Green, Blue, alpha);

    public byte Alpha => (byte)((color >> 24) & 0xff);
    public byte Red => (byte)((color >> 16) & 0xff);
    public byte Green => (byte)((color >> 8) & 0xff);
    public byte Blue => (byte)((color) & 0xff);

    public bool IsOpaque => Alpha == 0xFF;

    public static bool IsTransparentOrEmpty(in Color color) => color == Empty || color.Alpha == 0;

    public static bool IsTransparent(in Color color) => color.Alpha == 0;

    public float GetHue()
    {
        int r = Red;
        int g = Green;
        int b = Blue;
        byte minval = (byte)Math.Min(r, Math.Min(g, b));
        byte maxval = (byte)Math.Max(r, Math.Max(g, b));

        if (maxval == minval)
            return 0.0f;

        float diff = (float)(maxval - minval);
        float rnorm = (maxval - r) / diff;
        float gnorm = (maxval - g) / diff;
        float bnorm = (maxval - b) / diff;

        float hue = 0.0f;
        if (r == maxval)
            hue = 60.0f * (6.0f + bnorm - gnorm);
        if (g == maxval)
            hue = 60.0f * (2.0f + rnorm - bnorm);
        if (b == maxval)
            hue = 60.0f * (4.0f + gnorm - rnorm);
        if (hue > 360.0f)
            hue = hue - 360.0f;

        return hue;
    }

    public float GetSaturation()
    {
        byte minval = (byte)Math.Min(Red, Math.Min(Green, Blue));
        byte maxval = (byte)Math.Max(Red, Math.Max(Green, Blue));

        if (maxval == minval)
            return 0.0f;

        int sum = maxval + minval;
        if (sum > 255)
            sum = 510 - sum;

        return (float)(maxval - minval) / sum;
    }

    public float GetBrightness()
    {
        byte minval = Math.Min(Red, Math.Min(Green, Blue));
        byte maxval = Math.Max(Red, Math.Max(Green, Blue));

        return (float)(maxval + minval) / 510;
    }

    // public static SKColor FromHsv (float h, float s, float v, byte a = 255)
    // {
    // 	var colorf = SKColorF.FromHsv (h, s, v);
    //
    // 	// RGB results from 0 to 255
    // 	var r = colorf.Red * 255f;
    // 	var g = colorf.Green * 255f;
    // 	var b = colorf.Blue * 255f;
    //
    // 	return new SKColor ((byte)r, (byte)g, (byte)b, a);
    // }

    // public readonly void ToHsl (out float h, out float s, out float l)
    // {
    // 	// RGB from 0 to 255
    // 	var r = Red / 255f;
    // 	var g = Green / 255f;
    // 	var b = Blue / 255f;
    //
    // 	var colorf = new SKColorF (r, g, b);
    // 	colorf.ToHsl (out h, out s, out l);
    // }

    // public readonly void ToHsv (out float h, out float s, out float v)
    // {
    // 	// RGB from 0 to 255
    // 	var r = Red / 255f;
    // 	var g = Green / 255f;
    // 	var b = Blue / 255f;
    //
    // 	var colorf = new SKColorF (r, g, b);
    // 	colorf.ToHsv (out h, out s, out v);
    // }

    public readonly override string ToString() => $"#{Alpha:x2}{Red:x2}{Green:x2}{Blue:x2}";

    public readonly bool Equals(Color obj) => obj.color == color;

    public readonly override bool Equals(object other) => other is Color f && Equals(f);

    public static bool operator ==(Color left, Color right) => left.Equals(right);

    public static bool operator !=(Color left, Color right) => !left.Equals(right);

    public readonly override int GetHashCode() => color.GetHashCode();

    public static implicit operator Color(uint color) => new Color(color);

    public static explicit operator uint(Color color) => color.color;

    public static Color? Lerp(Color? a, Color? b, double t) => ColorUtils.Lerp(a, b, t);

    public static Color Parse(string hexString)
    {
        if (!TryParse(hexString, out var color))
            throw new ArgumentException("Invalid hexadecimal color string.", nameof(hexString));
        return color;
    }

    public static bool TryParse(string hexString, out Color color)
    {
        if (string.IsNullOrWhiteSpace(hexString))
        {
            // error
            color = Color.Empty;
            return false;
        }

        // clean up string
        hexString = hexString.Trim().ToUpperInvariant();
        if (hexString[0] == '#')
            hexString = hexString.Substring(1);

        var len = hexString.Length;
        if (len == 3 || len == 4)
        {
            byte a;
            // parse [A]
            if (len == 4)
            {
                if (!byte.TryParse(string.Concat(hexString[len - 4], hexString[len - 4]),
                        NumberStyles.HexNumber,
                        CultureInfo.InvariantCulture, out a))
                {
                    // error
                    color = Color.Empty;
                    return false;
                }
            }
            else
            {
                a = 255;
            }

            // parse RGB
            if (!byte.TryParse(string.Concat(hexString[len - 3], hexString[len - 3]),
                    NumberStyles.HexNumber,
                    CultureInfo.InvariantCulture, out var r) ||
                !byte.TryParse(string.Concat(hexString[len - 2], hexString[len - 2]),
                    NumberStyles.HexNumber,
                    CultureInfo.InvariantCulture, out var g) ||
                !byte.TryParse(string.Concat(hexString[len - 1], hexString[len - 1]),
                    NumberStyles.HexNumber,
                    CultureInfo.InvariantCulture, out var b))
            {
                // error
                color = Color.Empty;
                return false;
            }

            // success
            color = new Color(r, g, b, a);
            return true;
        }

        if (len == 6 || len == 8)
        {
            // parse [AA]RRGGBB
            if (!uint.TryParse(hexString, NumberStyles.HexNumber, CultureInfo.InvariantCulture,
                    out var number))
            {
                // error
                color = Color.Empty;
                return false;
            }

            // success
            color = (Color)number;

            // alpha was not provided, so use 255
            if (len == 6)
            {
                color = color.WithAlpha(255);
            }

            return true;
        }

        // error
        color = Color.Empty;
        return false;
    }
}

public sealed class ColorJsonConverter : JsonConverter<Color>
{
    public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var stringValue = reader.GetString()!;
        var intValue = uint.Parse(stringValue, NumberStyles.HexNumber);
        return new Color(intValue);
    }

    public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
    {
        var number = (uint)value;
        writer.WriteStringValue(number.ToString("X2"));
    }
}
#endif