namespace PixUI;

public abstract class PropagateEvent
{
    public bool IsHandled { get;  set; }

    public void StopPropagate() => IsHandled = true; //TODO: remove it

    public void ResetHandled() => IsHandled = false; //TODO: remove it
}