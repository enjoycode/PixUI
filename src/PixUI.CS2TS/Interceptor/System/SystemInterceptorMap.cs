using System.Collections.Generic;

namespace PixUI.CS2TS
{
    /// <summary>
    /// 用于拦截系统类型或成员转换为TS原生类型
    /// </summary>
    internal static class SystemInterceptorMap
    {
        private static readonly Dictionary<string, ITSInterceptor> Map = new()
        {
            { "System.Console", new ConsoleInterceptor() },
            { "System.Diagnostics.Debug", new DebugInterceptor() },
            { "System.Math", new MathInterceptor() },
            { "System.Array", new ArrayInterceptor() },
            { "System.WeakReference", new WeakReferenceInterceptor() },
            { "System.Threading.Tasks.Task", new TaskInterceptor() },
            { "System.Collections.Generic.EqualityComparer<T>", new EqualityComparerInterceptor() },
            { "string", new StringInterceptor() },
            { "byte", NumberInterceptor.Default },
            { "float", NumberInterceptor.Default },
            { "double", NumberInterceptor.Default },
            { "int", NumberInterceptor.Default },
            { "long", NumberInterceptor.Default },
            //TODO: others
        };

        internal static bool TryGetInterceptor(string name, out ITSInterceptor interceptor)
        {
            return Map.TryGetValue(name, out interceptor);
        }
    }
}