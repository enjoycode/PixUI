using System;

namespace PixUI;

public abstract class Toggleable : Widget, IMouseRegion
{
    protected Toggleable()
    {
        MouseRegion = new MouseRegion(() => Cursors.Hand);
        MouseRegion.PointerTap += OnTap;
    }

    protected State<bool?> Value = null!;
    private bool _triState;
    protected AnimationController PositionController = null!;
    public MouseRegion MouseRegion { get; }

    public event Action<bool?>? ValueChanged;

    protected void InitState(State<bool?> value, bool tristate)
    {
        _triState = tristate;
        Bind(ref Value!, value, OnValueChanged);
        PositionController = new AnimationController(100, value.Value != null && value.Value.Value ? 1 : 0);
        PositionController.ValueChanged += OnPositionValueChanged;
    }

    private void OnTap(PointerEvent e)
    {
        //TODO: skip on readonly

        //只切换true与false，中间状态只能程序改变
        if (Value.Value == null || Value.Value == false)
            Value.Value = true;
        else
            Value.Value = false;
    }

    private void AnimateToValue()
    {
        if (_triState)
        {
            if (Value.Value == null || Value.Value == true)
            {
                PositionController.SetValue(0);
                PositionController.Forward();
            }
            else
                PositionController.Reverse();
        }
        else
        {
            if (Value.Value != null && Value.Value == true)
                PositionController.Forward();
            else
                PositionController.Reverse();
        }
    }

    private void OnPositionValueChanged()
    {
        Invalidate(InvalidAction.Repaint);
    }

    private void OnValueChanged(State state)
    {
        ValueChanged?.Invoke(Value.Value);
        AnimateToValue();
    }
}