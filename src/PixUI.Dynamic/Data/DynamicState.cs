using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace PixUI.Dynamic;

public interface IDynamicState
{
    void WriteTo(Utf8JsonWriter writer);
}

public interface IDynamicValueState : IDynamicState
{
    void ReadFrom(ref Utf8JsonReader reader, DynamicState state);

    /// <summary>
    /// 获取设计时的值
    /// </summary>
    object? GetDesignValue(IDynamicContext ctx);

    /// <summary>
    /// 获取运行时状态
    /// </summary>
    State GetRuntimeState(IDynamicContext ctx, DynamicState state);
}

public interface IDynamicDataSetState : IDynamicState
{
    /// <summary>
    /// 数据集变更事件，用于通知绑定的组件刷新数据
    /// </summary>
    event Action DataSetValueChanged;

    void ReadFrom(ref Utf8JsonReader reader);

    ValueTask<object?> GetRuntimeDataSet(IDynamicContext dynamicContext);
}

public enum DynamicStateType
{
    DataSet,
    Int,
    String,
    DateTime,
    Float,
    Double,
}

public sealed class DynamicState
{
    public string Name { get; set; } = null!;
    public DynamicStateType Type { get; set; }
    public IDynamicState? Value { get; set; }
    public bool AllowNull { get; set; }

    /// <summary>
    /// 根据状态类型(DynamicStateType)获取相应的反射值类型(Type)
    /// </summary>
    public Type GetValueStateValueType()
    {
        if (Type == DynamicStateType.DataSet)
            throw new NotSupportedException("Only for value state");

        var valueType = Type switch
        {
            DynamicStateType.Int => typeof(int),
            DynamicStateType.String => typeof(string),
            DynamicStateType.DateTime => typeof(DateTime),
            DynamicStateType.Float => typeof(float),
            DynamicStateType.Double => typeof(double),
            _ => throw new NotImplementedException()
        };
        if (AllowNull && Type != DynamicStateType.String)
            valueType = typeof(Nullable<>).MakeGenericType(valueType);
        return valueType;
    }

    /// <summary>
    /// 根据属性值类型获取相应的DynamicStateType
    /// </summary>
    public static DynamicStateType GetStateTypeByValueType(DynamicPropertyMeta propertyMeta, out bool allowNull)
    {
        var valueType = propertyMeta.ValueType;
        allowNull = false;
        if (propertyMeta.IsNullableValueType)
        {
            valueType = valueType.GenericTypeArguments[0];
            allowNull = true;
        }

        return GetStateTypeByValueType(valueType);
    }

    public static DynamicStateType GetStateTypeByValueType(Type noneNullableValueType)
    {
        DynamicStateType stateType;
        if (noneNullableValueType == typeof(string))
            stateType = DynamicStateType.String;
        else if (noneNullableValueType == typeof(int))
            stateType = DynamicStateType.Int;
        else if (noneNullableValueType == typeof(DateTime))
            stateType = DynamicStateType.DateTime;
        else if (noneNullableValueType == typeof(float))
            stateType = DynamicStateType.Float;
        else if (noneNullableValueType == typeof(double))
            stateType = DynamicStateType.Double;
        else
            throw new NotImplementedException($"ValueType: {noneNullableValueType} to DynamicStateType");

        return stateType;
    }
}