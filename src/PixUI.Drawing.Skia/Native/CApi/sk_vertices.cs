using System.Runtime.InteropServices;

namespace PixUI.Drawing.Skia;

unsafe partial class SkiaApi
{
    // sk_vertices_t* sk_vertices_make_copy(sk_vertices_vertex_mode_t vmode, int vertexCount, const sk_point_t* positions, const sk_point_t* texs, const sk_color_t* colors, int indexCount, const uint16_t* indices)

    [LibraryImport(SKIA)]
    internal static partial sk_vertices_t sk_vertices_make_copy(SKVertexMode vmode, Int32 vertexCount, Point* positions,
        Point* texs, UInt32* colors, Int32 indexCount, UInt16* indices);


    // void sk_vertices_ref(sk_vertices_t* cvertices)

    [LibraryImport(SKIA)]
    internal static partial void sk_vertices_ref(sk_vertices_t cvertices);


    // void sk_vertices_unref(sk_vertices_t* cvertices)

    [LibraryImport(SKIA)]
    internal static partial void sk_vertices_unref(sk_vertices_t cvertices);
}