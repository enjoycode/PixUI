#if !__WEB__

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using static CodeEditor.TreeSitterApi;

namespace CodeEditor;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct TSNode : IEquatable<TSNode>
{
    public uint context1;
    public uint context2;
    public uint context3;
    public uint context4;
    public IntPtr id;
    public IntPtr tree;

    internal static TSNode? Create(TSNode node) => node.id == IntPtr.Zero ? null : node;

    /// <summary>
    /// Get the <see cref="TSLanguage"/> that was used to parse this node's syntax tree.
    /// </summary>
    public TSLanguage Language => TSLanguage.Get(ts_tree_language(tree));

    /// <summary>
    /// Get this node's type as a numerical id.
    /// </summary>
    public ushort TypeId => ts_node_symbol(this);

    /// <summary>
    /// Get this node's type as a string
    /// </summary>
    public string Type => Language.GetType(TypeId); //Marshal.PtrToStringAnsi(ts_node_type(Handle));

    /// <summary>
    /// Check if this node is named.
    /// </summary>
    /// <remarks>
    /// Named nodes correspond to named rules in the grammar, whereas anonymous nodes
    /// correspond to string literals in the grammar
    /// </remarks>
    public bool IsNamed() => ts_node_is_named(this);

    /// <summary>
    /// Check if this node is extra.
    /// </summary>
    /// <remarks>
    /// Extra nodes represent things like comments, which are not required the grammar,
    /// but can appear anywhere.
    /// </remarks>
    public bool IsExtra => ts_node_is_extra(this);

    /// <summary>
    /// Check if this node has been edited.
    /// </summary>
    public bool HasChanges => ts_node_has_changes(this);

    /// <summary>
    /// Check if this node represents a syntax error or contains any syntax errors anywhere
    /// within it.
    /// </summary>
    public bool HasError() => ts_node_has_error(this);

    /// <summary>
    /// Check if this node represents a syntax error.
    /// </summary>
    /// <remarks>
    /// Syntax errors represent parts of the code that could not be incorporated into a
    /// valid syntax tree.
    /// </remarks>
    public bool IsError => TypeId == ushort.MaxValue;

    /// <summary>
    /// Check if this node is *missing*.
    /// </summary>
    /// <remarks>
    /// Missing nodes are inserted by the parser in order to recover from certain kinds of
    /// syntax errors.
    /// </remarks>
    public bool IsMissing => ts_node_is_missing(this);

    /// <summary>
    /// Get the byte offsets where this node starts.
    /// </summary>
    public int StartIndex => (int)ts_node_start_byte(this);

    /// <summary>
    /// Get the byte offsets where this node ends.
    /// </summary>
    public int EndIndex => (int)ts_node_end_byte(this);

    /// <summary>
    /// Get the range of source code that this node represents, both in terms of raw bytes
    /// and of row/column coordinates.
    /// </summary>
    public TSRange Range => new TSRange
    {
        StartIndex = (uint)StartIndex, EndIndex = (uint)EndIndex,
        StartPosition = StartPosition, EndPosition = EndPosition
    };

    /// <summary>
    /// Get this node's start position in terms of rows and columns.
    /// </summary>
    public TSPoint StartPosition => new(context2, context3);

    /// <summary>
    /// Get this node's end position in terms of rows and columns.
    /// </summary>
    public TSPoint EndPosition
    {
        get
        {
            TSPoint endPoint;
            ts_node_end_point_wasm(this, &endPoint);
            return endPoint;
        }
    }

    /// <summary>
    /// Get the node's child at the given index, where zero represents the first
    /// child.
    /// </summary>
    /// <param name="index">Index of the child</param>
    /// <returns>The child at specified index</returns>
    /// <remarks>
    /// This method is fairly fast, but its cost is technically log(i), so you
    /// if you might be iterating over a long list of children, you should use
    /// <see cref="Children"/> instead.
    /// </remarks>
    public TSNode Child(int index)
    {
        TSNode child;
        ts_node_child_wasm(this, (uint)index, &child);
        return Create(child)!.Value;
    }

    /// <summary>
    /// Get this node's number of children.
    /// </summary>
    public int ChildCount => (int)ts_node_child_count(this);

    /// <summary>
    /// Get the node's named child at the given index.
    /// child.
    /// </summary>
    /// <param name="index">Index of the child</param>
    /// <returns>The child at specified index</returns>
    /// <seealso cref="IsNamed"/>
    /// <remarks>
    /// This method is fairly fast, but its cost is technically log(i), so you
    /// if you might be iterating over a long list of children, you should use
    /// <see cref="NamedChildren"/> instead.
    /// </remarks>
    public TSNode NamedChild(int index)
    {
        TSNode child;
        ts_node_named_child_wasm(this, (uint)index, &child);
        return Create(child)!.Value;
    }

    /// <summary>
    /// Get this node's number of children.
    /// </summary>
    public int NamedChildCount => (int)ts_node_named_child_count(this);

    /// <summary>
    /// Get the first child with the given field name.
    /// </summary>
    /// <param name="fieldName">Field name to get</param>
    /// <returns>The first child with given name</returns>
    /// <remarks>
    /// If multiple children may have the same field name, access them using
    /// <see cref="ChildrenByFieldName"/>
    /// </remarks>
    public TSNode ChildByFieldName(string fieldName)
    {
        var ptr = Marshal.StringToHGlobalAnsi(fieldName);
        TSNode child;
        ts_node_child_by_field_name_wasm(this, ptr, (uint)fieldName.Length, &child);
        Marshal.FreeHGlobal(ptr);
        return Create(child)!.Value;
    }

    /// <summary>
    /// Get this node's child with the given numerical field id.
    /// </summary>
    /// <seealso cref="ChildByFieldName"/>
    /// <param name="fieldId">Numerical field id</param>
    /// <returns>The child</returns>
    public TSNode ChildByFieldId(ushort fieldId)
    {
        TSNode child;
        ts_node_child_by_field_id_wasm(this, fieldId, &child);
        return Create(child)!.Value;
    }

    /// <summary>
    /// Iterate over this node's children.
    /// </summary>
    public IEnumerable<TSNode> Children
    {
        get
        {
            var cursor = new TSTreeCursor(this);
            cursor.GotoFirstChild();
            return Enumerable.Range(0, ChildCount).Select(_ =>
            {
                var result = cursor.Current;
                cursor.GotoNextSibling();
                return result;
            }).Finally(cursor.Dispose);
        }
    }

    /// <summary>
    /// Iterate over this node's named children.
    /// </summary>
    public IEnumerable<TSNode> NamedChildren => Children.Where(x => x.IsNamed());

    public IEnumerable<KeyValuePair<string, TSNode>> ChildrenWithFields
    {
        get
        {
            var cursor = new TSTreeCursor(this);
            cursor.GotoFirstChild();
            return Enumerable.Range(0, ChildCount).Select(_ =>
            {
                var result = cursor.Current;
                var key = cursor.FieldName!;
                cursor.GotoNextSibling();
                return new KeyValuePair<string, TSNode>(key, result);
            }).Finally(cursor.Dispose);
        }
    }

    public IEnumerable<KeyValuePair<string, TSNode>> NamedChildrenWithFields =>
        ChildrenWithFields.Where(x => x.Value.IsNamed());

    public IEnumerable<TSNode> ChildrenByFieldName(string fieldName)
    {
        var fieldId = Language.FieldIdForName(fieldName);
        return ChildrenByFieldId(fieldId ?? 0);
    }

    public IEnumerable<TSNode> ChildrenByFieldId(ushort fieldId)
    {
        using var cursor = new TSTreeCursor(this);
        cursor.GotoFirstChild();

        var done = false;
        while (!done)
        {
            while (cursor.FieldId != fieldId)
                if (!cursor.GotoNextSibling())
                    yield break;

            var result = cursor.Current;

            if (!cursor.GotoNextSibling())
                done = true;

            yield return result;
        }
    }

    public TSNode? Parent
    {
        get
        {
            TSNode parent;
            ts_node_parent_wasm(this, &parent);
            return Create(parent);
        }
    }

    public TSNode? NextSibling
    {
        get
        {
            TSNode next;
            ts_node_next_sibling_wasm(this, &next);
            return Create(next);
        }
    }

    public TSNode? PrevSibling
    {
        get
        {
            TSNode prev;
            ts_node_prev_sibling_wasm(this, &prev);
            return Create(prev);
        }
    }

    public TSNode? NextNamedSibling
    {
        get
        {
            TSNode node;
            ts_node_next_named_sibling_wasm(this, &node);
            return Create(node);
        }
    }

    public TSNode? PrevNamedSibling
    {
        get
        {
            TSNode prev;
            ts_node_prev_named_sibling_wasm(this, &prev);
            return Create(prev);
        }
    }

    internal TSNode? DescendantForPosition(TSPoint start, TSPoint? end = null)
    {
        TSNode child;
        ts_node_descendant_for_point_range_wasm(this, start, end ?? start, &child);
        return Create(child);
    }

    internal TSNode? NamedDescendantForPosition(TSPoint start, TSPoint? end = null)
    {
        TSNode node;
        ts_node_named_descendant_for_point_range_wasm(this, start, end ?? start, &node);
        return Create(node);
    }

    public override string ToString()
    {
        // var cPtr = ts_node_string(Handle);
        // var result = Marshal.PtrToStringAnsi(cPtr);
        // //TODO: System.Runtime.InteropServices.NativeMemory.Free(cPtr.ToPointer());
        // return result;
        var sb = new StringBuilder();
        DumpTree(this, sb, 0);
        return sb.ToString();
    }

    private static void DumpTree(TSNode node, StringBuilder sb, int depth)
    {
        sb.Append('\t', depth);
        if (node.IsNamed())
            sb.Append($"{node.Type}[{node.TypeId}]");
        else
            sb.Append($"\"{node.Type}\"[{node.TypeId}]");
        sb.Append($" {node.StartPosition}-{node.EndPosition}\n");
        foreach (var child in node.Children)
        {
            DumpTree(child, sb, depth + 1);
        }
    }

    public bool Equals(TSNode other) => ts_node_eq(this, other);

    public override int GetHashCode() => id.ToInt32();
}

public static class EnumerableExtensions
{
    public static IEnumerable<T> Finally<T>(this IEnumerable<T> enumerable, Action after)
    {
        try
        {
            foreach (var value in enumerable)
                yield return value;
        }
        finally
        {
            after();
        }
    }
}
#endif