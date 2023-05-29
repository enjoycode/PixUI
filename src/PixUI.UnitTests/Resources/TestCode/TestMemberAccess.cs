class BaseClass
{
    public int Time { get; set; }
    public void Stop() {}
    public static void Start() {}
}

class Bug : BaseClass
{
    private int _a = 10;
    private BaseClass? _ticker;
    
    void Test()
    {
        Start();

        Stop();
        this.Stop();
        
        Hello();
        this.Hello();
        
        _ticker?.Time = 32;
        _ticker?.Stop();
        _ticker!.Stop();
        _ticker.Stop();
    }

    void Hello()
    {
       var i= _a == 0 ? 1 + _a : (_a * 3);
       System.Console.Write(i);
    }
    
}