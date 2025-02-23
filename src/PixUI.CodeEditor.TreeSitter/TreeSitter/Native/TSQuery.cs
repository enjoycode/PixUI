#if !__WEB__
using System;
using System.Collections.Generic;
using static CodeEditor.TreeSitterApi;

namespace CodeEditor;

public sealed class TSQuery : IDisposable
{
    private static IntPtr _scratchQueryCursor;
    private IntPtr _handle;

    internal TSQuery(IntPtr handle)
    {
        _handle = handle;
        // TODO: cache capture names
        // Console.WriteLine("=====CaptureNames=====");
        // var captureCount = CaptureCount;
        // for (uint i = 0; i < captureCount; i++)
        // {
        //     Console.WriteLine(CaptureNameForId(i));
        // }

        // var patternCount = PatternCount;
        // unsafe
        // {
        //     uint stepCount = 0;
        //     for (uint i = 0; i < patternCount; i++)
        //     {
        //         var stepPtr = ts_query_predicates_for_pattern(_handle, i, ref stepCount);
        //         Console.WriteLine($"StepCount={stepCount}");
        //     }
        // }
    }

    public uint CaptureCount => ts_query_capture_count(_handle);
    public uint PatternCount => ts_query_pattern_count(_handle);
    public uint StringCount => ts_query_string_count(_handle);

    public string CaptureNameForId(uint id)
    {
        uint length = 0;
        var ptr = ts_query_capture_name_for_id(_handle, id, ref length);
        unsafe
        {
            var sPtr = (sbyte*)ptr.ToPointer();
            return new string(sPtr, 0, (int)length);
        }
    }

    public uint StartByteForPattern(uint patternIndex) => ts_query_start_byte_for_pattern(_handle, patternIndex);

    internal IEnumerable<QueryCapture> Captures(TSNode node,
        TSPoint? startPosition = null, TSPoint? endPosition = null)
    {
        if (_scratchQueryCursor == IntPtr.Zero)
            _scratchQueryCursor = ts_query_cursor_new();
        ts_query_cursor_set_match_limit(_scratchQueryCursor, uint.MaxValue);

        var start = startPosition ?? new TSPoint();
        var end = endPosition ?? new TSPoint();
        ts_query_cursor_set_point_range(_scratchQueryCursor, start, end);
        ts_query_cursor_exec(_scratchQueryCursor, _handle, node);

        var match = new TsQueryMatch();
        uint captureIndex = 0;
        while (ts_query_cursor_next_capture(_scratchQueryCursor, ref match, ref captureIndex))
        {
            for (var i = 0; i < match.capture_count; i++)
            {
                yield return GetQueryCapture(match.captures, i);
            }
        }
    }

    private static unsafe QueryCapture GetQueryCapture(IntPtr capturesPtr, int index)
    {
        var captures = (QueryCapture*)capturesPtr.ToPointer();
        return captures[index];
    }

    public void Dispose()
    {
        if (_handle == IntPtr.Zero) return;

        ts_query_delete(_handle);
        _handle = IntPtr.Zero;
    }
}

#endif