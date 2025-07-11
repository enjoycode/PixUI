#if !__WEB__
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PixUI;

[JsonConverter(typeof(ColorJsonConverter))]
public readonly struct Color : IEquatable<Color>
{
    public static readonly Color Empty = 0;

    public static Color FromArgb(byte alpha, byte red, byte green, byte blue) =>
        new(red, green, blue, alpha);

    public static Color FromHsl(double hue, double saturation = 1, double lightness = 0.5, byte alpha = 255)
    {
        // 规范化输入值（使用Math.Clamp确保范围正确）
        hue = Math.Min(Math.Max(hue % 360, 0), 360); // 色相0-360度
        if (hue < 0) hue += 360;

        saturation = Math.Min(Math.Max(saturation, 0), 1); // 饱和度0-1
        lightness = Math.Min(Math.Max(lightness, 0), 1); // 亮度0-1

        // 无颜色情况（饱和度为0）
        if (saturation == 0)
        {
            byte l = (byte)(lightness * 255);
            return Color.FromArgb(alpha, l, l, l);
        }

        // 计算临时值q
        double q = lightness < 0.5
            ? lightness * (1 + saturation)
            : lightness + saturation - (lightness * saturation);

        double p = 2 * lightness - q;
        double hk = hue / 360.0;

        // 计算三个颜色通道
        double[] tc = { hk + 1.0 / 3, hk, hk - 1.0 / 3 };
        double[] colors = new double[3];

        for (int i = 0; i < 3; i++)
        {
            // 调整色相值到0-1范围
            if (tc[i] < 0) tc[i] += 1;
            if (tc[i] > 1) tc[i] -= 1;

            // 计算各通道颜色值
            if (tc[i] < 1.0 / 6)
            {
                colors[i] = p + ((q - p) * 6 * tc[i]);
            }
            else if (tc[i] < 1.0 / 2)
            {
                colors[i] = q;
            }
            else if (tc[i] < 2.0 / 3)
            {
                colors[i] = p + ((q - p) * 6 * (2.0 / 3 - tc[i]));
            }
            else
            {
                colors[i] = p;
            }

            // 确保最终值在0-1范围内（二次保护）
            colors[i] = Math.Min(Math.Max(colors[i], 0), 1);
        }

        // 转换为RGB并四舍五入
        return Color.FromArgb(
            alpha,
            (byte)(colors[0] * 255 + 0.5),
            (byte)(colors[1] * 255 + 0.5),
            (byte)(colors[2] * 255 + 0.5));
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
        byte minval = (byte)Math.Min(r, Math.Min(g, b));
        byte maxval = (byte)Math.Max(r, Math.Max(g, b));

        if (maxval == minval)
            return 0.0f;

        float diff = maxval - minval;
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
        byte minval = Math.Min(Red, Math.Min(Green, Blue));
        byte maxval = Math.Max(Red, Math.Max(Green, Blue));

        return (float)(maxval + minval) / 510;
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