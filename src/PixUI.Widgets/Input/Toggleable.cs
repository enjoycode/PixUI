using System;

namespace PixUI;

public abstract class Toggleable : Widget, IMouseRegion
{
    protected Toggleable()
    {
        MouseRegion = new MouseRegion(() => Cursors.Hand);
        MouseRegion.PointerTap += OnTap;
    }

    protected State<bool?> _value = null!;
    private bool _triState;
    protected AnimationController _positionController = null!;
    public MouseRegion MouseRegion { get; }

    public event Action<bool?>? ValueChanged;

    protected void InitState(State<bool?> value, bool tristate)
    {
        _triState = tristate;
        _value = Bind(value, OnValueChanged);
        _positionController = new AnimationController(100, value.Value != null && value.Value.Value ? 1 : 0);
        _positionController.ValueChanged += OnPositionValueChanged;
    }

    private void OnTap(PointerEvent e)
    {
        //TODO: skip on readonly

        //只切换true与false，中间状态只能程序改变
        if (_value.Value == null || _value.Value == false)
            _value.Value = true;
        else
            _value.Value = false;
    }

    private void AnimateToValue()
    {
        if (_triState)
        {
            if (_value.Value == null || _value.Value == true)
            {
                _positionController.SetValue(0);
                _positionController.Forward();
            }
            else
                _positionController.Reverse();
        }
        else
        {
            if (_value.Value != null && _value.Value == true)
                _positionController.Forward();
            else
                _positionController.Reverse();
        }
    }

    private void OnPositionValueChanged()
    {
        Invalidate(InvalidAction.Repaint);
    }

    private void OnValueChanged(State state)
    {
        ValueChanged?.Invoke(_value.Value);
        AnimateToValue();
    }
}