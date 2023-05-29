#if __WEB__
using System;

namespace PixUI
{
    [TSType("CanvasKit.PathEffect")]
    public class PathEffect
    {
        private PathEffect() {}
        
        [TSTemplate("CanvasKit.PathEffect.MakeDiscrete({1},{2},{3})")]
        public static PathEffect CreateDiscrete(float segLength, float deviation, uint seedAssist) => throw new Exception();

        [TSTemplate("CanvasKit.PathEffect.MakeCorner({1})")]
        public static PathEffect CreateCorner(float radius) => throw new Exception();
        
        [TSTemplate("CanvasKit.PathEffect.MakePath1D({1},{2},{3},{4})")]
        public static PathEffect Create1DPath(Path path, float advance, float phase,
            Path1DPathEffectStyle style) => throw new Exception();

        [TSTemplate("CanvasKit.PathEffect.MakeLine2D({1},{2})")]
        public static PathEffect Create2DLine(float width, Matrix3 matrix) => throw new Exception();

        [TSTemplate("CanvasKit.PathEffect.MakePath2D({1},{2})")]
        public static PathEffect Create2DPath(Matrix3 matrix, Path path) => throw new Exception();

        [TSTemplate("CanvasKit.PathEffect.MakeDash(Array.from({1}),{2})")]
        public static PathEffect CreateDash(float[] intervals, float phase) => throw new Exception();
        
        [TSRename("delete")]
        public void Dispose() {}
    }
}
#endif