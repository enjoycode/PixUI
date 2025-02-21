namespace CodeEditor;

internal interface ISegmentTree
{
    void Add(TextSegment s);
    void Remove(TextSegment s);
    void UpdateAugmentedData(TextSegment s);
}