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
            var itemType = typeof(Widget);
            if (listType.IsGenericType)
                itemType = listType.GenericTypeArguments[0];
            var addMethodInfo = typeof(ICollection<>).MakeGenericType(itemType).GetMethod("Add");

            var parentArg = Expression.Parameter(typeof(Widget));
            var childArg = Expression.Parameter(typeof(Widget));
            var convertedParent = Expression.Convert(parentArg, parentType);
            var convertedChild = Expression.Convert(childArg, itemType);
            var childrenMember = Expression.MakeMemberAccess(convertedParent, childrenPropInfo);
            _addChildAction = Expression.Lambda<Action<Widget, Widget>>(
                Expression.Call(childrenMember, addMethodInfo!, convertedChild), parentArg, childArg
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
}