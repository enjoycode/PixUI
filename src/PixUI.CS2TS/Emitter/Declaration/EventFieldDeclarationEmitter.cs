using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    partial class Emitter
    {
        public override void VisitEventFieldDeclaration(EventFieldDeclarationSyntax node)
        {
            //TODO: fix event declaration in Interface
            
            AddUsedModule("System"); //always use System module

            if (node.IsTSRawScript(out var script))
            {
                Write(script!);
                return;
            }

            WriteLeadingTrivia(node);
            WriteModifiers(node.Modifiers);

            var isAbstract = node.HasAbstractModifier();

            var type = node.Declaration.Type;
            if (type is NullableTypeSyntax nullableTypeSyntax)
                type = nullableTypeSyntax.ElementType;

            var typeSymbol = (INamedTypeSymbol) SemanticModel.GetSymbolInfo(type).Symbol!;
            var delegateMethod = typeSymbol.DelegateInvokeMethod!;
            if (delegateMethod.Parameters.Length > 2)
                throw new Exception("暂只支持最多两个事件参数");

            var eventName = node.Declaration.Variables[0].Identifier.Text;
            if (isAbstract)
            {
                Write("get ");
                Write(eventName);
                Write("(): ");
                WriteEventType(delegateMethod);
                Write(';');
            }
            else
            {
                Write("readonly ");
                Write(eventName);
                Write(" = new ");
                WriteEventType(delegateMethod);
                Write("();");
            }

            WriteTrailingTrivia(node);
        }

        private void WriteEventType(IMethodSymbol delegateMethod)
        {
            Write("System.Event");
            if (delegateMethod.Parameters.Length == 0) return;
            
            Write('<');
            for (var i = 0; i < delegateMethod.Parameters.Length; i++)
            {
                if (i != 0) Write(", ");
                WriteTypeSymbol(delegateMethod.Parameters[i].Type, true, true);
            }
            Write('>');
        }
    }
}