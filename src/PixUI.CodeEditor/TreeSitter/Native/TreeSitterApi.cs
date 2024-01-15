#if !__WEB__
using System;
using System.Runtime.InteropServices;

namespace CodeEditor;

internal enum TsInputEncoding
{
    Utf8,
    Utf16
}

internal enum TsSymbolType
{
    Regular,
    Anonymous,
    Auxiliary
}

internal enum TsLogType
{
    Parse,
    Lex
}

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal delegate void TsLogDelegate(IntPtr payload, TsLogType logType, IntPtr data);

[StructLayout(LayoutKind.Sequential)]
internal struct TsLogger
{
    public IntPtr payload;
    public IntPtr log;
}

[StructLayout(LayoutKind.Sequential)]
internal struct TsTreeCursor
{
    public IntPtr tree;
    public IntPtr id;
    public uint context1;
    public uint context2;
}

[StructLayout(LayoutKind.Sequential)]
internal struct QueryCapture
{
    public TSNode node;
    public uint index;
}

[StructLayout(LayoutKind.Sequential)]
internal struct TsQueryMatch
{
    public uint id;
    public ushort pattern_index;
    public ushort capture_count;
    public IntPtr captures;

    public override string ToString() =>
        $"TsQueryMatch[Id={id}, PatternIndex={pattern_index}, CaptureCount={capture_count}]";
}

public enum TsQueryPredicateStepType
{
    Done,
    Capture,
    String,
};

[StructLayout(LayoutKind.Sequential)]
public struct TsQueryPredicateStep
{
    TsQueryPredicateStepType type;
    uint value_id;
}

internal enum TsQueryError
{
    None = 0,
    Syntax,
    NodeType,
    Field,
    Capture,
}

internal static unsafe partial class TreeSitterApi
{
    private const string LibTreeSitter = "tree-sitter";

    // 临时使用，待使用NativeMemory.Free()
    // [LibraryImport(LibTreeSitter)]
    // internal static partial void ts_util_free(IntPtr ptr);

    #region ====Parser====

    [LibraryImport(LibTreeSitter)]
    internal static partial IntPtr ts_parser_new();

    [LibraryImport(LibTreeSitter)]
    internal static partial void ts_parser_delete(IntPtr parser);

    [LibraryImport(LibTreeSitter)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool ts_parser_set_language(IntPtr self, IntPtr language);

    [LibraryImport(LibTreeSitter)]
    internal static partial IntPtr ts_parser_language(IntPtr self);

    [LibraryImport(LibTreeSitter)]
    internal static partial void ts_parser_set_included_ranges(
        IntPtr self,
        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)]
        TSRange[] ranges,
        uint length);

    [LibraryImport(LibTreeSitter)]
    [return: MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]
    internal static partial TSRange[] ts_parser_included_ranges(
        IntPtr self,
        out uint length);

    [LibraryImport(LibTreeSitter)]
    internal static partial IntPtr ts_parser_parse(
        IntPtr self,
        IntPtr oldTree,
        TSInput input
    );

    [LibraryImport(LibTreeSitter)]
    internal static partial IntPtr ts_parser_parse_string(
        IntPtr self,
        IntPtr oldTree,
        IntPtr input,
        uint length
    );

    [LibraryImport(LibTreeSitter)]
    internal static partial IntPtr ts_parser_parse_string_encoding(
        IntPtr self,
        IntPtr oldTree,
        IntPtr input,
        uint length,
        TsInputEncoding encoding
    );

    [LibraryImport(LibTreeSitter)]
    internal static partial void ts_parser_reset(IntPtr self);

    [LibraryImport(LibTreeSitter)]
    internal static partial void ts_parser_set_timeout_micros(IntPtr self, ulong timeout);

    [LibraryImport(LibTreeSitter)]
    internal static partial ulong ts_parser_timeout_micros(IntPtr self);

    [LibraryImport(LibTreeSitter)]
    internal static partial void ts_parser_set_cancellation_flag(IntPtr self, IntPtr flag);

    [LibraryImport(LibTreeSitter)]
    internal static partial void ts_parser_set_logger(IntPtr self, TsLogger logger);

    [LibraryImport(LibTreeSitter)]
    internal static partial TsLogger ts_parser_get_logger(IntPtr self);

    [LibraryImport(LibTreeSitter)]
    internal static partial void ts_parser_print_dot_graphs(IntPtr self, int file);

    #endregion

    #region ====Tree====

    [LibraryImport(LibTreeSitter)]
    internal static partial IntPtr ts_tree_copy(IntPtr self);

    [LibraryImport(LibTreeSitter)]
    internal static partial void ts_tree_delete(IntPtr self);

    // [LibraryImport(LibTreeSitter)]
    // internal static partial TsNode ts_tree_root_node(IntPtr self);

    [LibraryImport(LibTreeSitter)]
    internal static unsafe partial void ts_tree_root_node_wasm(IntPtr self, TSNode* node);

    [LibraryImport(LibTreeSitter)]
    internal static partial IntPtr ts_tree_language(IntPtr self);

    [LibraryImport(LibTreeSitter)]
    internal static partial void ts_tree_edit(IntPtr self, ref TSEdit edit);

    [LibraryImport(LibTreeSitter)]
    internal static unsafe partial TSRange* ts_tree_get_changed_ranges(IntPtr oldTree, IntPtr newTree,
        ref uint length);

    #endregion

    #region ====Node====

    [LibraryImport(LibTreeSitter)]
    internal static partial IntPtr ts_node_type(TSNode node);

    [LibraryImport(LibTreeSitter)]
    internal static partial ushort ts_node_symbol(TSNode node);

    [LibraryImport(LibTreeSitter)]
    internal static partial uint ts_node_start_byte(TSNode node);

    // [LibraryImport(LibTreeSitter)]
    // internal static partial TSPoint ts_node_start_point(TsNode node);

    [LibraryImport(LibTreeSitter)]
    internal static partial uint ts_node_end_byte(TSNode node);

    // [LibraryImport(LibTreeSitter)]
    // internal static partial TSPoint ts_node_end_point(TsNode node);

    [LibraryImport(LibTreeSitter)]
    internal static unsafe partial void ts_node_end_point_wasm(TSNode node, TSPoint* point);

    [LibraryImport(LibTreeSitter)]
    internal static partial IntPtr ts_node_string(TSNode node);

    [LibraryImport(LibTreeSitter)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool ts_node_is_null(TSNode node);

    [LibraryImport(LibTreeSitter)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool ts_node_is_named(TSNode node);

    [LibraryImport(LibTreeSitter)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool ts_node_is_missing(TSNode node);

    [LibraryImport(LibTreeSitter)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool ts_node_is_extra(TSNode node);

    [LibraryImport(LibTreeSitter)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool ts_node_has_changes(TSNode node);

    [LibraryImport(LibTreeSitter)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool ts_node_has_error(TSNode node);

    // [LibraryImport(LibTreeSitter)]
    // internal static partial TsNode ts_node_parent(TsNode node);

    [LibraryImport(LibTreeSitter)]
    internal static unsafe partial void ts_node_parent_wasm(TSNode node, TSNode* parent);

    // [LibraryImport(LibTreeSitter)]
    // internal static partial TsNode ts_node_child(TsNode node, uint index);

    [LibraryImport(LibTreeSitter)]
    internal static unsafe partial void ts_node_child_wasm(TSNode node, uint index, TSNode* child);

    [LibraryImport(LibTreeSitter)]
    internal static partial uint ts_node_child_count(TSNode node);

    // [LibraryImport(LibTreeSitter)]
    // internal static partial TsNode ts_node_named_child(TsNode node, uint index);

    [LibraryImport(LibTreeSitter)]
    internal static partial void ts_node_named_child_wasm(TSNode node, uint index, TSNode* child);

    [LibraryImport(LibTreeSitter)]
    internal static partial uint ts_node_named_child_count(TSNode node);

    // [LibraryImport(LibTreeSitter)]
    // internal static partial TsNode ts_node_child_by_field_name(
    //     TsNode node,
    //     IntPtr fieldName,
    //     uint fieldNameLength
    // );

    [LibraryImport(LibTreeSitter)]
    internal static partial void ts_node_child_by_field_name_wasm(
        TSNode node,
        IntPtr fieldName,
        uint fieldNameLength,
        TSNode* child
    );

    // [LibraryImport(LibTreeSitter)]
    // internal static partial TsNode ts_node_child_by_field_id(TsNode node, ushort fieldId);

    [LibraryImport(LibTreeSitter)]
    internal static partial void ts_node_child_by_field_id_wasm(TSNode node, ushort fieldId, TSNode* child);

    // [LibraryImport(LibTreeSitter)]
    // internal static partial TsNode ts_node_next_sibling(TsNode node);

    [LibraryImport(LibTreeSitter)]
    internal static partial void ts_node_next_sibling_wasm(TSNode node, TSNode* next);

    // [LibraryImport(LibTreeSitter)]
    // internal static partial TsNode ts_node_prev_sibling(TsNode node);
    [LibraryImport(LibTreeSitter)]
    internal static partial void ts_node_prev_sibling_wasm(TSNode node, TSNode* prev);

    // [LibraryImport(LibTreeSitter)]
    // internal static partial TsNode ts_node_next_named_sibling(TsNode node);

    [LibraryImport(LibTreeSitter)]
    internal static unsafe partial void ts_node_next_named_sibling_wasm(TSNode node, TSNode* result);

    // [LibraryImport(LibTreeSitter)]
    // internal static partial TsNode ts_node_prev_named_sibling(TsNode node);

    [LibraryImport(LibTreeSitter)]
    internal static partial void ts_node_prev_named_sibling_wasm(TSNode node, TSNode* prev);

    // [LibraryImport(LibTreeSitter)]
    // internal static partial TsNode ts_node_first_child_for_byte(TsNode node, uint byteOffset);

    [LibraryImport(LibTreeSitter)]
    internal static partial void ts_node_first_child_for_byte_wasm(TSNode node, uint byteOffset, TSNode* child);

    // [LibraryImport(LibTreeSitter)]
    // internal static partial TsNode ts_node_first_named_child_for_byte(TsNode node, uint offset);

    [LibraryImport(LibTreeSitter)]
    internal static partial void ts_node_first_named_child_for_byte_wasm(TSNode node, uint offset, TSNode* child);

    // [LibraryImport(LibTreeSitter)]
    // internal static partial TsNode ts_node_descendant_for_byte_range(TsNode node, uint start, uint end);

    [LibraryImport(LibTreeSitter)]
    internal static partial void ts_node_descendant_for_byte_range_wasm(TSNode node, uint start, uint end,
        TSNode* child);

    // [LibraryImport(LibTreeSitter)]
    // internal static partial TsNode ts_node_descendant_for_point_range(TsNode node, TSPoint start, TSPoint end);

    [LibraryImport(LibTreeSitter)]
    internal static partial void ts_node_descendant_for_point_range_wasm(TSNode node, TSPoint start, TSPoint end,
        TSNode* child);

    // [LibraryImport(LibTreeSitter)]
    // internal static partial TsNode ts_node_named_descendant_for_byte_range(TsNode node, uint start, uint end);

    [LibraryImport(LibTreeSitter)]
    internal static partial void ts_node_named_descendant_for_byte_range_wasm(TSNode node, uint start, uint end,
        TSNode* child);

    // [LibraryImport(LibTreeSitter)]
    // internal static partial TsNode ts_node_named_descendant_for_point_range(TsNode node, TSPoint start, TSPoint end);

    [LibraryImport(LibTreeSitter)]
    internal static unsafe partial void ts_node_named_descendant_for_point_range_wasm(
        TSNode node, TSPoint start, TSPoint end, TSNode* result);

    [LibraryImport(LibTreeSitter)]
    internal static partial void ts_node_edit(TSNode node, ref TSEdit edit);


    [LibraryImport(LibTreeSitter)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool ts_node_eq(TSNode node, TSNode other);

    #endregion

    #region ====Tree Cursor====

    // [LibraryImport(LibTreeSitter)]
    // internal static partial TsTreeCursor ts_tree_cursor_new(TsNode node);

    [LibraryImport(LibTreeSitter)]
    internal static unsafe partial void ts_tree_cursor_new_wasm(TSNode node, TsTreeCursor* cursor);

    [LibraryImport(LibTreeSitter)]
    internal static partial void ts_tree_cursor_delete(ref TsTreeCursor cursor);

    [LibraryImport(LibTreeSitter)]
    internal static partial TsTreeCursor ts_tree_cursor_reset(
        ref TsTreeCursor self,
        TSNode node
    );

    // [LibraryImport(LibTreeSitter)]
    // internal static partial TsNode ts_tree_cursor_current_node(ref TsTreeCursor self);

    [LibraryImport(LibTreeSitter)]
    internal static unsafe partial void ts_tree_cursor_current_node_wasm(ref TsTreeCursor self, TSNode* node);

    [LibraryImport(LibTreeSitter)]
    internal static partial IntPtr ts_tree_cursor_current_field_name(ref TsTreeCursor self);

    [LibraryImport(LibTreeSitter)]
    internal static partial ushort ts_tree_cursor_current_field_id(ref TsTreeCursor self);

    [LibraryImport(LibTreeSitter)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool ts_tree_cursor_goto_parent(ref TsTreeCursor self);

    [LibraryImport(LibTreeSitter)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool ts_tree_cursor_goto_next_sibling(ref TsTreeCursor self);

    [LibraryImport(LibTreeSitter)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool ts_tree_cursor_goto_first_child(ref TsTreeCursor self);

    [LibraryImport(LibTreeSitter)]
    internal static partial ulong ts_tree_cursor_goto_first_child_for_byte(ref TsTreeCursor self, uint offset);

    [LibraryImport(LibTreeSitter)]
    internal static partial TsTreeCursor ts_tree_cursor_copy(ref TsTreeCursor self);

    #endregion

    #region ====Query====

    [LibraryImport(LibTreeSitter)]
    internal static partial IntPtr ts_query_new(IntPtr language, IntPtr source,
        uint sourceLength, ref uint errorOffset, ref TsQueryError errorType);

    [LibraryImport(LibTreeSitter)]
    internal static partial void ts_query_delete(IntPtr query);

    [LibraryImport(LibTreeSitter)]
    internal static partial uint ts_query_pattern_count(IntPtr query);

    [LibraryImport(LibTreeSitter)]
    internal static partial uint ts_query_capture_count(IntPtr query);

    [LibraryImport(LibTreeSitter)]
    internal static partial uint ts_query_string_count(IntPtr query);

    [LibraryImport(LibTreeSitter)]
    internal static partial IntPtr ts_query_capture_name_for_id(IntPtr query, uint id, ref uint length);

    [LibraryImport(LibTreeSitter)]
    internal static partial uint ts_query_start_byte_for_pattern(IntPtr query, uint pattern_index);

    [LibraryImport(LibTreeSitter)]
    internal static unsafe partial TsQueryPredicateStep* ts_query_predicates_for_pattern(
        IntPtr query, uint patternIndex, ref uint length);

    [LibraryImport(LibTreeSitter)]
    internal static partial IntPtr ts_query_cursor_new();

    [LibraryImport(LibTreeSitter)]
    internal static partial void ts_query_cursor_delete(IntPtr queryCursor);

    [LibraryImport(LibTreeSitter)]
    internal static partial void ts_query_cursor_set_match_limit(IntPtr queryCursor, uint limit);

    [LibraryImport(LibTreeSitter)]
    internal static partial void ts_query_cursor_set_point_range(IntPtr queryCursor,
        TSPoint start, TSPoint end);

    [LibraryImport(LibTreeSitter)]
    internal static partial void ts_query_cursor_exec(IntPtr queryCursor, IntPtr query, TSNode node);

    [LibraryImport(LibTreeSitter)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool ts_query_cursor_next_capture(IntPtr queryCursor,
        ref TsQueryMatch match, ref uint captureIndex);

    #endregion

    #region ====Language====

    [LibraryImport(LibTreeSitter)]
    public static partial uint ts_language_symbol_count(IntPtr language);

    [LibraryImport(LibTreeSitter)]
    public static partial IntPtr ts_language_symbol_name(IntPtr language, ushort symbol);

    [LibraryImport(LibTreeSitter)]
    public static partial ushort ts_language_symbol_for_name(
        IntPtr language,
        IntPtr name,
        uint length,
        [MarshalAs(UnmanagedType.Bool)] bool isNamed
    );

    [LibraryImport(LibTreeSitter)]
    public static partial ushort ts_language_field_count(IntPtr language);

    [LibraryImport(LibTreeSitter)]
    public static partial IntPtr ts_language_field_name_for_id(IntPtr language, ushort fieldId);

    [LibraryImport(LibTreeSitter)]
    public static partial ushort ts_language_field_id_for_name(IntPtr language, IntPtr name, uint length);

    [LibraryImport(LibTreeSitter)]
    public static partial TsSymbolType ts_language_symbol_type(IntPtr language, ushort symbol);

    [LibraryImport(LibTreeSitter)]
    public static partial uint ts_language_version(IntPtr language);

    #endregion
}
#endif