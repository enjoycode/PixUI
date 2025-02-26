// using System;
// using System.Collections.Generic;
// using PixUI;
//
// namespace CodeEditor;
//
// public sealed class FoldingManager
// {
//     public FoldingManager(Document document)
//     {
//         _document = document;
//         // _document.DocumentChanged += OnDocumentChanged;
//     }
//
//     private readonly Document _document;
//     private List<FoldingSegment> _foldMarker = new List<FoldingSegment>();
//     private List<FoldingSegment> _foldMarkerByEnd = new List<FoldingSegment>();
//
//     #region ====Events====
//
//     public event Action? FoldingsChanged;
//
//     internal void RaiseFoldingsChanged() => FoldingsChanged?.Invoke();
//
//     #endregion
//
//     // private void OnDocumentChanged(DocumentEventArgs args)
//     // {
//     //_document.UpdateSegmentsOnDocumentChanged(_foldMarker, args);
//     // }
//
//
//     public List<FoldingSegment> GetTopLevelFoldedFoldings()
//     {
//         var foldings = new List<FoldingSegment>();
//         var end = new TextLocation(0, 0);
//         foreach (var fm in _foldMarker)
//         {
//             if (fm.IsFolded && (fm.StartLine > end.Line ||
//                                 fm.StartLine == end.Line && fm.StartColumn >= end.Column))
//             {
//                 foldings.Add(fm);
//                 end = new TextLocation(fm.EndColumn, fm.EndLine);
//             }
//         }
//
//         return foldings;
//     }
//
//     internal List<FoldingSegment> GetFoldingsWithStart(int lineNumber)
//     {
//         return GetFoldingsByStartAfterColumn(lineNumber, -1, false);
//     }
//
//     internal List<FoldingSegment> GetFoldingsContainsLineNumber(int lineNumber)
//     {
//         var foldings = new List<FoldingSegment>();
//         foreach (var fm in _foldMarker)
//         {
//             if (fm.StartLine < lineNumber && lineNumber < fm.EndLine)
//                 foldings.Add(fm);
//         }
//
//         return foldings;
//     }
//
//     internal List<FoldingSegment> GetFoldingsWithEnd(int lineNumber)
//     {
//         return GetFoldingsByEndAfterColumn(lineNumber, 0 /*-1*/, false);
//     }
//
//     internal List<FoldingSegment> GetFoldedFoldingsWithStartAfterColumn(int lineNumber, int column)
//     {
//         return GetFoldingsByStartAfterColumn(lineNumber, column, true);
//     }
//
//     internal List<FoldingSegment> GetFoldedFoldingsWithStart(int lineNumber)
//     {
//         return GetFoldingsByStartAfterColumn(lineNumber, -1, true);
//     }
//
//     internal List<FoldingSegment> GetFoldedFoldingsWithEnd(int lineNumber)
//     {
//         return GetFoldingsByEndAfterColumn(lineNumber, 0 /*-1*/, true);
//     }
//
//     private List<FoldingSegment> GetFoldingsByStartAfterColumn(int lineNumber, int column, bool forceFolded)
//     {
//         var foldings = new List<FoldingSegment>();
//
//         //TODO: check web's BinarySearch
//         var pattern = new FoldingSegment(_document, lineNumber, column, lineNumber, column,
//             FoldType.Unspecified, "", false);
//         var index = _foldMarker.BinarySearch(pattern, StartComparer.Instance);
//         if (index < 0) index = ~index;
//
//         for (; index < _foldMarker.Count; index++)
//         {
//             var fm = _foldMarker[index];
//             if (fm.StartLine < lineNumber || fm.StartLine > lineNumber) break;
//             if (fm.StartColumn <= column) continue;
//             if (!forceFolded || fm.IsFolded)
//                 foldings.Add(fm);
//         }
//
//         return foldings;
//     }
//
//     private List<FoldingSegment> GetFoldingsByEndAfterColumn(int lineNumber, int column, bool forceFolded)
//     {
//         var foldings = new List<FoldingSegment>();
//
//         var pattern = new FoldingSegment(_document, lineNumber, column, lineNumber, column,
//             FoldType.Unspecified, "", false);
//         var index = _foldMarkerByEnd.BinarySearch(pattern, EndComparer.Instance);
//         if (index < 0) index = ~index;
//
//         for (; index < _foldMarkerByEnd.Count; index++)
//         {
//             var fm = _foldMarkerByEnd[index];
//             if (fm.EndLine < lineNumber || fm.EndLine > lineNumber) break;
//             if (fm.EndColumn <= column) continue;
//             if (!forceFolded || fm.IsFolded)
//                 foldings.Add(fm);
//         }
//
//         return foldings;
//     }
//
//     public void UpdateFoldings(List<FoldingSegment>? newFoldings)
//     {
//         // final int oldFoldingCount = foldMarker.length;
//         if (newFoldings != null && newFoldings.Count != 0)
//         {
//             newFoldings.Sort((a, b) => a.CompareTo(b));
//             if (_foldMarker.Count == newFoldings.Count)
//             {
//                 for (var i = 0; i < _foldMarker.Count; ++i)
//                 {
//                     newFoldings[i].IsFolded = _foldMarker[i].IsFolded;
//                 }
//
//                 _foldMarker = newFoldings;
//             }
//             else
//             {
//                 for (int i = 0, j = 0; i < _foldMarker.Count && j < newFoldings.Count;)
//                 {
//                     var n = newFoldings[j].CompareTo(_foldMarker[i]);
//                     if (n > 0)
//                     {
//                         ++i;
//                     }
//                     else
//                     {
//                         if (n == 0)
//                         {
//                             newFoldings[j].IsFolded = _foldMarker[i].IsFolded;
//                         }
//
//                         ++j;
//                     }
//                 }
//             }
//         }
//
//         if (newFoldings != null)
//         {
//             _foldMarker = newFoldings;
//             _foldMarkerByEnd = new List<FoldingSegment>(newFoldings);
//             _foldMarkerByEnd.Sort((a, b) => EndComparer.Instance.Compare(a, b));
//         }
//         else
//         {
//             _foldMarker.Clear();
//             _foldMarkerByEnd.Clear();
//         }
//
//         //TODO:暂激发foldingsChanged
//         FoldingsChanged?.Invoke();
//     }
// }
//
// internal sealed class StartComparer : IComparer<FoldingSegment>
// {
//     internal static readonly StartComparer Instance = new StartComparer();
//
//     public int Compare(FoldingSegment x, FoldingSegment y)
//     {
//         if (x.StartLine < y.StartLine) return -1;
//         return x.StartLine == y.StartLine ? x.StartColumn.CompareTo(y.StartColumn) : 1;
//     }
// }
//
// internal sealed class EndComparer : IComparer<FoldingSegment>
// {
//     internal static readonly EndComparer Instance = new EndComparer();
//
//     public int Compare(FoldingSegment x, FoldingSegment y)
//     {
//         if (x.EndLine < y.EndLine) return -1;
//         return x.EndLine == y.EndLine ? x.EndColumn.CompareTo(y.EndColumn) : 1;
//     }
// }