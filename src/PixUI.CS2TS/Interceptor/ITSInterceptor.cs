using Microsoft.CodeAnalysis;

namespace PixUI.CS2TS
{
    public interface ITSInterceptor
    {
        public void Emit(Emitter emitter, SyntaxNode node, ISymbol symbol);
    }
}