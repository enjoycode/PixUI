#if !__WEB__
using System;
using System.Runtime.InteropServices;
using static CodeEditor.TreeSitterApi;

namespace CodeEditor
{
    public sealed class TSLanguage
    {
        #region ====static====

        internal const string LibTreeSitterCSharp = "tree-sitter-csharp";

        public static TSLanguage Get(IntPtr ptr)
        {
            //always return TSCSharpLanguage instance now
            return TSCSharpLanguage.Get();
        }

        #endregion

        internal IntPtr Handle;
        private readonly string[] _symbols;

        public TSLanguage(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
                throw new ArgumentNullException(nameof(handle));

            Handle = handle;

            _symbols = new string[SymbolCount];
            for (var i = 0; i < _symbols.Length; i++)
            {
                _symbols[i] = SymbolName((ushort)i);
            }
        }

        public string GetType(ushort typeId)
        {
            return typeId >= _symbols.Length ? "Error" : _symbols[typeId];
        }

        public int SymbolCount => (int)ts_language_symbol_count(Handle);

        public string SymbolName(ushort symbol) =>
            Marshal.PtrToStringAnsi(ts_language_symbol_name(Handle, symbol))!;

        public ushort? SymbolForName(string name, bool isNamed)
        {
            var ptr = Marshal.StringToHGlobalAnsi(name);
            var id = ts_language_symbol_for_name(Handle, ptr, (uint)name.Length, isNamed);
            Marshal.FreeHGlobal(ptr);
            return id == 0 ? (ushort?)null : id;
        }

        public int FieldCount => ts_language_field_count(Handle);

        public string FieldNameForId(ushort fieldId) =>
            Marshal.PtrToStringAnsi(ts_language_field_name_for_id(Handle, fieldId))!;

        public ushort? FieldIdForName(string fieldName)
        {
            var ptr = Marshal.StringToHGlobalAnsi(fieldName);
            var id = ts_language_field_id_for_name(Handle, ptr, (uint)fieldName.Length);
            Marshal.FreeHGlobal(ptr);
            return id == 0 ? (ushort?)null : id;
        }

        public SymbolType SymbolType(ushort symbol) =>
            (SymbolType)ts_language_symbol_type(Handle, symbol);

        public TSQuery? Query(string source)
        {
            var ptr = Marshal.StringToHGlobalAnsi(source);
            uint errorOffset = 0;
            var errorType = TsQueryError.None;
            var queryHandle = ts_query_new(Handle, ptr, (uint)source.Length, ref errorOffset, ref errorType);
            Marshal.FreeHGlobal(ptr);

            return queryHandle == IntPtr.Zero ? null : new TSQuery(queryHandle);
        }
    }

    public static class TSCSharpLanguage
    {
        private static TSLanguage? _csharpLanguage;

        [DllImport(TSLanguage.LibTreeSitterCSharp)]
        private static extern IntPtr tree_sitter_c_sharp();

        public static TSLanguage Get()
        {
            _csharpLanguage ??= new TSLanguage(tree_sitter_c_sharp());
            return _csharpLanguage;
        }
    }

    public enum SymbolType
    {
        Regular,
        Anonymous,
        Auxiliary
    }
}

#endif