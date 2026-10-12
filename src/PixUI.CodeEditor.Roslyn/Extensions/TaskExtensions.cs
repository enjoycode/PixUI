namespace PixUI.CodeEditor.Roslyn;

internal static class TaskExtensions
{
    public static T WaitAndGetResult<T>(this Task<T> task, CancellationToken cancellationToken)
    {
        task.Wait(cancellationToken);
        return task.Result;
    }
}