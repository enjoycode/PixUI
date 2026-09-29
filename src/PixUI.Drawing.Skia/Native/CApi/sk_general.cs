using System.Runtime.InteropServices;

namespace PixUI.Drawing.Skia;

unsafe partial class SkiaApi
{
    // sk_colortype_t sk_colortype_get_default_8888()

    [LibraryImport(SKIA)]
    internal static partial SKColorTypeNative sk_colortype_get_default_8888();


    // int sk_nvrefcnt_get_ref_count(const sk_nvrefcnt_t* refcnt)

    [LibraryImport(SKIA)]
    internal static partial Int32 sk_nvrefcnt_get_ref_count(sk_nvrefcnt_t refcnt);


    // void sk_nvrefcnt_safe_ref(sk_nvrefcnt_t* refcnt)

    [LibraryImport(SKIA)]
    internal static partial void sk_nvrefcnt_safe_ref(sk_nvrefcnt_t refcnt);


    // void sk_nvrefcnt_safe_unref(sk_nvrefcnt_t* refcnt)

    [LibraryImport(SKIA)]
    internal static partial void sk_nvrefcnt_safe_unref(sk_nvrefcnt_t refcnt);


    // bool sk_nvrefcnt_unique(const sk_nvrefcnt_t* refcnt)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_nvrefcnt_unique(sk_nvrefcnt_t refcnt);


    // int sk_refcnt_get_ref_count(const sk_refcnt_t* refcnt)

    [LibraryImport(SKIA)]
    internal static partial Int32 sk_refcnt_get_ref_count(sk_refcnt_t refcnt);


    // void sk_refcnt_safe_ref(sk_refcnt_t* refcnt)

    [LibraryImport(SKIA)]
    internal static partial void sk_refcnt_safe_ref(sk_refcnt_t refcnt);


    // void sk_refcnt_safe_unref(sk_refcnt_t* refcnt)

    [LibraryImport(SKIA)]
    internal static partial void sk_refcnt_safe_unref(sk_refcnt_t refcnt);


    // bool sk_refcnt_unique(const sk_refcnt_t* refcnt)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_refcnt_unique(sk_refcnt_t refcnt);


    // int sk_version_get_increment()

    [LibraryImport(SKIA)]
    internal static partial Int32 sk_version_get_increment();


    // int sk_version_get_milestone()

    [LibraryImport(SKIA)]
    internal static partial Int32 sk_version_get_milestone();


    // const char* sk_version_get_string()

    [LibraryImport(SKIA)]
    internal static partial /* char */ void* sk_version_get_string();
}