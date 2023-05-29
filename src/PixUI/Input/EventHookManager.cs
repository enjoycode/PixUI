using System;
using System.Collections.Generic;

namespace PixUI;

public sealed class EventHookManager
{
    private readonly List<WeakReference> _hookRefs = new List<WeakReference>();

    public bool HookEvent(EventType type, object? e)
    {
        if (_hookRefs.Count == 0) return false;

        var r = EventPreviewResult.NotProcessed;
        for (var i = 0; i < _hookRefs.Count; i++)
        {
            var hook = _hookRefs[i].Target as IEventHook;

            if (hook == null /*!_hookRefs[i].IsAlive*/)
            {
                _hookRefs.RemoveAt(i);
                i--;
            }
            else
            {
                var single = hook.PreviewEvent(type, e);
                if ((single & EventPreviewResult.Processed) == EventPreviewResult.Processed)
                    r |= EventPreviewResult.Processed;
                if ((single & EventPreviewResult.NoDispatch) == EventPreviewResult.NoDispatch)
                    r |= EventPreviewResult.NoDispatch;
                if ((single & EventPreviewResult.NoContinue) == EventPreviewResult.NoContinue)
                {
                    r |= EventPreviewResult.NoContinue;
                    break;
                }
            }
        }

        return (r & EventPreviewResult.NoDispatch) == EventPreviewResult.NoDispatch;
    }

    public void Add(IEventHook hook)
    {
        _hookRefs.Add(new WeakReference(hook));
    }

    public void Remove(IEventHook hook)
    {
        for (var i = 0; i < _hookRefs.Count; i++)
        {
            var weakRef = _hookRefs[i];
            if ( /*weakRef.IsAlive &&*/ ReferenceEquals(weakRef.Target, hook))
            {
                _hookRefs.RemoveAt(i);
                break;
            }
        }
    }
}