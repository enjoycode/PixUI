namespace PixUI;

/// <summary>
/// A widget that applies a transformation before painting its child.
/// </summary>
public class Transform : SingleChildWidget
{
    private Matrix4 _transform;
    private Offset? _origin;
    private bool _clipBounds; //裁剪范围,并只能在本组件范围内HitTest

    public Transform(Matrix4 transform, Offset? origin = null, bool clipBounds = true)
    {
        SetTransform(transform);
        Origin = origin;
        _clipBounds = clipBounds;
    }

    /// <summary>
    /// The origin of the coordinate system (relative to the upper left corner of
    /// this) in which to apply the matrix.
    /// </summary>
    public Offset? Origin
    {
        get => _origin;
        set
        {
            if (_origin == value) return;
            _origin = value;
            Repaint();
        }
    }

    /// <summary>
    /// 初始化，不激发重绘
    /// </summary>
    protected void InitTransformAndOrigin(Matrix4 value, Offset? origin = null)
    {
        _transform = value;
        _origin = origin;
    }

    /// <summary>
    /// The matrix to transform the child by during painting.
    /// The provided value is copied on assignment.
    /// </summary>
    protected void SetTransform(Matrix4 value)
    {
        if (_transform == value) return;

        _transform = value;
        Repaint();
    }

    internal Matrix4 EffectiveTransform
    {
        get
        {
            if (_origin == null /* && resolvedAlignment == null */)
                return _transform;
            var result = Matrix4.CreateIdentity();
            if (_origin != null)
                result.Translate(_origin.Value.Dx, _origin.Value.Dy);

            result.Multiply(_transform);

            if (_origin != null)
                result.Translate(-_origin.Value.Dx, -_origin.Value.Dy);

            return result;
        }
    }

    protected internal override bool HitTest(float x, float y, HitTestResult result)
    {
        if (Child == null) return false;

        if (_clipBounds && !ContainsPoint(x, y))
            return false;

        var effectiveTransform = EffectiveTransform;
        //TODO: short path for effectiveTransform is Identity

        // The provided paint `transform` (which describes the transform from the
        // child to the parent in 3D) is processed by
        // [RemovePerspectiveTransform] to remove the
        // perspective component and inverted before it is used to transform
        // `position` from the coordinate system of the parent to the system of the child.
        var transform = Matrix4.TryInvert(MatrixUtils.RemovePerspectiveTransform(effectiveTransform));
        if (transform == null)
            return false; // Objects are not visible on screen and cannot be hit-tested.

        var transformed = MatrixUtils.TransformPoint(transform.Value, x, y);
        //不要加入 result.Add(this, effectiveTransform);
        var oldTransform = result.ConcatTransform(transform.Value, X, Y);
        var hitChild = Child.HitTest(transformed.Dx, transformed.Dy, result);
        if (hitChild)
        {
            if (_clipBounds)
                result.ConcatRestrictedBounds(this);
        }
        else
        {
            result.RestoreTransform(oldTransform);
        }

        return hitChild;
    }

    protected internal override void BeforePaint(Canvas canvas, bool onlyTransform = false,
        IDirtyArea? dirtyArea = null)
    {
        if (Child == null) return;

        canvas.Save();
        canvas.Translate(X, Y);
        if (_clipBounds)
            canvas.ClipRect(Rect.FromLTWH(0, 0, W, H), ClipOp.Intersect, false);

        canvas.Concat(EffectiveTransform); //canvas.Transform(EffectiveTransform);
    }

    protected internal override void AfterPaint(Canvas canvas)
    {
        if (Child == null) return;
        canvas.Restore();
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        if (Child == null) return;

        PaintChildren(canvas, area);
    }
}