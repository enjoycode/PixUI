using System;
using Microsoft.CodeAnalysis.Text;

namespace PixUI.CS2TS
{
    public sealed class EmitException : Exception
    {
        public readonly TextSpan Span;

        public EmitException(string message, TextSpan span) : base(message)
        {
            Span = span;
        }
    }
}