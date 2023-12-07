using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace PixUI;

/// <summary>
/// 通过取值及赋值委托代理的状态
/// </summary>
public sealed class RxProxy<T> : State<T>
{
    /// <summary>
    /// 新建状态代理, 注意代理目标值变更不会通知状态变更
    /// </summary>
    public RxProxy(Func<T> getter, Action<T>? setter = null, bool autoNotify = true)
    {
        _getter = getter;
        if (setter == null || !autoNotify)
            _setter = setter;
        else
            _setter = v =>
            {
                if (EqualityComparer<T>.Default.Equals(_getter(), v))
                    return;
                setter(v);
                NotifyValueChanged();
            };
    }

    /// <summary>
    /// 新建状态代理，监听目标值变更后通知状态变更
    /// </summary>
    public RxProxy(INotifyPropertyChanged obj, string propertyName, Func<T> getter, Action<T>? setter = null)
    {
        _getter = getter;
        _setter = setter;
        _target = obj;
        _propertyName = propertyName;
        _target.PropertyChanged += OnTargetPropertyChanged;
    }

    /// <summary>
    /// 新建使用Emit生成属性Getter及Setter的简化的状态代理
    /// </summary>
    public RxProxy(INotifyPropertyChanged obj, string propertyName)
    {
        var getter = EmitGetter(obj, propertyName);
        _getter = () => getter(obj);

        //TODO: maybe check property readonly
        var setter = EmitSetter(obj, propertyName);
        _setter = v => setter(obj, v);

        _target = obj;
        _propertyName = propertyName;
        _target.PropertyChanged += OnTargetPropertyChanged;
    }

    private readonly Func<T> _getter;
    private readonly Action<T>? _setter;
    private readonly INotifyPropertyChanged? _target;
    private readonly string? _propertyName;

    public override bool Readonly => _setter == null;

    private void OnTargetPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == _propertyName)
            NotifyValueChanged();
    }

    private static Func<INotifyPropertyChanged, T> EmitGetter(INotifyPropertyChanged obj, string propertyName)
    {
        var objType = obj.GetType();
        var propInfo = objType.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);
        if (propInfo == null) throw new Exception($"Can't find property: {objType.Name}.{propertyName}");

        var objArg = Expression.Parameter(typeof(INotifyPropertyChanged));
        var convertedObj = Expression.Convert(objArg, objType);

        return Expression.Lambda<Func<INotifyPropertyChanged, T>>(
            Expression.MakeMemberAccess(convertedObj, propInfo), objArg
        ).Compile();
    }

    private static Action<INotifyPropertyChanged, T> EmitSetter(INotifyPropertyChanged obj, string propertyName)
    {
        var objType = obj.GetType();
        var propInfo = objType.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);
        if (propInfo == null) throw new Exception($"Can't find property: {objType.Name}.{propertyName}");

        var objArg = Expression.Parameter(typeof(INotifyPropertyChanged));
        var convertedObj = Expression.Convert(objArg, objType);
        var valArg = Expression.Parameter(typeof(T));
        var memberAccess = Expression.MakeMemberAccess(convertedObj, propInfo);

        return Expression.Lambda<Action<INotifyPropertyChanged, T>>(
            Expression.Assign(memberAccess, valArg), objArg, valArg
        ).Compile();
    }

    public override object? BoxedValue => Value;

    public override T Value
    {
        get => _getter();
        set
        {
            if (_setter == null) throw new NotSupportedException("状态值只读");
            _setter(value);
        }
    }

    public override void Dispose()
    {
        base.Dispose();
        if (_target != null)
            _target.PropertyChanged -= OnTargetPropertyChanged;
    }
}

public static class RxProxyExtensions
{
    public static RxProxy<T> Observe<T>(this INotifyPropertyChanged obj, string propertyName) =>
        new(obj, propertyName);

    public static RxProxy<T> Observe<T>(this INotifyPropertyChanged obj, string propertyName,
        Func<T> getter, Action<T>? setter = null) =>
        new(obj, propertyName, getter, setter);

    public static RxProxy<T> Observe<T>(this INotifyPropertyChanged obj,
        Expression<Func<T>> getter, Action<T>? setter = null)
    {
        if (getter.Body is not MemberExpression member)
            throw new NotSupportedException();
        return new RxProxy<T>(obj, member.Member.Name, getter.Compile(), setter);
    }
}