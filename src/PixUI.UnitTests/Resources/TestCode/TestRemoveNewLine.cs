using System;

class TestCalss
{
    int GetNum()
    {
        return
            1;
    }

    void Lambda()
    {
        Func<int> func = ()
            => 32;
        func();
    }
}