using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

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

public enum ChildrenLayoutAxis
{
    None,
    Horizontal,
    Vertical,
    Positioned,
}

public sealed class ContainerSlot
{
    public ContainerSlot(string propertyName, ContainerType type,
        ChildrenLayoutAxis layoutAxis = ChildrenLayoutAxis.None)
    {
        PropertyName = propertyName;
        ContainerType = type;
        LayoutAxis = layoutAxis;
    }

    public readonly string PropertyName;
    public readonly ContainerType ContainerType;
    public readonly ChildrenLayoutAxis LayoutAxis;
    private Action<Widget, Widget>? _addChildAction;
    private Action<Widget, Widget>? _removeChildAction;
    private Action<Widget, Widget, Widget>? _replaceChildAction;
    private Func<Widget, Widget, MoveChildAction, bool>? _moveChildAction;
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

    public bool MoveChild(Widget parent, Widget child, MoveChildAction action)
    {
        if (ContainerType != ContainerType.MultiChild)
            throw new NotSupportedException();

        if (_moveChildAction == null)
        {
            var parentType = parent.GetType();
            var childrenPropInfo = parentType.GetProperty(PropertyName);
            if (childrenPropInfo == null)
                throw new Exception($"Can't find property[{PropertyName}] for [{parentType.Name}]");
            var listType = childrenPropInfo.PropertyType;
            var childType = typeof(Widget);
            if (listType.IsGenericType)
                childType = listType.GenericTypeArguments[0];
            var widgetListType = typeof(WidgetList<>).MakeGenericType(childType);
            var moveMethodInfo = widgetListType.GetMethod("MoveItem", BindingFlags.Instance | BindingFlags.NonPublic);

            var parentArg = Expression.Parameter(typeof(Widget));
            var childArg = Expression.Parameter(typeof(Widget));
            var actionArg = Expression.Parameter(typeof(MoveChildAction));
            var convertedParent = Expression.Convert(parentArg, parentType);
            var convertedChild = Expression.Convert(childArg, childType);
            var childrenMember = Expression.MakeMemberAccess(convertedParent, childrenPropInfo);
            var convertedChildren = Expression.Convert(childrenMember, widgetListType);
            _moveChildAction = Expression.Lambda<Func<Widget, Widget, MoveChildAction, bool>>(
                Expression.Call(convertedChildren, moveMethodInfo!, convertedChild, actionArg),
                parentArg, childArg, actionArg
            ).Compile();
        }

        return _moveChildAction(parent, child, action);
    }

    public bool TryMoveChild(Widget parent, Widget child, MoveChildAction action)
    {
        try
        {
            return MoveChild(parent, child, action);
        }
        catch (Exception e)
        {
            Notification.Error(e.Message);
            return false;
        }
    }

    public void ReplaceChild(Widget parent, Widget oldChild, Widget newChild)
    {
        if (ContainerType != ContainerType.MultiChild)
            throw new NotSupportedException();

        if (_replaceChildAction == null)
        {
            var parentType = parent.GetType();
            var childrenPropInfo = parentType.GetProperty(PropertyName);
            if (childrenPropInfo == null)
                throw new Exception($"Can't find property[{PropertyName}] for [{parentType.Name}]");
            var listType = childrenPropInfo.PropertyType;
            var childType = typeof(Widget);
            if (listType.IsGenericType)
                childType = listType.GenericTypeArguments[0];

            var indexerPropInfo = listType.GetProperty("Item")!;
            var indexOfMethodInfo = typeof(IList<>).MakeGenericType(childType).GetMethod("IndexOf")!;

            var parentArg = Expression.Parameter(typeof(Widget));
            var oldChildArg = Expression.Parameter(typeof(Widget));
            var newChildArg = Expression.Parameter(typeof(Widget));
            var convertedParent = Expression.Convert(parentArg, parentType);
            var convertedOldChild = Expression.Convert(oldChildArg, childType);
            var convertedNewChild = Expression.Convert(newChildArg, childType);
            var childrenMember = Expression.MakeMemberAccess(convertedParent, childrenPropInfo);
            _replaceChildAction = Expression.Lambda<Action<Widget, Widget, Widget>>(
                Expression.Assign(
                    Expression.MakeIndex(childrenMember, indexerPropInfo,
                        new[] { Expression.Call(childrenMember, indexOfMethodInfo, convertedOldChild) }),
                    convertedNewChild
                ),
                parentArg, oldChildArg, newChildArg
            ).Compile();
        }

        _replaceChildAction(parent, oldChild, newChild);
    }

    public bool TryReplaceChild(Widget parent, Widget oldChild, Widget newChild)
    {
        try
        {
            ReplaceChild(parent, oldChild, newChild);
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