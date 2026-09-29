using System.Runtime.InteropServices;

namespace PixUI.Drawing.Skia;

unsafe partial class SkiaApi
{
    // void sk_runtimeeffect_get_child_name(const sk_runtimeeffect_t* effect, int index, sk_string_t* name)

    [LibraryImport(SKIA)]
    internal static partial void sk_runtimeeffect_get_child_name(sk_runtimeeffect_t effect, Int32 index,
        sk_string_t name);


    // size_t sk_runtimeeffect_get_children_count(const sk_runtimeeffect_t* effect)

    [LibraryImport(SKIA)]
    internal static partial /* size_t */ IntPtr sk_runtimeeffect_get_children_count(sk_runtimeeffect_t effect);


    // const sk_runtimeeffect_uniform_t* sk_runtimeeffect_get_uniform_from_index(const sk_runtimeeffect_t* effect, int index)

    [LibraryImport(SKIA)]
    internal static partial sk_runtimeeffect_uniform_t sk_runtimeeffect_get_uniform_from_index(
        sk_runtimeeffect_t effect, Int32 index);


    // const sk_runtimeeffect_uniform_t* sk_runtimeeffect_get_uniform_from_name(const sk_runtimeeffect_t* effect, const char* name, size_t len)

    [LibraryImport(SKIA)]
    internal static partial sk_runtimeeffect_uniform_t sk_runtimeeffect_get_uniform_from_name(
        sk_runtimeeffect_t effect, /* char */ void* name, /* size_t */ IntPtr len);


    // void sk_runtimeeffect_get_uniform_name(const sk_runtimeeffect_t* effect, int index, sk_string_t* name)

    [LibraryImport(SKIA)]
    internal static partial void sk_runtimeeffect_get_uniform_name(sk_runtimeeffect_t effect, Int32 index,
        sk_string_t name);


    // size_t sk_runtimeeffect_get_uniform_size(const sk_runtimeeffect_t* effect)

    [LibraryImport(SKIA)]
    internal static partial /* size_t */ IntPtr sk_runtimeeffect_get_uniform_size(sk_runtimeeffect_t effect);


    // size_t sk_runtimeeffect_get_uniforms_count(const sk_runtimeeffect_t* effect)

    [LibraryImport(SKIA)]
    internal static partial /* size_t */ IntPtr sk_runtimeeffect_get_uniforms_count(sk_runtimeeffect_t effect);


    // sk_runtimeeffect_t* sk_runtimeeffect_make(sk_string_t* sksl, sk_string_t* error)

    [LibraryImport(SKIA)]
    internal static partial sk_runtimeeffect_t sk_runtimeeffect_make(sk_string_t sksl, sk_string_t error);


    // sk_colorfilter_t* sk_runtimeeffect_make_color_filter(sk_runtimeeffect_t* effect, sk_data_t* uniforms, sk_colorfilter_t** children, size_t childCount)

    [LibraryImport(SKIA)]
    internal static partial sk_colorfilter_t sk_runtimeeffect_make_color_filter(sk_runtimeeffect_t effect,
        sk_data_t uniforms, sk_colorfilter_t* children, /* size_t */ IntPtr childCount);


    // sk_shader_t* sk_runtimeeffect_make_shader(sk_runtimeeffect_t* effect, sk_data_t* uniforms, sk_shader_t** children, size_t childCount, const sk_matrix_t* localMatrix, bool isOpaque)

    [LibraryImport(SKIA)]
    internal static partial sk_shader_t sk_runtimeeffect_make_shader(sk_runtimeeffect_t effect, sk_data_t uniforms,
        sk_shader_t* children, /* size_t */ IntPtr childCount, Matrix3* localMatrix,
        [MarshalAs(UnmanagedType.I1)] bool isOpaque);


    // size_t sk_runtimeeffect_uniform_get_offset(const sk_runtimeeffect_uniform_t* variable)

    [LibraryImport(SKIA)]
    internal static partial /* size_t */
        IntPtr sk_runtimeeffect_uniform_get_offset(sk_runtimeeffect_uniform_t variable);


    // size_t sk_runtimeeffect_uniform_get_size_in_bytes(const sk_runtimeeffect_uniform_t* variable)

    [LibraryImport(SKIA)]
    internal static partial /* size_t */
        IntPtr sk_runtimeeffect_uniform_get_size_in_bytes(sk_runtimeeffect_uniform_t variable);


    // void sk_runtimeeffect_unref(sk_runtimeeffect_t* effect)

    [LibraryImport(SKIA)]
    internal static partial void sk_runtimeeffect_unref(sk_runtimeeffect_t effect);
}