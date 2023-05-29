namespace PixUI;

/// <summary>
/// The status of an animation.
/// </summary>
public enum AnimationStatus
{
    /// <summary>
    /// The animation is stopped at the beginning.
    /// </summary>
    Dismissed,

    /// <summary>
    /// The animation is running from beginning to end.
    /// </summary>
    Forward,

    /// <summary>
    /// The animation is running backwards, from end to beginning.
    /// </summary>
    Reverse,

    /// <summary>
    /// The animation is stopped at the end.
    /// </summary>
    Completed,
}