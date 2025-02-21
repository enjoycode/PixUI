using System.Collections.Generic;
using System.Diagnostics;

namespace CodeEditor;

internal interface IRedBlackTreeNode<TSelf> where TSelf : class, IRedBlackTreeNode<TSelf>
{
    TSelf? Left { get; set; }
    TSelf? Right { get; set; }
    TSelf? Parent { get; set; }
    bool Color { get; set; }
}

internal static class RedBlackTreeExtensions
{
    public static T LeftMost<T>(this T node) where T : class, IRedBlackTreeNode<T>
    {
        var temp = node;
        while (temp.Left != null)
            temp = temp.Left;
        return temp;
    }

    public static T RightMost<T>(this T node) where T : class, IRedBlackTreeNode<T>
    {
        var temp = node;
        while (temp.Right != null)
            temp = temp.Right;
        return temp;
    }

    /// <summary>
    /// Gets the inorder successor of the node.
    /// </summary>
    /// <returns>The node following this node, or null if this is the last node.</returns>
    public static T? Successor<T>(this T node) where T : class, IRedBlackTreeNode<T>
    {
        if (node.Right != null)
        {
            return node.Right.LeftMost();
        }

        T? temp = node;
        T oldNode;
        do
        {
            oldNode = temp;
            temp = temp.Parent;
            // we are on the way up from the right part, don't output node again
        } while (temp != null && temp.Right == oldNode);

        return temp;
    }

    /// <summary>
    /// Gets the inorder predecessor of the node.
    /// </summary>
    /// <returns>The node before this node, or null if this is the first node.</returns>
    public static T? Predecessor<T>(this T node) where T : class, IRedBlackTreeNode<T>
    {
        if (node.Left != null)
        {
            return node.Left.RightMost();
        }

        T? temp = node;
        T oldNode;
        do
        {
            oldNode = temp;
            temp = temp.Parent;
            // we are on the way up from the left part, don't output node again
        } while (temp != null && temp.Left == oldNode);

        return temp;
    }
}

internal abstract class RedBlackTree<T> where T : class, IRedBlackTreeNode<T>
{
    protected const bool Red = true;
    protected const bool Black = false;

    protected T? Root { get; set; }

    #region ====Insert & Remove====

    protected void InsertAsLeft(T parentNode, T newNode)
    {
        Debug.Assert(parentNode.Left == null);
        parentNode.Left = newNode;
        newNode.Parent = parentNode;
        newNode.Color = Red;
        UpdateAfterChildrenChange(parentNode);
        FixTreeOnInsert(newNode);
    }

    protected void InsertAsRight(T parentNode, T newNode)
    {
        Debug.Assert(parentNode.Right == null);
        parentNode.Right = newNode;
        newNode.Parent = parentNode;
        newNode.Color = Red;
        UpdateAfterChildrenChange(parentNode);
        FixTreeOnInsert(newNode);
    }

    private void FixTreeOnInsert(T node)
    {
        Debug.Assert(node != null);
        Debug.Assert(node.Color == Red);
        Debug.Assert(node.Left == null || node.Left.Color == Black);
        Debug.Assert(node.Right == null || node.Right.Color == Black);

        var parentNode = node.Parent;
        if (parentNode == null)
        {
            // we inserted in the root -> the node must be black
            // since this is a root node, making the node black increments the number of black nodes
            // on all paths by one, so it is still the same for all paths.
            node.Color = Black;
            return;
        }

        if (parentNode.Color == Black)
        {
            // if the parent node where we inserted was black, our red node is placed correctly.
            // since we inserted a red node, the number of black nodes on each path is unchanged
            // -> the tree is still balanced
            return;
        }
        // parentNode is red, so there is a conflict here!

        // because the root is black, parentNode is not the root -> there is a grandparent node
        var grandparentNode = parentNode.Parent!;
        var uncleNode = Sibling(parentNode);
        if (uncleNode != null && uncleNode.Color == Red)
        {
            parentNode.Color = Black;
            uncleNode.Color = Black;
            grandparentNode.Color = Red;
            FixTreeOnInsert(grandparentNode);
            return;
        }

        // now we know: parent is red but uncle is black
        // First rotation:
        if (node == parentNode.Right && parentNode == grandparentNode.Left)
        {
            RotateLeft(parentNode);
            node = node.Left!;
        }
        else if (node == parentNode.Left && parentNode == grandparentNode.Right)
        {
            RotateRight(parentNode);
            node = node.Right!;
        }

        // because node might have changed, reassign variables:
        // ReSharper disable once PossibleNullReferenceException
        parentNode = node.Parent;
        grandparentNode = parentNode!.Parent!;

        // Now recolor a bit:
        parentNode.Color = Black;
        grandparentNode.Color = Red;
        // Second rotation:
        if (node == parentNode.Left && parentNode == grandparentNode.Left)
        {
            RotateRight(grandparentNode);
        }
        else
        {
            // because of the first rotation, this is guaranteed:
            Debug.Assert(node == parentNode.Right && parentNode == grandparentNode.Right);
            RotateLeft(grandparentNode);
        }
    }

    protected void RemoveNode(T removedNode)
    {
        if (removedNode.Left != null && removedNode.Right != null)
        {
            // replace removedNode with it's in-order successor

            var leftMost = removedNode.Right.LeftMost();
            RemoveNode(leftMost); // remove leftMost from its current location

            // and overwrite the removedNode with it
            ReplaceNode(removedNode, leftMost);
            leftMost.Left = removedNode.Left;
            if (leftMost.Left != null) leftMost.Left.Parent = leftMost;
            leftMost.Right = removedNode.Right;
            if (leftMost.Right != null) leftMost.Right.Parent = leftMost;
            leftMost.Color = removedNode.Color;

            UpdateAfterChildrenChange(leftMost);
            if (leftMost.Parent != null) UpdateAfterChildrenChange(leftMost.Parent);
            return;
        }

        // now either removedNode.left or removedNode.right is null
        // get the remaining child
        var parentNode = removedNode.Parent;
        var childNode = removedNode.Left ?? removedNode.Right;
        ReplaceNode(removedNode, childNode);
        if (parentNode != null) UpdateAfterChildrenChange(parentNode);
        if (removedNode.Color == Black)
        {
            if (childNode != null && childNode.Color == Red)
            {
                childNode.Color = Black;
            }
            else
            {
                FixTreeOnDelete(childNode, parentNode);
            }
        }
    }

    private void FixTreeOnDelete(T? node, T? parentNode)
    {
        Debug.Assert(node == null || node.Parent == parentNode);
        if (parentNode == null)
            return;

        // warning: node may be null
        var sibling = Sibling(node, parentNode)!;
        if (sibling.Color == Red)
        {
            parentNode.Color = Red;
            sibling.Color = Black;
            if (node == parentNode.Left)
            {
                RotateLeft(parentNode);
            }
            else
            {
                RotateRight(parentNode);
            }

            sibling = Sibling(node, parentNode)!; // update value of sibling after rotation
        }

        if (parentNode.Color == Black
            && sibling.Color == Black
            && GetColor(sibling.Left) == Black
            && GetColor(sibling.Right) == Black)
        {
            sibling.Color = Red;
            FixTreeOnDelete(parentNode, parentNode.Parent);
            return;
        }

        if (parentNode.Color == Red
            && sibling.Color == Black
            && GetColor(sibling.Left) == Black
            && GetColor(sibling.Right) == Black)
        {
            sibling.Color = Red;
            parentNode.Color = Black;
            return;
        }

        if (node == parentNode.Left &&
            sibling.Color == Black &&
            GetColor(sibling.Left) == Red &&
            GetColor(sibling.Right) == Black)
        {
            sibling.Color = Red;
            sibling.Left!.Color = Black;
            RotateRight(sibling);
        }
        else if (node == parentNode.Right &&
                 sibling.Color == Black &&
                 GetColor(sibling.Right) == Red &&
                 GetColor(sibling.Left) == Black)
        {
            sibling.Color = Red;
            sibling.Right!.Color = Black;
            RotateLeft(sibling);
        }

        sibling = Sibling(node, parentNode)!; // update value of sibling after rotation

        sibling.Color = parentNode.Color;
        parentNode.Color = Black;
        if (node == parentNode.Left)
        {
            if (sibling.Right != null)
            {
                Debug.Assert(sibling.Right.Color == Red);
                sibling.Right.Color = Black;
            }

            RotateLeft(parentNode);
        }
        else
        {
            if (sibling.Left != null)
            {
                Debug.Assert(sibling.Left.Color == Red);
                sibling.Left.Color = Black;
            }

            RotateRight(parentNode);
        }
    }

    private void ReplaceNode(T replacedNode, T? newNode)
    {
        if (replacedNode.Parent == null)
        {
            Debug.Assert(replacedNode == Root);
            Root = newNode;
        }
        else
        {
            if (replacedNode.Parent.Left == replacedNode)
                replacedNode.Parent.Left = newNode;
            else
                replacedNode.Parent.Right = newNode;
        }

        if (newNode != null)
        {
            newNode.Parent = replacedNode.Parent;
        }

        replacedNode.Parent = null;
    }

    private void RotateLeft(T p)
    {
        // let q be p's right child
        var q = p.Right;
        Debug.Assert(q != null);
        Debug.Assert(q.Parent == p);
        // set q to be the new root
        ReplaceNode(p, q);

        // set p's right child to be q's left child
        p.Right = q.Left;
        if (p.Right != null) p.Right.Parent = p;
        // set q's left child to be p
        q.Left = p;
        p.Parent = q;

        UpdateAfterRotate(p);
    }

    private void RotateRight(T p)
    {
        // let q be p's left child
        var q = p.Left;
        Debug.Assert(q != null);
        Debug.Assert(q.Parent == p);
        // set q to be the new root
        ReplaceNode(p, q);

        // set p's left child to be q's right child
        p.Left = q.Right;
        if (p.Left != null) p.Left.Parent = p;
        // set q's right child to be p
        q.Right = p;
        p.Parent = q;

        UpdateAfterRotate(p);
    }

    private static T? Sibling(T node)
    {
        return node == node.Parent!.Left ? node.Parent.Right : node.Parent.Left;
    }

    private static T? Sibling(T? node, T parentNode)
    {
        Debug.Assert(node == null || node.Parent == parentNode);
        return node == parentNode.Left ? parentNode.Right : parentNode.Left;
    }

    private static bool GetColor(T? node)
    {
        return node != null && node.Color;
    }

    protected virtual void UpdateAfterChildrenChange(T node) { }

    protected virtual void UpdateAfterRotate(T node)
    {
        UpdateAfterChildrenChange(node);
        UpdateAfterChildrenChange(node.Parent!);
    }

    #endregion

    #region ====ICollection====

    public IEnumerator<T> GetEnumerator()
    {
        return Enumerate();
    }

    private IEnumerator<T> Enumerate()
    {
        var node = Root!.LeftMost();
        while (node != null)
        {
            yield return node;
            node = node.Successor();
        }
    }

    #endregion
}