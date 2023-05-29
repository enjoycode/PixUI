namespace PixUI;

/// <summary>
/// Structure that specifies maximum allowable magnitudes for distances,
/// durations, and velocity differences to be considered equal.
/// </summary>
public sealed class Tolerance //TODO: maybe struct
{
    private const double EpsilonDefault = 1e-3;

    public static readonly Tolerance Default = new Tolerance();

    /// <summary>
    /// The magnitude of the maximum distance between two points for them to be
    /// considered within tolerance.
    /// </summary>
    /// <remarks>
    /// The units for the distance tolerance must be the same as the units used
    /// for the distances that are to be compared to this tolerance.
    /// </remarks>
    public readonly double Distance;


    /// <summary>
    /// The magnitude of the maximum duration between two times for them to be
    /// considered within tolerance.
    /// </summary>
    /// <remarks>
    /// The units for the time tolerance must be the same as the units used
    /// for the times that are to be compared to this tolerance.
    /// </remarks>
    public readonly double Time;

    /// <summary>
    /// The magnitude of the maximum difference between two velocities for them to
    /// be considered within tolerance.
    /// </summary>
    /// <remarks>
    /// The units for the velocity tolerance must be the same as the units used
    /// for the velocities that are to be compared to this tolerance.
    /// </remarks>
    public readonly double Velocity;

    public Tolerance(double distance = EpsilonDefault, double time = EpsilonDefault,
        double velocity = EpsilonDefault)
    {
        Distance = distance;
        Time = time;
        Velocity = velocity;
    }
}