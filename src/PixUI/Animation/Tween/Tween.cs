namespace PixUI;

public abstract class Tween<T> : Animatable<T>
{
    protected readonly T? Begin;
    protected readonly T? End;

    public Tween(T? begin = default, T? end = default)
    {
        Begin = begin;
        End = end;
    }

    public abstract T Lerp(double t);

    public sealed override T Transform(double t)
    {
        if (t == 0.0) return Begin!;
        if (t == 1.0) return End!;
        return Lerp(t)!;
    }
}

public sealed class FloatTween : Tween<float>
{
    public FloatTween(float begin, float end) : base(begin, end) { }

    public override float Lerp(double t) => (float)(Begin! + (End! - Begin!) * t);
}

public sealed class ColorTween : Tween<Color>
{
    public ColorTween(Color begin, Color end) : base(begin, end) { }

    public override Color Lerp(double t) => Color.Lerp(Begin, End, t)!.Value;
}

public sealed class OffsetTween : Tween<Offset>
{
    public OffsetTween(Offset begin, Offset end) : base(begin, end) { }

    public override Offset Lerp(double t) => Offset.Lerp(Begin, End, t)!.Value;
}