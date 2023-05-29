using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PixUI;

public class Ticker
{
    private const int Interval = 16;

    private readonly Action<double> _onTick;

    private DateTime? _startTime;
    private int _animationId = 0;
    private bool _isActive = false;

    public bool IsActive => _isActive;

    protected bool ShouldScheduleTick => _isActive;

    public Ticker(Action<double> onTick)
    {
        _onTick = onTick;
    }

    public void Start()
    {
        //Debug.Assert(_startTime == null);
        _startTime = DateTime.UtcNow;
        _animationId++;
        _isActive = true;
        if (ShouldScheduleTick)
            ScheduleTick();
    }

    public void Stop(bool canceled = false)
    {
        if (!_isActive) return;

        _isActive = false;
        _startTime = null;
    }


#if __WEB__
        [TSRawScript(@"
        private ScheduleTick(rescheduling = false) {
            let id = this._animationId;
            requestAnimationFrame(() => this.Tick(System.DateTime.UtcNow, id));
        }
        ")]
        private void ScheduleTick(bool rescheduling = false) {}
#else
    private async void ScheduleTick(bool rescheduling = false)
    {
        var id = _animationId;
        await Task.Delay(Interval);
        Tick(DateTime.UtcNow, id); //注意仍旧在UI线程回调
    }
#endif

    private void Tick(DateTime timeStamp, int id)
    {
        if (id != _animationId) return;

        _startTime ??= timeStamp;
        _onTick((timeStamp - _startTime.Value).TotalSeconds);

        if (ShouldScheduleTick)
            ScheduleTick(true);
    }
}