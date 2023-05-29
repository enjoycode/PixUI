using PixUI;

namespace PixUI
{
    public struct Point
    {
        public int X;
        public int Y;
    }
    
    public sealed class State<T>
    {
        public T Value { get; set; }

        [PixUI.TSCtorNonInitializer]
        public State(T value)
        {
            Value = value;
        }

        // public static implicit operator T(State<T> rx) => rx.Value;
        public static implicit operator State<T>(T value) => new State<T>(value);
    }
}

class TestWidget
{
    private readonly State<string> _name = "Rick";
    private readonly State<int> _age;

    TestWidget(State<Point>? pos)
    {
        _age = 100;
    }
    
    void Test()
    {
        State<int>? state = 32;
        var widget = new TestWidget(new Point());
        int value = state?.Value ?? 0;
        // System.Console.Write(value);
    }
}