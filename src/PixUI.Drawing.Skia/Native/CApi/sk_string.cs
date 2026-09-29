using System.Runtime.InteropServices;

namespace PixUI.Drawing.Skia;

unsafe partial class SkiaApi
{
    // void sk_string_destructor(const sk_string_t*)

    [LibraryImport(SKIA)]
    internal static partial void sk_string_destructor(sk_string_t param0);


    // const char* sk_string_get_c_str(const sk_string_t*)

    [LibraryImport(SKIA)]
    internal static partial /* char */ void* sk_string_get_c_str(sk_string_t param0);


    // size_t sk_string_get_size(const sk_string_t*)

    [LibraryImport(SKIA)]
    internal static partial /* size_t */ IntPtr sk_string_get_size(sk_string_t param0);


    // sk_string_t* sk_string_new_empty()

    [LibraryImport(SKIA)]
    internal static partial sk_string_t sk_string_new_empty();


    // sk_string_t* sk_string_new_with_copy(const char* src, size_t length)

    [LibraryImport(SKIA)]
    internal static partial sk_string_t sk_string_new_with_copy( /* char */ void* src, /* size_t */ IntPtr length);
}