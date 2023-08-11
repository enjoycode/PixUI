namespace PixUI.Dynamic;

public interface IDynamicStateProvider
{
    State GetState(string name);
}