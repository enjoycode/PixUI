using System;
using System.Threading;
using System.Threading.Tasks;

namespace PixUI;

/// <summary>
/// 用于延迟执行任务，如执行过程中更新则继续延迟
/// </summary>
[TSType("PixUI.DelayTask")]
public sealed class DelayTask
{
    private int _flag;
    private readonly int _millisecondsDelay;
    private readonly Action _action;

    public DelayTask(int millisecondsDelay, Action action)
    {
        _millisecondsDelay = millisecondsDelay;
        _action = action;
    }

    public async void Run()
    {
        if (Interlocked.Increment(ref _flag) != 1)
            return;

        while (true)
        {
            await Task.Delay(_millisecondsDelay);
            if (Interlocked.CompareExchange(ref _flag, 0, 1) == 1)
                break;
            Interlocked.Exchange(ref _flag, 1);
        }

        _action();
    }
}