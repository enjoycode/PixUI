using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PixUI.Dynamic;

public enum ContainerType
{
    SingleChild,

    /// <summary>
    /// 用于如Expanded, Positioned等特例，反向向上包装
    /// </summary>
    SingleChildReversed,
    MultiChild,
}

public sealed class ContainerSlot
{
    public ContainerSlot(string propertyName, ContainerType type)
    {
        PropertyName = propertyName;
        ContainerType = type;
    }

    public readonly string PropertyName;
    public readonly ContainerType ContainerType;
    private Action<Widget, Widget>? _addChildAction;
    private Action<Widget, Widget>? _removeChildAction;
    private Action<Widget, Widget?>? _setChildAction;

    public void AddChild(Widget parent, Widget child)
    {
        if (ContainerType != ContainerType.MultiChild)
            throw new NotSupportedException();

        if (_addChildAction == null)
        {
            var parentType = parent.GetType();
            var childrenPropInfo = parentType.GetProperty(PropertyName);
            if (childrenPropInfo == null)
                throw new Exception($"Can't find property[{PropertyName}] for [{parentType.Name}]");
            var listType = childrenPropInfo.PropertyType;
            var childType = typeof(Widget);
            if (listType.IsGenericType)
                childType = listType.GenericTypeArguments[0];
            var addMethodInfo = typeof(ICollection<>).MakeGenericType(childType).GetMethod("Add");

            var parentArg = Expression.Parameter(typeof(Widget));
            var childArg = Expression.Parameter(typeof(Widget));
            var convertedParent = Expression.Convert(parentArg, parentType);
            var convertedChild = Expression.Convert(childArg, childType);
            var childrenMember = Expression.MakeMemberAccess(convertedParent, childrenPropInfo);
            _addChildAction = Expression.Lambda<Action<Widget, Widget>>(
                Expression.Call(childrenMember, addMethodInfo!, convertedChild),
                parentArg, childArg
            ).Compile();
        }

        _addChildAction(parent, child);
    }

    public bool TryAddChild(Widget parent, Widget child)
    {
        try
        {
            AddChild(parent, child);
            return true;
        }
        catch (Exception ex)
        {
            Notification.Error(ex.Message);
            return false;
        }
    }

    public void RemoveChild(Widget parent, Widget child)
    {
        if (ContainerType != ContainerType.MultiChild)
            throw new NotSupportedException();

        if (_removeChildAction == null)
        {
            var parentType = parent.GetType();
            var childrenPropInfo = parentType.GetProperty(PropertyName);
            if (childrenPropInfo == null)
                throw new Exception($"Can't find property[{PropertyName}] for [{parentType.Name}]");
            var listType = childrenPropInfo.PropertyType;
            var childType = typeof(Widget);
            if (listType.IsGenericType)
                childType = listType.GenericTypeArguments[0];
            var removeMethodInfo = typeof(ICollection<>).MakeGenericType(childType).GetMethod("Remove");

            var parentArg = Expression.Parameter(typeof(Widget));
            var childArg = Expression.Parameter(typeof(Widget));
            var convertedParent = Expression.Convert(parentArg, parentType);
            var convertedChild = Expression.Convert(childArg, childType);
            var childrenMember = Expression.MakeMemberAccess(convertedParent, childrenPropInfo);
            _removeChildAction = Expression.Lambda<Action<Widget, Widget>>(
                Expression.Call(childrenMember, removeMethodInfo!, convertedChild),
                parentArg, childArg
            ).Compile();
        }

        _removeChildAction(parent, child);
    }

    public bool TryRemoveChild(Widget parent, Widget child)
    {
        try
        {
            RemoveChild(parent, child);
            return true;
        }
        catch (Exception ex)
        {
            Notification.Error(ex.Message);
            return false;
        }
    }

    public void SetChild(Widget parent, Widget? child)
    {
        if (ContainerType == ContainerType.MultiChild)
            throw new NotSupportedException();

        if (_setChildAction == null)
        {
            var parentType = parent.GetType();
            var childPropInfo = parentType.GetProperty(PropertyName);
            if (childPropInfo == null)
                throw new Exception($"Can't find property[{PropertyName}] for [{parentType.Name}]");
            var childType = typeof(Widget); //TODO: maybe other type

            var parentArg = Expression.Parameter(typeof(Widget));
            var childArg = Expression.Parameter(typeof(Widget));
            var convertedParent = Expression.Convert(parentArg, parentType);
            var convertedChild = Expression.Convert(childArg, childType);
            var childMember = Expression.MakeMemberAccess(convertedParent, childPropInfo);
            _setChildAction = Expression.Lambda<Action<Widget, Widget?>>(
                Expression.Assign(childMember, convertedChild)
                , parentArg, childArg
            ).Compile();
        }

        _setChildAction(parent, child);
    }

    public bool TrySetChild(Widget parent, Widget? child)
    {
        try
        {
            SetChild(parent, child);
            return true;
        }
        catch (Exception ex)
        {
            Notification.Error(ex.Message);
            return false;
        }
    }
}