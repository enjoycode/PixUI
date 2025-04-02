using System;

namespace PixUI.Dynamic;

/// <summary>
/// 动态组件的属性定义
/// </summary>
public sealed class DynamicPropertyMeta
{
    public DynamicPropertyMeta(string name, Type runtimeType, bool allowNull, bool initSetter = false,
        DynamicValue? initValue = null, string? editorName = null)
    {
        Name = name;
        AllowNull = allowNull;
        IsInitSetter = initSetter;
        InitValue = initValue;
        EditorName = editorName;
        //判断是否状态类型
        if (typeof(State).IsAssignableFrom(runtimeType))
        {
            if (runtimeType.IsGenericType && runtimeType.GetGenericTypeDefinition() == typeof(State<>))
            {
                ValueType = runtimeType.GenericTypeArguments[0];
                IsState = true;
            }
            else
            {
                throw new NotSupportedException("Only State<> supported");
            }
        }
        else
        {
            ValueType = runtimeType;
            IsState = false;
        }
    }

    public readonly string Name;
    public readonly Type ValueType;
    public readonly bool AllowNull;
    public readonly bool IsInitSetter;
    public readonly bool IsState;
    public readonly string? EditorName; //指定属性编辑器的名称

    /// <summary>
    /// 仅用于设计时创建实例时的初始化值
    /// </summary>
    public readonly DynamicValue? InitValue;

    /// <summary>
    /// ValueType是否可空值类型
    /// </summary>
    public bool IsNullableValueType => ValueType is { IsValueType: true, IsGenericType: true } &&
                                       ValueType.GetGenericTypeDefinition() == typeof(Nullable<>);

    private object? GetRuntimeValue(in DynamicValue source, IDynamicContext dynamicContext)
    {
        if (source.From == ValueSource.Const)
        {
            //已经在读取时转换类型为ValueType
            if (IsState && source.Value != null) //TODO: check Nullable of value
            {
                var rxType = typeof(RxValue<>).MakeGenericType(ValueType);
                return Activator.CreateInstance(rxType, source.Value);
            }

            return source.Value;
        }

        if (source.From == ValueSource.State)
        {
            return dynamicContext.GetPrimitiveState(source.StateName);
        }

        throw new NotImplementedException();
    }

    public void SetRuntimeValue(DynamicWidgetMeta meta, Widget target, in DynamicValue propertyValue,
        IDynamicContext dynamicContext)
    {
        //TODO: emit 优化，暂用反射

        var runtimeValue = GetRuntimeValue(propertyValue, dynamicContext);
        var propInfo = meta.WidgetType.GetProperty(Name);
        propInfo!.SetValue(target, runtimeValue);
    }
}