using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;

namespace PixUI.CS2TS
{
    partial class Emitter
    {
        public override void VisitObjectCreationExpression(ObjectCreationExpressionSyntax node)
        {
            //预先处理 new object()
            if (node.Type is PredefinedTypeSyntax predefinedType &&
                predefinedType.Keyword.Kind() == SyntaxKind.ObjectKeyword)
            {
                WriteLeadingTrivia(node);
                Write("{}");
                WriteTrailingTrivia(node);
                return;
            }
            
            var symbol = SemanticModel.GetSymbolInfo(node).Symbol;

            //尝试系统拦截
            if (symbol != null && symbol.IsSystemNamespace() &&
                SystemInterceptorMap.TryGetInterceptor(symbol.ContainingType.ToString(),
                    out var systemInterceptor))
            {
                WriteLeadingTrivia(node);
                systemInterceptor.Emit(this, node, symbol);
                return;
            }

            //尝试拦截器
            if (TryGetInterceptor(symbol, out var interceptor))
            {
                interceptor!.Emit(this, node, symbol!);
                return;
            }

            //特殊处理new RxEntity()
            if (TryEmitNewRxEntity(node, symbol!.ContainingType))
                return;
            //特殊处理new PixUI.Route()
            if (TryEmitNewRoute(node, symbol!.ContainingType))
                return;

            //Type, maybe: new SomeType \n{ Prop = 1}, so disable trailing trivia
            VisitToken(node.NewKeyword);
            if (node.ArgumentList == null)
                DisableVisitTrailingTrivia();
            Visit(node.Type);
            if (node.ArgumentList == null)
                EnableVisitTrailingTrivia();

            // arguments
            Write('(');
            if (node.ArgumentList != null)
                VisitSeparatedList(node.ArgumentList.Arguments);
            Write(')');

            // initializer
            Visit(node.Initializer);

            // TrailingTrivia
            if (node.Initializer == null)
                WriteTrailingTrivia(node);
        }

        /// <summary>
        /// new RxEntity`sys.Entities.Employee`();
        /// 转换为:
        /// new AppBoxClient.RxEntity(new sys_Employee());
        /// </summary>
        private bool TryEmitNewRxEntity(BaseObjectCreationExpressionSyntax node, INamedTypeSymbol typeSymbol)
        {
            if (!(typeSymbol.Name == "RxEntity" && typeSymbol.ContainingNamespace.Name == "AppBoxClient"))
                return false;

            var entityType = typeSymbol.TypeArguments[0];
            AppBoxContext!.AddUsedModel(entityType.ToString());
            AddUsedModule("AppBoxClient");

            VisitToken(node.NewKeyword);
            Write(" AppBoxClient.RxEntity(");
            Write("new ");
            Write(entityType.ContainingNamespace.ContainingNamespace.Name);
            Write('_');
            Write(entityType.Name);
            Write("())");

            return true;
        }

        private bool TryEmitNewRoute(BaseObjectCreationExpressionSyntax node, INamedTypeSymbol typeSymbol)
        {
            if (!(typeSymbol.Name == "Route" && typeSymbol.ContainingNamespace.Name == "PixUI"))
                return false;

            if (node.ArgumentList!.Arguments[1].Expression is not LambdaExpressionSyntax lambdaArg)
                throw new Exception("new Route()的第二个参数仅支持Lambda表达式");

            VisitToken(node.NewKeyword);
            Write(" PixUI.Route(");

            for (var i = 0; i < node.ArgumentList!.Arguments.Count; i++)
            {
                if (i != 0) Write(',');
                if (i != 1)
                {
                    Visit(node.ArgumentList.Arguments[i]);
                }
                else
                {
                    Write("async ");

                    if (lambdaArg is SimpleLambdaExpressionSyntax simpleLambda)
                        Visit(simpleLambda.Parameter);
                    else
                        Visit(((ParenthesizedLambdaExpressionSyntax)lambdaArg).ParameterList.Parameters[0]);

                    Write(" => {");

                    //拦截使用到的模型，准备异步import
                    var usedModels = new HashSet<string>();
                    AppBoxContext!.AddUsedModelInterceptor = model => usedModels.Add(model);

                    UseTempOutput();

                    if (lambdaArg.ExpressionBody != null)
                    {
                        Write("return ");
                        Visit(lambdaArg.ExpressionBody);
                        Write(';');
                    }
                    else
                    {
                        Visit(lambdaArg.Body);
                    }

                    var temp = GetTempOutput();
                    EmitImportAsyncModels(usedModels);
                    Write(temp);
                    Write('}');
                }
            }

            Write(')');
            return true;
        }

        private void EmitImportAsyncModels(HashSet<string> usedModels)
        {
            foreach (var model in usedModels)
            {
                var firstDot = model.IndexOf('.');
                var lastDot = model.LastIndexOf('.');
                var appName = model[..firstDot];
                // TODO: maybe Entity?
                // var typeName = model.Substring(firstDot + 1, lastDot);
                // typeName = typeName == "Entities" ? "Entity" : typeName.Substring(0, typeName.Length - 1);
                var modelName = model[(lastDot + 1)..];
                if (AppBoxContext is { ForPreview: true })
                {
                    var modelId = AppBoxContext.FindModelId($"{appName}.Views.{modelName}");
                    Write(
                        $"let {appName}_{modelName} = (await import('/preview/view/{AppBoxContext.SessionId}/{modelId}'))['{modelName}'];");
                }
                else
                {
                    Write(
                        $"let {appName}_{modelName} = (await import('/model/view/{appName}.{modelName}'))['{modelName}'];");
                }
            }
        }
    }
}