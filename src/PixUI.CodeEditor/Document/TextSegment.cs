using System;
using System.Diagnostics;

namespace CodeEditor;

/// <summary>
/// A segment that can be put into a <see cref="TextSegmentTree{T}"/>.
/// </summary>
/// <remarks>
/// <para>
/// A <see cref="TextSegment"/> can be stand-alone or part of a <see cref="TextSegmentTree{T}"/>.
/// If the segment is stored inside a TextSegmentCollection, its Offset and Length will be updated by that collection.
/// </para>
/// <para>
/// When the document changes, the offsets of all text segments in the TextSegmentCollection will be adjusted accordingly.
/// Start offsets move like <see cref="AnchorMovementType">AnchorMovementType.AfterInsertion</see>,
/// end offsets move like <see cref="AnchorMovementType">AnchorMovementType.BeforeInsertion</see>
/// (i.e. the segment will always stay as small as possible).</para>
/// <para>
/// If a document change causes a segment to be deleted completely, it will be reduced to length 0, but segments are
/// never automatically removed from the collection.
/// Segments with length 0 will never expand due to document changes, and they move as <c>AfterInsertion</c>.
/// </para>
/// <para>
/// Thread-safety: a TextSegmentCollection that is connected to a <see cref="Document"/> may only be used on that document's owner thread.
/// A disconnected TextSegmentCollection is safe for concurrent reads, but concurrent access is not safe when there are writes.
/// Keep in mind that reading the Offset properties of a text segment inside the collection is a read access on the
/// collection; and setting an Offset property of a text segment is a write access on the collection.
/// </para>
/// </remarks>
/// <seealso cref="ISegment"/>
/// <seealso cref="AnchorSegment"/>
/// <seealso cref="TextSegmentTree{T}"/>
public class TextSegment : IRedBlackTreeNode<TextSegment>, ISegment
{
    internal ISegmentTree? OwnerTree { get; set; }
    public TextSegment? Left { get; set; }
    public TextSegment? Right { get; set; }
    public TextSegment? Parent { get; set; }

    /// <summary>
    /// The color of the segment in the red/black tree.
    /// </summary>
    public bool Color { get; set; }

    /// <summary>
    /// The "length" of the node (distance to previous node)
    /// </summary>
    internal int NodeLength { get; set; }

    /// <summary>
    /// The total "length" of this subtree.
    /// </summary>
    /// <remarks>totalNodeLength = nodeLength + left.totalNodeLength + right.totalNodeLength</remarks>
    internal int TotalNodeLength { get; set; }

    /// <summary>
    /// The length of the segment (do not confuse with nodeLength).
    /// </summary>
    internal int SegmentLength { get; set; }

    /// <summary>
    /// distanceToMaxEnd = Max(segmentLength,
    ///                        left.distanceToMaxEnd + left.Offset - Offset,
    ///                        left.distanceToMaxEnd + right.Offset - Offset)
    /// </summary>
    internal int DistanceToMaxEnd { get; set; }

    int ISegment.Offset
    {
        get => StartOffset;
        set => throw new NotSupportedException();
    }

    /// <summary>
    /// Gets whether this segment is connected to a TextSegmentCollection and will automatically
    /// update its offsets.
    /// </summary>
    protected bool IsConnectedToCollection => OwnerTree != null;

    /// <summary>
    /// Gets/Sets the start offset of the segment.
    /// </summary>
    /// <remarks>
    /// When setting the start offset, the end offset will change, too: the Length of the segment will stay constant.
    /// </remarks>
    public int StartOffset
    {
        get
        {
            // If the segment is not connected to a tree, we store the offset in "nodeLength".
            // Otherwise, "nodeLength" contains the distance to the start offset of the previous node
            Debug.Assert(!(OwnerTree == null && Parent != null));
            Debug.Assert(!(OwnerTree == null && Left != null));

            var n = this;
            var offset = n.NodeLength;
            if (n.Left != null)
                offset += n.Left.TotalNodeLength;
            while (n.Parent != null)
            {
                if (n == n.Parent.Right)
                {
                    if (n.Parent.Left != null)
                        offset += n.Parent.Left.TotalNodeLength;
                    offset += n.Parent.NodeLength;
                }

                n = n.Parent;
            }

            return offset;
        }
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Offset must not be negative");
            if (StartOffset != value)
            {
                // need a copy of the variable because ownerTree.Remove() sets this.ownerTree to null
                var ownerTree = OwnerTree;
                if (ownerTree != null)
                {
                    ownerTree.Remove(this);
                    NodeLength = value;
                    ownerTree.Add(this);
                }
                else
                {
                    NodeLength = value;
                }

                OnSegmentChanged();
            }
        }
    }

    /// <summary>
    /// Gets/Sets the end offset of the segment.
    /// </summary>
    /// <remarks>
    /// Setting the end offset will change the length, the start offset will stay constant.
    /// </remarks>
    public int EndOffset
    {
        get => StartOffset + Length;
        set
        {
            var newLength = value - StartOffset;
            if (newLength < 0)
                throw new ArgumentOutOfRangeException(nameof(value),
                    "EndOffset must be greater or equal to StartOffset");
            Length = newLength;
        }
    }

    /// <summary>
    /// Gets/Sets the length of the segment.
    /// </summary>
    /// <remarks>
    /// Setting the length will change the end offset, the start offset will stay constant.
    /// </remarks>
    public int Length
    {
        get => SegmentLength;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Length must not be negative");
            if (SegmentLength != value)
            {
                SegmentLength = value;
                OwnerTree?.UpdateAugmentedData(this);
                OnSegmentChanged();
            }
        }
    }

    /// <summary>
    /// This method gets called when the StartOffset/Length/EndOffset properties are set.
    /// It is not called when StartOffset/Length/EndOffset change due to document changes
    /// </summary>
    protected virtual void OnSegmentChanged() { }

#if DEBUG
    internal string ToDebugString()
    {
        return
            $"[nodeLength={NodeLength} totalNodeLength={TotalNodeLength} distanceToMaxEnd={DistanceToMaxEnd} MaxEndOffset={StartOffset + DistanceToMaxEnd}]";
    }
#endif

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"[{GetType().Name} Offset={StartOffset} Length={Length} EndOffset={EndOffset}]";
    }
}