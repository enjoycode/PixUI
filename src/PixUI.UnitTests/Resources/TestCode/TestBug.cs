// class BugOfIndexer
// {
//     private string a = "aa";
//     public string this[int index]
//     {
//         get => a;
//         set => a = value;
//     }
// }

// class Point
// {
//     public readonly int X;
//     public readonly int Y;
//
//     public string a = null!;
//
//     public string? Name = null;
// }

// class BugOfBinaryExpression
// {
//     void Test(Point? point)
//     {
//         var x = 32 + point?.X ?? 0;
//         var y = 32 + point == null ? 0 : point.Y;
//     }
// }

// class BugOfAssignment
// {
//     void Test(Point pt)
//     {
//         var n = pt.Name!;
//     }
// }

// using PixUI;
//
// interface IDirtyArea
// {
//     IDirtyArea ToChild();
// }
//
// class BugOfDelegateBind
// {
//     void Paint(IDirtyArea area) { }
//     
//     void Test(IDirtyArea? area)
//     {
//         Paint(area?.ToChild());
//     }
// }

// using PixUI;
//
// namespace PixUI
// {
//         /// Delegate for something
//         public delegate string RouteWidgetAsyncBuilder(string? arg);
//         [TSType("PixUI.RouteWidgetAsyncBuilder"), TSRename("RouteWidgetAsyncBuilder")]
//         public delegate string RouteWidgetBuilder(string? arg);
//
//         /// <summary>
//         /// This is Test Class
//         /// Don't use it
//         /// </summary>
//         public sealed class MyClass
//         {
//                 public readonly RouteWidgetBuilder Builder;
//
//                 public MyClass(RouteWidgetBuilder builder)
//                 {
//                         Builder = builder;
//                 }
//         }
// }

using System.Runtime.CompilerServices;

class BugClass
{
    void Hello(int v = 0, [CallerMemberName] string? name = null) {}

    void Test()
    {
        Hello();
    }
}