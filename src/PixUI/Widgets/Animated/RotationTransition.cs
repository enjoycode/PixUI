using System;

namespace PixUI;

public sealed class RotationTransition : Transform
{
    private readonly Animation<float> _turns;

    public RotationTransition(Animation<float> turns) : base(Matrix4.CreateIdentity())
    {
        _turns = turns;
        _turns.ValueChanged += OnTurnChanged;
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        base.Layout(availableWidth, availableHeight);
        //根据子组件大小计算并初始化偏移量
        var originX = 0f;
        var originY = 0f;
        if (Child != null)
        {
            //TODO: 暂直接中心点
            originX = Child.W / 2;
            originY = Child.H / 2;
        }

        InitTransformAndOrigin(CalcTransform(), new Offset(originX, originY));
    }

    private Matrix4 CalcTransform()
    {
        var matrix = Matrix4.CreateIdentity();
        matrix.RotateZ((float)(_turns.Value * Math.PI * 2.0f));
        return matrix;
    }

    private void OnTurnChanged() => SetTransform(CalcTransform());

    public override void Dispose()
    {
        _turns.ValueChanged -= OnTurnChanged;
        base.Dispose();
    }
}