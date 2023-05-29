using System;

class TestClass
{
    void TestMethod()
    {
        void InnerFunc()
        {
            Console.WriteLine("Hello");
        }
        
        InnerFunc();
    }
}