using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace PixUI.Dynamic;

public interface IDynamicStateValue
{
    void WriteTo(Utf8JsonWriter writer);

    void ReadFrom(ref Utf8JsonReader reader, DynamicState state);
}

/// <summary>
/// 基元类型状态值
/// </summary>
public interface IDynamicPrimitive : IDynamicStateValue
{
    /// <summary>
    /// 获取设计时的值
    /// </summary>
    object? GetDesignValue(IDynamicContext ctx);

    /// <summary>
    /// 获取运行时状态
    /// </summary>
    State GetRuntimeState(IDynamicContext ctx, DynamicState state);
}

/// <summary>
/// 数据表状态值
/// </summary>
public interface IDynamicDataTable : IDynamicStateValue
{
    /// <summary>
    /// 数据变更事件，用于通知绑定的组件刷新数据或重置相关配置
    /// </summary>
    event Action<bool> DataChanged;

    ValueTask<object?> GetRuntimeState(IDynamicContext dynamicContext);
}

/// <summary>
/// 数据行状态值
/// </summary>
public interface IDynamicDataRow : IDynamicStateValue { }

public enum DynamicStateType
{
    DataTable,
    DataRow,
    Int,
    String,
    DateTime,
    Float,
    Double,
}

/// <summary>
/// 设计时的状态
/// </summary>
public sealed class DynamicState
{
    public string Name { get; init; } = null!;
    public DynamicStateType Type { get; set; }
    public IDynamicStateValue? Value { get; set; }
    public bool AllowNull { get; set; }

    /// <summary>
    /// 根据状态类型(DynamicStateType)获取相应的运行时类型(Type)
    /// </summary>
    public Type GetTypeOfPrimitiveState()
    {
        if (Type is DynamicStateType.DataTable or DynamicStateType.DataRow)
            throw new NotSupportedException("Only for primitive value state");

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