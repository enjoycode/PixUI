using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    /// <summary>
    /// 用于拦截处理new ParagraphBuilder/TextStyle/ParagraphStyle
    /// </summary>
    internal sealed class CanvasKitCtorInterceptor : ITSInterceptor
    {
        void ITSInterceptor.Emit(Emitter emitter, SyntaxNode node, ISymbol symbol)
        {
            var creation = (ObjectCreationExpressionSyntax)node;
            var typeName = symbol.ContainingType.Name;

            emitter.Write("PixUI.Make");
            emitter.Write(typeName);
            emitter.Write('(');

            if (typeName is "TextStyle" or "ParagraphStyle")
            {
                if (creation.Initializer != null)
                {
                    emitter.Write('{');
                    
                    var sep = false;
                    foreach (var expression in creation.Initializer.Expressions)
                    {
                        if (sep) emitter.Write(", ");
                        else sep = true;

                        var assignment = (AssignmentExpressionSyntax)expression;

                        //Do not Visit assignment.Left
                        var memberName = assignment.Left.ToString();
                        emitter.TryRenameSymbol(
                            emitter.SemanticModel.GetSymbolInfo(assignment.Left).Symbol!,
                            ref memberName);
                        emitter.Write(memberName);
                        emitter.Write(": ");
                        emitter.Visit(assignment.Right);
                    }

                    emitter.Write('}');
                }
            }
            else
            {
                emitter.Visit(creation.ArgumentList);
            }

            emitter.Write(')');
        }
    }
}