namespace PixUI.Dynamic;

public interface IDynamicStateProvider
{
    StateBase GetState(string name);
}