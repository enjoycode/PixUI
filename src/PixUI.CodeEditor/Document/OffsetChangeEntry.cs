using System;

namespace CodeEditor;

/// <summary>
/// This represents the offset of a document change (either insertion or removal, not both at once).
/// </summary>
public readonly struct OffsetChangeEntry : IEquatable<OffsetChangeEntry>
{
    // MSB: DefaultAnchorMovementIsBeforeInsertion
    private readonly uint _insertionLengthWithMovementFlag;

    // MSB: RemovalNeverCausesAnchorDeletion; other 31 bits: RemovalLength
    private readonly uint _removalLengthWithDeletionFlag;

    /// <summary>
    /// The offset at which the change occurs.
    /// </summary>
    public int Offset { get; }

    /// <summary>
    /// The number of characters inserted.
    /// Returns 0 if this entry represents a removal.
    /// </summary>
    public int InsertionLength => (int)(_insertionLengthWithMovementFlag & 0x7fffffff);

    /// <summary>
    /// The number of characters removed.
    /// Returns 0 if this entry represents an insertion.
    /// </summary>
    public int RemovalLength => (int)(_removalLengthWithDeletionFlag & 0x7fffffff);

    /// <summary>
    /// Gets whether the removal should not cause any anchor deletions.
    /// </summary>
    public bool RemovalNeverCausesAnchorDeletion => (_removalLengthWithDeletionFlag & 0x80000000) != 0;

    /// <summary>
    /// Gets whether default anchor movement causes the anchor to stay in front of the caret.
    /// </summary>
    public bool DefaultAnchorMovementIsBeforeInsertion => (_insertionLengthWithMovementFlag & 0x80000000) != 0;

    /// <summary>
    /// Gets the new offset where the specified offset moves after this document change.
    /// </summary>
    public int GetNewOffset(int oldOffset, AnchorMovementType movementType = AnchorMovementType.Default)
    {
        int insertionLength = InsertionLength;
        int removalLength = RemovalLength;
        if (!(removalLength == 0 && oldOffset == Offset))
        {
            // we're getting trouble (both if statements in here would apply)
            // if there's no removal and we insert at the offset
            // -> we'd need to disambiguate by movementType, which is handled after the if

            // offset is before start of change: no movement
            if (oldOffset <= Offset)
                return oldOffset;
            // offset is after end of change: movement by normal delta
            if (oldOffset >= Offset + removalLength)
                return oldOffset + insertionLength - removalLength;
        }

        // we reach this point if
        // a) the oldOffset is inside the deleted segment
        // b) there was no removal, and we insert at the caret position
        if (movementType == AnchorMovementType.AfterInsertion)
            return Offset + insertionLength;
        if (movementType == AnchorMovementType.BeforeInsertion)
            return Offset;
        return DefaultAnchorMovementIsBeforeInsertion ? Offset : Offset + insertionLength;
    }

    /// <summary>
    /// Creates a new OffsetChangeMapEntry instance.
    /// </summary>
    public OffsetChangeEntry(int offset, int removalLength, int insertionLength)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset);
        ArgumentOutOfRangeException.ThrowIfNegative(removalLength);
        ArgumentOutOfRangeException.ThrowIfNegative(insertionLength);

        Offset = offset;
        _removalLengthWithDeletionFlag = (uint)removalLength;
        _insertionLengthWithMovementFlag = (uint)insertionLength;
    }

    /// <summary>
    /// Creates a new OffsetChangeMapEntry instance.
    /// </summary>
    public OffsetChangeEntry(int offset, int removalLength, int insertionLength,
        bool removalNeverCausesAnchorDeletion, bool defaultAnchorMovementIsBeforeInsertion)
        : this(offset, removalLength, insertionLength)
    {
        if (removalNeverCausesAnchorDeletion)
            _removalLengthWithDeletionFlag |= 0x80000000;
        if (defaultAnchorMovementIsBeforeInsertion)
            _insertionLengthWithMovementFlag |= 0x80000000;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        unchecked
        {
            return Offset + 3559 * (int)_insertionLengthWithMovementFlag + 3571 * (int)_removalLengthWithDeletionFlag;
        }
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is OffsetChangeEntry && Equals((OffsetChangeEntry)obj);
    }

    /// <inheritdoc/>
    public bool Equals(OffsetChangeEntry other)
    {
        return Offset == other.Offset && _insertionLengthWithMovementFlag == other._insertionLengthWithMovementFlag &&
               _removalLengthWithDeletionFlag == other._removalLengthWithDeletionFlag;
    }

    /// <summary>
    /// Tests the two entries for equality.
    /// </summary>
    public static bool operator ==(OffsetChangeEntry left, OffsetChangeEntry right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Tests the two entries for inequality.
    /// </summary>
    public static bool operator !=(OffsetChangeEntry left, OffsetChangeEntry right)
    {
        return !left.Equals(right);
    }
}