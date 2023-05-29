#if !__WEB__
using System;
using System.Runtime.InteropServices;
using static CodeEditor.TreeSitterApi;

namespace CodeEditor
{
    public sealed class TSTreeCursor : IDisposable
    {
        private TsTreeCursor _native;

        internal TSTreeCursor(TSSyntaxNode initial)
        {
            unsafe
            {
                TsTreeCursor cursor;
                ts_tree_cursor_new_wasm(initial.NativeTsNode, &cursor);
                _native = cursor;
            }
        }

        public void Reset(TSSyntaxNode newNode)
        {
            ts_tree_cursor_reset(ref _native, newNode.NativeTsNode);
        }

        public bool GotoFirstChild()
        {
            return ts_tree_cursor_goto_first_child(ref _native);
        }

        public bool GotoNextSibling()
        {
            return ts_tree_cursor_goto_next_sibling(ref _native);
        }

        public bool GotoParent()
        {
            return ts_tree_cursor_goto_parent(ref _native);
        }

        public TSSyntaxNode Current
        {
            get
            {
                unsafe
                {
                    TsNode node;
                    ts_tree_cursor_current_node_wasm(ref _native, &node);
                    return TSSyntaxNode.Create(node)!;
                }
            }   
        }
        
        public ushort FieldId => ts_tree_cursor_current_field_id(ref _native);

        public string FieldName
        {
            get
            {
                var ptr = ts_tree_cursor_current_field_name(ref _native);
                return ptr == IntPtr.Zero ? null : Marshal.PtrToStringAnsi(ptr);
            }
        }

        public void Dispose()
        {
            ts_tree_cursor_delete(ref _native);
        }
    }
}
#endif