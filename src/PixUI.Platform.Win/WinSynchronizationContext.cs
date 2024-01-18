using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixUI.Platform.Win;

internal sealed class WinSynchronizationContext : SynchronizationContext
{
    public override void Post(SendOrPostCallback d, object? state)
    {
        WinApplication.Current.BeginInvoke(() => d(state));
    }
    public override SynchronizationContext CreateCopy()
    {
        return new WinSynchronizationContext();
    }
}