using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    partial class Emitter
    {
        private readonly Stack<BlockResources> _blockStack = new();
        
        internal void EnterBlock(BlockSyntax block)
        {
            _blockStack.Push(new BlockResources(block));
        }

        private void AddUsingResourceToBlock(VariableDeclarationSyntax resource) 
            => _blockStack.Peek().Add(resource);

        internal void SetBlockInjectPosition() 
            => _blockStack.Peek().SetInjectPosition(GetCurrentOutput(), GetCurrentOutputPosition());

        private void InjectBlockStatement(string statement) 
            => _blockStack.Peek().InjectStatement(statement);

        /// <summary>
        /// 离开Block前，如果有需要自动释放的资源生成相关Dispose代码
        /// </summary>
        internal void LeaveBlock(bool lastIsReturnStatement)
        {
            var block = _blockStack.Pop();
            if (!lastIsReturnStatement)
                AutoDisposeBlockResources(block);
        }

        /// <summary>
        /// 在return前如果有需要自动释放的资源生成相关Dispose代码
        /// </summary>
        internal bool AutoDisposeBeforeReturn(ReturnStatementSyntax returnStatement)
        {
            // 如下示例:
            // using var res = new Resource();
            // if (some)
            //     return;
            var autoBlock = returnStatement.Parent is not BlockSyntax &&
                            _blockStack.Any(b => b.Resources != null);
            if (autoBlock)
            {
                WriteLeadingWhitespaceOnly(returnStatement.Parent!);
                Write("{\n");
            }

            //注意需要循环向上处理所有Block，除非是Lambda表达式的Block
            foreach (var block in _blockStack)
            {
                AutoDisposeBlockResources(block, returnStatement);
                if (block.BlockSyntax.Parent is ParenthesizedLambdaExpressionSyntax)
                    break;
            }

            return autoBlock;
        }

        private void AutoDisposeBlockResources(BlockResources block, ReturnStatementSyntax? returnStatement = null)
        {
            if (block.Resources == null) return;

            //dispose using resources
            foreach (var resource in block.Resources)
            {
                //TODO:暂简单根据名称特殊处理CanvasKit相关资源Dispose重命名或忽略
                //var typeInfo = SemanticModel.GetTypeInfo(resource.Type);
                var typeInfo = SemanticModel.GetTypeInfo(resource.Variables[0].Initializer!.Value);
                var typeName = typeInfo.Type!.Name;
                var rootNamespace = typeInfo.Type!.GetRootNamespace();
                var renameForCanvasKitResource = false;
                if (rootNamespace is { Name: "PixUI" })
                {
                    if (typeName is "ParagraphStyle" or "RRect" or "TextStyle")
                        continue;
                    if (typeName is "Paint" or "Paragraph" or "ParagraphBuilder" or "Path")
                        renameForCanvasKitResource = true;
                }

                foreach (var variable in resource.Variables)
                {
                    if (returnStatement == null)
                    {
                        WriteLeadingWhitespaceOnly(block.BlockSyntax);
                        Write('\t');
                    }
                    else
                    {
                        WriteLeadingWhitespaceOnly(returnStatement);
                    }

                    Write(variable.Identifier.Text);
                    if (typeInfo.Nullability.FlowState == NullableFlowState.MaybeNull)
                        Write('?');
                    Write(renameForCanvasKitResource ? ".delete();\n" : ".Dispose();\n");
                }
            }
        }
    }
    
    /// <summary>
    /// Block内需要自动Dispose的资源
    /// </summary>
    internal sealed class BlockResources
    {
        internal readonly BlockSyntax BlockSyntax;

        private int _injectPosition; //用于注入out var xxx之类的变量定义
        private StringBuilder? _injectOutput;

        //需要自动Dispose的资源列表
        internal List<VariableDeclarationSyntax>? Resources { get; private set; }

        internal BlockResources(BlockSyntax blockSyntax)
        {
            BlockSyntax = blockSyntax;
        }

        internal void Add(VariableDeclarationSyntax resource)
        {
            Resources ??= new List<VariableDeclarationSyntax>();
            Resources.Add(resource);
        }

        internal void SetInjectPosition(StringBuilder output, int position)
        {
            _injectOutput = output;
            _injectPosition = position;
        }

        internal void InjectStatement(string statement)
        {
            _injectOutput!.Insert(_injectPosition, statement + ";\n");
            _injectPosition += statement.Length + 2;
        }
    }
}