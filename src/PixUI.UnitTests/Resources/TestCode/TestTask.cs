using System;
using System.Threading.Tasks;

class TestClass
{

    void TestWhenAny()
    {
        var tasks = new Task[] { Task.CompletedTask};
        Task.WhenAny(tasks);
        Task.WhenAny(Task.CompletedTask);
        Task.WhenAny(Task.CompletedTask, Task.CompletedTask);
    }

    void TestRun()
    {
        Action action = () => { };
        Task.Run(action);
        Task.Run(() => Console.WriteLine("hello"));
        Task.Run(() =>
        {
            Console.WriteLine("hello");
            Console.WriteLine("world");
        });
    }

    async void TestDelay()
    {
        await Task.Delay(1000);
    }

}