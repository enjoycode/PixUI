#if __WEB__
using System;

namespace PixUI
{
    [TSType("CanvasKit.Path")]
    public sealed class Path : IDisposable
    {
        [TSTemplate("new CanvasKit.Path()")]
        public Path() { }

        [TSRename("isEmpty")]
        public bool IsEmpty() => false;

        [TSRename("addRect")]
        public void AddRect(Rect rect, bool isCCW = false) { }

        [TSRename("addRRect")]
        public void AddRRect(RRect rect, bool isCCW = false) { }

        [TSRename("moveTo")]
        public void MoveTo(float x, float y) { }

        [TSRename("lineTo")]
        public void LineTo(float x, float y) { }

        [TSRename("arcToOval")]
        public void ArcTo(Rect oval, float startAngle, float sweepAngle, bool forceMoveTo) { }

        [TSRename("arcToRotated")]
        public void ArcTo(float rx, float ry, float xAxisRotate, bool useSmallArc, bool isCCW, float x, float y) {}
        
        [TSRename("cubicTo")]
        public void CubicTo(float x0, float y0, float x1, float y1, float x2, float y2) {}
        
        [TSRename("offset")]
        public void Offset(float dx, float dy) {}

        [TSRename("op")]
        public bool Op(Path other, PathOp pathOp) => true;
        
        [TSTemplate("PixUI.Rect.FromFloat32Array({0}.getBounds())")]
        public Rect GetBounds() => Rect.Empty;
        
        [TSRename("close")]
        public void Close() {}

        [TSRename("delete")]
        public void Dispose() { }
    }
}
#endif