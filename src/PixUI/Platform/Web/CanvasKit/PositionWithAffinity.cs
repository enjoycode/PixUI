#if __WEB__
namespace PixUI
{
    [TSType("CanvasKit.PositionWithAffinity")]
    public sealed class PositionWithAffinity
    {
        [TSRename("pos")]
        public readonly int Position;

        [TSRename("affinity")]
        public readonly Affinity Affinity;

        internal PositionWithAffinity(int pos, int affinity)
        {
            Position = pos;
            Affinity = (Affinity)affinity;
        }
    }
}
#endif