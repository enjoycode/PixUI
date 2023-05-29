using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynUtils;

namespace PixUI.CS2TS
{
    /// <summary>
    /// 将Roslyn Document转译为TypeScript或JavaScript
    /// </summary>
    public sealed partial class Emitter : CSharpSyntaxWalker
    {
        private Emitter(Translator translator, Document document, SemanticModel semanticModel,
            bool toJavaScript = false, AppBoxContext? appBoxContext = null)
            : base(SyntaxWalkerDepth.Trivia)
        {
            Translator = translator;
            Document = document;
            SemanticModel = semanticModel;
            ToJavaScript = toJavaScript;
            AppBoxContext = appBoxContext;
            _typeSymbolCache = new TypeSymbolCache(semanticModel);
        }

        public static async Task<Emitter> MakeAsync(Translator translator, Document document,
            bool toJavascript = false, AppBoxContext? appBoxContext = null)
        {
            var semanticModel = await document.GetSemanticModelAsync();
            return new Emitter(translator, document, semanticModel!, toJavascript, appBoxContext);
        }

        private readonly Document Document;
        internal readonly SemanticModel SemanticModel;
        internal readonly Translator Translator;
        internal readonly bool ToJavaScript; //直接翻译为ES2017

        internal readonly AppBoxContext? AppBoxContext;

        // 使用到的模块，用于生成文件头import
        private readonly HashSet<string> _usedModules = new();

        // 是否忽略委托绑定，事件+= or -=时设为true
        internal bool IgnoreDelegateBind = false;

        // 转为参数时是否将charCode的数值转为js的字符串(因为char会统一转换为number)
        internal bool CharCodeToString = false;

        // // 用于将构造转换为工厂方法时替换this为指定名称
        // private string? ReplaceThisTo = null;

        //公开导出的类型
        private readonly List<BaseTypeDeclarationSyntax> _publicTypes = new();

        //专用于处理IfStatement的IsPatternExpression
        internal IsPatternExpressionSyntax? InjectIsPatternForIfStatement = null;

        public void Emit() => Visit(SemanticModel.SyntaxTree.GetRoot());

        /// <summary>
        /// 添加使用到的包
        /// </summary>
        internal void AddUsedModule(string moduleName)
        {
            if (!_usedModules.Contains(moduleName))
                _usedModules.Add(moduleName);
        }

        internal void AddPublicType(BaseTypeDeclarationSyntax typeDeclarationSyntax)
            => _publicTypes.Add(typeDeclarationSyntax);
    }
}