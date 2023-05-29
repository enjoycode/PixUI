using System;
using System.Threading.Tasks;

namespace PixUI;

public sealed class FutureBuilder<T> : DynamicView
{
    public FutureBuilder(Task<T> future,
        Func<T?, Exception?, Widget?> doneBuilder,
        Func<Widget>? runningBuilder = null)
    {
        _future = future;
        _doneBuilder = doneBuilder;

        if (runningBuilder != null)
            ReplaceTo(runningBuilder());
    }

    private readonly Task<T> _future;
    private readonly Func<T?, Exception?, Widget?> _doneBuilder;

    protected override void OnMounted()
    {
        base.OnMounted();

        if (!HasLayout)
            Run();
    }

    private async void Run()
    {
        try
        {
            var res = await _future;
            // @ts-ignore
            ReplaceTo(_doneBuilder(res, null));
        }
        catch (Exception ex)
        {
            object? nullValue = null; //Don't use default(T) for web
            ReplaceTo(_doneBuilder((T?)nullValue, ex));
        }
    }

}