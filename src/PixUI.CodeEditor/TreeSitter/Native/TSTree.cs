#if !__WEB__
using System;
using System.Diagnostics;
using static CodeEditor.TreeSitterApi;

namespace CodeEditor;

public sealed class TSTree : IDisposable
{
    internal IntPtr Handle { get; }

    internal TSTree(IntPtr handle)
    {
        Debug.Assert(handle != IntPtr.Zero);
        Handle = handle;
    }

    public TSTree Copy()
    {
        return new TSTree(ts_tree_copy(Handle));
    }

    public TSNode Root
    {
        get
        {
            unsafe
            {
                TSNode node;
                ts_tree_root_node_wasm(Handle, &node);
                return TSNode.Create(node)!.Value;
            }
        }
    }

    internal void Edit(ref TSEdit edit) => ts_tree_edit(Handle, ref edit);

    public void Dispose() => ts_tree_delete(Handle);
}
#endif