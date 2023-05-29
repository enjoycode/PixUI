using Microsoft.CodeAnalysis;

namespace PixUI.CS2TS
{
    internal sealed class TSIngnoreMethodInvokeInterceptor : ITSInterceptor
    {
        internal static readonly TSIngnoreMethodInvokeInterceptor Default = new();

        private TSIngnoreMethodInvokeInterceptor() { }

        void ITSInterceptor.Emit(Emitter emitter, SyntaxNode node, ISymbol symbol)
        {
            //do nothing
        }
    }
}