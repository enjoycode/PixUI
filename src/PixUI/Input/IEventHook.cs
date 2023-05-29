using System;

namespace PixUI;

public interface IEventHook
{
    EventPreviewResult PreviewEvent(EventType type, object? e);
}

[Flags]
public enum EventPreviewResult
{
    /// <summary>
    /// The message is not processed.
    /// </summary>
    NotProcessed = 0,

    /// <summary>
    /// The message is processed.
    /// </summary>
    Processed = 1,

    /// <summary>
    /// No dispatch of the message is allowed.
    /// </summary>
    NoDispatch = Processed << 1,

    /// <summary>
    /// No further delegation to other listeners is desired.
    /// </summary>
    NoContinue = NoDispatch << 1,

    /// <summary>
    /// Processed and Dispatched flags
    /// </summary>
    ProcessedNoDispatch = Processed | NoDispatch,

    /// <summary>
    /// All flags are set
    /// </summary>
    All = Processed | NoDispatch | NoContinue,
}