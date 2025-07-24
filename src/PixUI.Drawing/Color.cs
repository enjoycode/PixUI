#if !__WEB__
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PixUI;

[JsonConverter(typeof(ColorJsonConverter))]
public readonly struct Color : IEquatable<Color>
{
    public static readonly Color Empty = 0;
    private const double OneSixth = 1.0 / 6.0;
    private const double OneThird = 1.0 / 3.0;
    private const double TwoThird = 2.0 / 3.0;

    public static Color FromArgb(byte alpha, byte red, byte green, byte blue) =>
        new(red, green, blue, alpha);

    public static Color FromHsl(double hue, double saturation = 1, double lightness = 0.5, byte alpha = 255)
    {
        double h = hue / 360;
        double l = lightness;
        double s = saturation;
        double a = alpha;
        double m2 = 0, m1 = 0;

        if (s == 0.0)
        {
            return FromArgb(
                (byte)Clamp(a * 255, 0.0, 255, 5),
                (byte)Clamp(l * 255, 0.0, 255, 5),
                (byte)Clamp(l * 255, 0.0, 255, 5),
                (byte)Clamp(l * 255, 0.0, 255, 5));
        }

        if (l <= 0.5)
            m2 = l * (1.0 + s);
        else
            m2 = l + s - (l * s);

        m1 = (2.0 * l) - m2;

        double r = ConvertValue(m1, m2, h + OneThird);
        double g = ConvertValue(m1, m2, h);
        double b = ConvertValue(m1, m2, h - OneThird);

        return FromArgb(
            (byte)Clamp(a * 255, 0.0, 255, 5),
            (byte)Clamp(r * 255, 0.0, 255, 5),
            (byte)Clamp(g * 255, 0.0, 255, 5),
            (byte)Clamp(b * 255, 0.0, 255, 5));
    }

    private static double ConvertValue(double m1, double m2, double hue)
    {
        hue = CustomMod(hue);

        if (hue < OneSixth)
            return m1 + (((m2 - m1) * hue) * 6.0);

        if (hue < 0.5)
            return m2;

        if (hue < TwoThird)
            return m1 + (((m2 - m1) * (TwoThird - hue)) * 6.0);

        return m1;
    }

    private static double CustomMod(double number)
    {
        if (number > 0)
        {
            return number - Math.Floor(number);
        }
        else if (number < 0)
        {
            double abs = Math.Abs(number);
            return 1 - (abs - Math.Floor(abs));
        }

        return 0;
    }

    private static double Clamp(double value, double minimum, double maximum, int precision)
    {
        var clampedValue = value > maximum ? maximum : value;
        if (clampedValue < minimum) clampedValue = minimum;

        return Math.Round(clampedValue, precision);
    }

    private readonly uint _color;

    public Color(uint value)
    {
        _color = value;
    }

    public Color(byte red, byte green, byte blue, byte alpha = 255)
    {
        _color = (uint)((alpha << 24) | (red << 16) | (green << 8) | blue);
    }

    public Color WithRed(byte red) => new Color(red, Green, Blue, Alpha);

    public Color WithGreen(byte green) => new Color(Red, green, Blue, Alpha);

    public Color WithBlue(byte blue) => new Color(Red, Green, blue, Alpha);

    public Color WithAlpha(byte alpha) => new Color(Red, Green, Blue, alpha);

    public byte Alpha => (byte)((_color >> 24) & 0xff);
    public byte Red => (byte)((_color >> 16) & 0xff);
    public byte Green => (byte)((_color >> 8) & 0xff);
    public byte Blue => (byte)((_color) & 0xff);

    public bool IsOpaque => Alpha == 0xFF;

    public static bool IsTransparentOrEmpty(in Color color) => color == Empty || color.Alpha == 0;

    public static bool IsTransparent(in Color color) => color.Alpha == 0;

    public float GetHue()
    {
        int r = Red;
        int g = Green;
        int b = Blue;
        byte minVal = (byte)Math.Min(r, Math.Min(g, b));
        byte maxVal = (byte)Math.Max(r, Math.Max(g, b));

        if (maxVal == minVal)
            return 0.0f;

        float diff = maxVal - minVal;
        float rnorm = (maxVal - r) / diff;
        float gnorm = (maxVal - g) / diff;
        float bnorm = (maxVal - b) / diff;

        float hue = 0.0f;
        if (r == maxVal)
            hue = 60.0f * (6.0f + bnorm - gnorm);
        if (g == maxVal)
            hue = 60.0f * (2.0f + rnorm - bnorm);
        if (b == maxVal)
            hue = 60.0f * (4.0f + gnorm - rnorm);
        if (hue > 360.0f)
            hue = hue - 360.0f;

        return hue;
    }

    public float GetSaturation()
    {
        byte minval = Math.Min(Red, Math.Min(Green, Blue));
        byte maxval = Math.Max(Red, Math.Max(Green, Blue));

        if (maxval == minval)
            return 0.0f;

        int sum = maxval + minval;
        if (sum > 255)
            sum = 510 - sum;

        return (float)(maxval - minval) / sum;
    }

    public float GetBrightness()
    {
        byte minVal = Math.Min(Red, Math.Min(Green, Blue));
        byte maxVal = Math.Max(Red, Math.Max(Green, Blue));

        return (float)(maxVal + minVal) / 510;
    }

    public override string ToString() => $"#{Alpha:x2}{Red:x2}{Green:x2}{Blue:x2}";

    public bool Equals(Color obj) => obj._color == _color;

    public override bool Equals(object? other) => other is Color f && Equals(f);

    public static bool operator ==(Color left, Color right) => left.Equals(right);

    public static bool operator !=(Color left, Color right) => !left.Equals(right);

    public override int GetHashCode() => _color.GetHashCode();

    public static implicit operator Color(uint color) => new(color);

    public static explicit operator uint(Color color) => color._color;

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
            color = number;

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