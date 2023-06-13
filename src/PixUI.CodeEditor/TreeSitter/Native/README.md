
# Build TreeSitter

## Mac
```shell
clang -fPIC -shared lib/src/lib.c -Ilib/src -Ilib/include -std=c99 -O3 -o libtree-sitter.dylib
```
## For Blazor WebAssembly
因目前blazor的native回调不支持封送struct，所以暂修改源码回调参数TSPoint改为指针
api.h
```c++
typedef struct {
  void *payload;
  const char *(*read)(void *payload, uint32_t byte_index, TSPoint* position /*TSPoint position*/, uint32_t *bytes_read);
  TSInputEncoding encoding;
} TSInput;
```

lexer.c
```c++
// Call the lexer's input callback to obtain a new chunk of source code
// for the current position.
static void ts_lexer__get_chunk(Lexer *self) {
  self->chunk_start = self->current_position.bytes;
  self->chunk = self->input.read(
    self->input.payload,
    self->current_position.bytes,
    &self->current_position.extent, //转换为指针
    &self->chunk_size
  );
  if (!self->chunk_size) {
    self->current_included_range_index = self->included_range_count;
    self->chunk = NULL;
  }
}
```

另因为不支持返回struct，所以修改lib.c添加包装方法
```c++
//====Rick Add For Blazor WASM====
void ts_util_free(void* ptr) {
	free(ptr);
}

void ts_tree_root_node_wasm(const TSTree *tree, TSNode* node) {
	*node = ts_tree_root_node(tree);
}

void ts_tree_cursor_new_wasm(TSNode node, TSTreeCursor* cursor) {
	*cursor = ts_tree_cursor_new(node);
}

void ts_node_end_point_wasm(TSNode node, TSPoint* point) {
	*point = ts_node_end_point(node);
}

void ts_node_parent_wasm(TSNode node, TSNode* parent) {
	*parent = ts_node_parent(node);
}

void ts_node_child_wasm(TSNode node, uint32_t index, TSNode* child) {
	*child = ts_node_child(node, index);
}

void ts_node_named_child_wasm(TSNode node, uint32_t index, TSNode* child) {
	*child = ts_node_named_child(node, index);
}

void ts_node_child_by_field_name_wasm(TSNode node, const char* name, size_t nameLen, TSNode* child) {
	*child = ts_node_child_by_field_name(node, name, nameLen);
}

void ts_node_child_by_field_id_wasm(TSNode node, TSFieldId id, TSNode* child) {
	*child = ts_node_child_by_field_id(node, id);
}

void ts_node_next_sibling_wasm(TSNode node, TSNode* next) {
	*next = ts_node_next_sibling(node);
}

void ts_node_prev_sibling_wasm(TSNode node, TSNode* prev) {
	*prev = ts_node_prev_sibling(node);
}

void ts_node_named_descendant_for_point_range_wasm(TSNode node, TSPoint start, TSPoint end, TSNode* result) {
	*result = ts_node_named_descendant_for_point_range(node, start, end);
}

void ts_node_next_named_sibling_wasm(TSNode node, TSNode* next) {
	*next = ts_node_next_named_sibling(node);
}

void ts_node_prev_named_sibling_wasm(TSNode node, TSNode* prev) {
	*prev = ts_node_prev_named_sibling(node);
}

void ts_node_first_child_for_byte_wasm(TSNode node, uint32_t byteOffset, TSNode* child) {
	*child = ts_node_first_child_for_byte(node, byteOffset);
}

void ts_node_first_named_child_for_byte_wasm(TSNode node, uint32_t offset, TSNode* child) {
	*child = ts_node_first_named_child_for_byte(node, offset);
}

void ts_node_descendant_for_byte_range_wasm(TSNode node, uint32_t start, uint32_t end, TSNode* child) {
	*child = ts_node_descendant_for_byte_range(node, start, end);
}

void ts_node_named_descendant_for_byte_range_wasm(TSNode node, uint32_t start, uint32_t end, TSNode* child) {
	*child = ts_node_named_descendant_for_byte_range(node, start, end);
}

void ts_node_descendant_for_point_range_wasm(TSNode node, TSPoint start, TSPoint end, TSNode* child) {
	*child = ts_node_descendant_for_point_range(node, start, end);
}

void ts_tree_cursor_current_node_wasm(const TSTreeCursor* cursor, TSNode* node) {
	*node = ts_tree_cursor_current_node(cursor);
}
```

```shell
emcc lib/src/lib.c -Ilib/src -Ilib/include -std=c99 -fno-exceptions -O3 --no-entry -c -o tree-sitter.o
```

parser.c
```c
static const char *ts_string_input_read(
  void *_self,
  uint32_t byte,
  TSPoint* pt, /*Rick TSPoint pt*/
  uint32_t *length
) {
  (void)pt;
  TSStringInput *self = (TSStringInput *)_self;
  if (byte >= self->length) {
    *length = 0;
    return "";
  } else {
    *length = self->length - byte;
    return self->string + byte;
  }
}
```

## Windows

### MSVC

* 新建输出符号表 lib.def文件:

```text
LIBRARY   tree-sitter
EXPORTS
   ts_parser_new
   ts_parser_delete
   ts_parser_set_language
   ts_parser_language
   ts_parser_set_included_ranges
   ts_parser_included_ranges
   ts_parser_parse
   ts_parser_parse_string
   ts_parser_parse_string_encoding
   ts_parser_reset
   ts_parser_set_timeout_micros
   ts_parser_timeout_micros
   ts_parser_set_cancellation_flag
   ts_parser_set_logger
   ts_parser_print_dot_graphs
   ts_tree_copy
   ts_tree_delete
   ts_tree_root_node
   ts_tree_language
   ts_tree_edit
   ts_tree_get_changed_ranges
   ts_node_type
   ts_node_symbol
   ts_node_start_byte
   ts_node_start_point
   ts_node_end_byte
   ts_node_end_point
   ts_node_string
   ts_node_is_null
   ts_node_is_named
   ts_node_is_missing
   ts_node_is_extra
   ts_node_has_changes
   ts_node_has_error
   ts_node_parent
   ts_node_child
   ts_node_child_count
   ts_node_named_child
   ts_node_named_child_count
   ts_node_child_by_field_name
   ts_node_child_by_field_id
   ts_node_next_sibling
   ts_node_prev_sibling
   ts_node_next_named_sibling
   ts_node_prev_named_sibling
   ts_node_first_child_for_byte
   ts_node_first_named_child_for_byte
   ts_node_descendant_for_byte_range
   ts_node_descendant_for_point_range
   ts_node_named_descendant_for_byte_range
   ts_node_named_descendant_for_point_range
   ts_node_edit
   ts_node_eq
   ts_tree_cursor_new
   ts_tree_cursor_delete
   ts_tree_cursor_reset
   ts_tree_cursor_current_node
   ts_tree_cursor_current_field_name
   ts_tree_cursor_current_field_id
   ts_tree_cursor_goto_parent
   ts_tree_cursor_goto_next_sibling
   ts_tree_cursor_goto_first_child
   ts_tree_cursor_goto_first_child_for_byte
   ts_tree_cursor_copy
   ts_query_new
   ts_query_delete
   ts_query_pattern_count
   ts_query_capture_count
   ts_query_string_count
   ts_query_capture_name_for_id
   ts_query_predicates_for_pattern
   ts_query_cursor_new
   ts_query_cursor_delete
   ts_query_cursor_set_match_limit
   ts_query_cursor_set_point_range
   ts_query_cursor_exec
   ts_query_cursor_next_capture
   ts_language_symbol_count
   ts_language_symbol_name
   ts_language_symbol_for_name
   ts_language_field_count
   ts_language_field_name_for_id
   ts_language_field_id_for_name
   ts_language_symbol_type
   ts_language_version
```

* 根据arch在命令行编译:

```shell
cl lib/src/lib.c /I lib/src /I lib/include /O2 /link /dll /def:lib.def /out:tree-sitter.dll
```

# Build C# Language Native

## MAC
```shell
cd tree-sitter-c-sharp
clang -fPIC -shared -o ~/Desktop/libtree-sitter-csharp.dylib src/parser.c src/scanner.c -Isrc -O3
```

## For Blazor WebAssembly
```shell
emcc src/parser.c -Isrc -std=c99 -fno-exceptions -O3 -c -o csparser.o
emcc src/scanner.c -Isrc -std=c99 -fno-exceptions -O3 -c -o csscanner.o
emar cr tree-sitter-csharp.a csparser.o csscanner.o
```

## WINDOWS

### MSVC

* 根据arch在命令行编译:

```shell
cl src/parser.c src/scanner.c /I src /O2 /link /dll /out:tree-sitter-csharp.dll
```

# Generate C# Language WASM

https://github.com/tree-sitter/tree-sitter/tree/master/lib/binding_web

The following example shows how to generate .wasm file for tree-sitter JavaScript grammar.

IMPORTANT: emscripten or docker need to be installed.

First install tree-sitter-cli and the tree-sitter language for which to generate .wasm (tree-sitter-c-sharp in this example):

```shell
npm install --save-dev tree-sitter-cli tree-sitter-c-sharp
```

Then just use tree-sitter cli tool to generate the .wasm.

```shell
npx tree-sitter build-wasm node_modules/tree-sitter-c-sharp
```