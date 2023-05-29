using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynUtils;

namespace PixUI.CS2TS
{
    partial class Emitter
    {
        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            //特例处理nameof(xxx)
            if (TryEmitNameof(node))
                return;

            var symbolInfo = SemanticModel.GetSymbolInfo(node);
            Debug.Assert(symbolInfo.Symbol is IMethodSymbol);
            var methodSymbol = (IMethodSymbol)symbolInfo.Symbol!;

            //特殊处理Equals(a,b) or ReferenceEquals(a,b)
            if (TryEmitEquals(node, methodSymbol))
                return;

            //尝试转换EnumValue.ToString()
            if (TryEmitEnumToString(node, methodSymbol))
                return;

            //尝试Delegate.Invoke
            if (TryEmitDelegateInvoke(node, methodSymbol))
                return;

            //尝试转换系统方法调用, eg: Console.Write(), Math.Max()
            if (methodSymbol.IsSystemNamespace())
            {
                var systemType = methodSymbol.ContainingType.IsGenericType
                    ? methodSymbol.ContainingType.OriginalDefinition
                    : methodSymbol.ContainingType;
                if (SystemInterceptorMap.TryGetInterceptor(systemType.ToString(), out var systemInterceptor))
                {
                    WriteLeadingTrivia(node);
                    systemInterceptor.Emit(this, node, methodSymbol);
                    return;
                }
            }

            //尝试使用拦截器
            if (TryGetInterceptor(methodSymbol, out var interceptor))
            {
                interceptor!.Emit(this, node, methodSymbol);
                return;
            }

            //尝试特殊处理调用AppBox的服务
            if (TryEmitInvokeAppBoxService(node, methodSymbol))
                return;
            //尝试特殊处理Entity.Observe() or RxEntity.Observe()
            if (TryEmitEntityObserve(node, methodSymbol))
                return;
            //尝试处理扩展方法调用
            if (TryEmitExtensionMethod(node, methodSymbol))
                return;

            IgnoreDelegateBind = true;
            Visit(node.Expression);
            IgnoreDelegateBind = false;

            // Arguments
            VisitToken(node.ArgumentList.OpenParenToken);
            EmitInvocationArgs(node, methodSymbol, false);
            //暂在这里处理排序类方法的比较器 eg: aa.OrderBy((float n) => n)转为 aa.OrderBy(n=>n, System.NumberComparer)
            TryEmitComparerArgForSortMethod(node, methodSymbol);
            VisitToken(node.ArgumentList.CloseParenToken);
        }

        private void EmitInvocationArgs(InvocationExpressionSyntax node, IMethodSymbol methodSymbol, bool firstSep)
        {
            var argsCount = node.ArgumentList.Arguments.Count;
            if (argsCount > 0)
            {
                var argIndex = 0;
                var lastParameterIndex = methodSymbol.Parameters.Length - 1;
                var lastIsParams = methodSymbol.Parameters[lastParameterIndex].IsParams;
                SyntaxToken? sepToken = null;
                foreach (var item in node.ArgumentList.Arguments.GetWithSeparators())
                {
                    if (item.IsToken)
                    {
                        sepToken = item.AsToken(); //VisitToken(item.AsToken());
                        continue;
                    }

                    var argNode = (ArgumentSyntax)item.AsNode()!;
                    var isNullArg = argNode.Expression is LiteralExpressionSyntax literal &&
                                    literal.Kind() == SyntaxKind.NullLiteralExpression;
                    var isArrayArg = false;
                    var isParams = lastIsParams && argIndex >= lastParameterIndex;

                    //特殊处理null赋值给params
                    if (isParams && isNullArg)
                        break;

                    if (firstSep)
                    {
                        Write(", ");
                        firstSep = false;
                    }

                    if (sepToken.HasValue)
                        VisitToken(sepToken.Value);

                    //需要判断是否params参数且当前是数组，是则加...前缀
                    if (isParams && !isNullArg)
                    {
                        isArrayArg = SemanticModel.GetTypeInfo(argNode.Expression).Type?.Kind == SymbolKind.ArrayType;
                        if (isArrayArg)
                            Write("...");
                    }

                    Visit(argNode);

                    //特殊处理params的JsNativeArray，需要重新转换回数组
                    if (isParams && !isNullArg && isArrayArg)
                    {
                        var jsArrayType = GetJsNativeArrayType(methodSymbol.Parameters[lastParameterIndex].Type);
                        if (jsArrayType != null)
                            Write(".ToArray()");
                    }

                    argIndex++;
                }
            }

            //如果参数数量不一致，特殊处理eg: OnPropertyChanged([CallerMemberName] name = null)之类
            if (argsCount != methodSymbol.Parameters.Length)
            {
                //判断有无[CallerMemberName]的参数
                var hasCallerMemberName = methodSymbol.Parameters.Skip(argsCount).Any(HasCallerMemberNameAttribute);
                if (hasCallerMemberName)
                {
                    if (argsCount > 0) Write(", ");
                    for (var i = argsCount; i < methodSymbol.Parameters.Length; i++)
                    {
                        if (i > argsCount) Write(", ");
                        var para = methodSymbol.Parameters[i];
                        if (HasCallerMemberNameAttribute(para))
                        {
                            Write('"');
                            Write(FindCallerMemberName(node));
                            Write('"');
                            break; //后续的暂可以忽略了
                        }

                        Write("undefined"); //可以简单用undefined代替默认值
                    }
                }
            }
        }

        private void TryEmitComparerArgForSortMethod(InvocationExpressionSyntax node, IMethodSymbol symbol)
        {
            //暂只处理Linq.OrderBy
            if (symbol.Name != "OrderBy" && symbol.Name != "OrderByDescending") return;
            if (!IsIEnumerableType(symbol.ReceiverType)) return;
            if (symbol.Parameters.Length != 1) return; //已经指定IComparer，由VisitArgument时转换处理

            //目前只处理数字类型，TODO: 待判断是否实现IComparable
            var argType = SemanticModel.GetTypeInfo(node.ArgumentList.Arguments[0].Expression).ConvertedType;
            if (argType is INamedTypeSymbol namedType && namedType.TypeArguments[1].IsNumber())
            {
                AddUsedModule("System");
                Write(", System.NumberComparer");
            }
        }

        private static string FindCallerMemberName(InvocationExpressionSyntax node)
        {
            var callerMember = node.Ancestors().First(n => n is PropertyDeclarationSyntax or MethodDeclarationSyntax);
            return callerMember is PropertyDeclarationSyntax property
                ? property.Identifier.Text
                : ((MethodDeclarationSyntax)callerMember).Identifier.Text;
        }

        private bool TryEmitEnumToString(InvocationExpressionSyntax node, IMethodSymbol symbol)
        {
            if (symbol.ContainingType.SpecialType == SpecialType.System_Enum && symbol.Name == "ToString")
            {
                var memberAccess = (MemberAccessExpressionSyntax)node.Expression;
                var enumTypeSymbol = SemanticModel.GetTypeInfo(memberAccess.Expression).Type;

                WriteTypeSymbol(enumTypeSymbol!, true);
                Write('[');
                Visit(memberAccess.Expression);
                Write(']');
                return true;
            }

            return false;
        }

        private bool TryEmitNameof(InvocationExpressionSyntax node)
        {
            if (node.Expression is not IdentifierNameSyntax { Identifier: { Text: "nameof" } })
                return false;

            var exp = node.ArgumentList.Arguments[0].Expression;
            var name = exp switch
            {
                IdentifierNameSyntax identifierName => identifierName.Identifier.Text,
                MemberAccessExpressionSyntax memberAccess => memberAccess.Name.Identifier.Text,
                GenericNameSyntax genericName => genericName.Identifier.Text,
                _ => throw new NotImplementedException()
            };

            WriteLeadingTrivia(node);
            Write('"');
            Write(name);
            Write('"');
            WriteTrailingTrivia(node);

            return true;
        }

        private bool TryEmitEquals(InvocationExpressionSyntax node, IMethodSymbol methodSymbol)
        {
            if (node.Expression is IdentifierNameSyntax
                {
                    Identifier: { Text: "ReferenceEquals" or "Equals" }
                } identifier
                &&
                methodSymbol.ContainingType.SpecialType == SpecialType.System_Object)
            {
                WriteLeadingTrivia(node);
                var isEquals = identifier.Identifier.Text == "Equals";
                if (isEquals)
                {
                    AddUsedModule("System");
                    Write("System.Equals");
                }

                Write('(');
                Visit(node.ArgumentList.Arguments[0]);
                Write(isEquals ? ", " : " === ");
                Visit(node.ArgumentList.Arguments[1]);
                Write(')');
                return true;
            }

            return false;
        }

        private bool TryEmitDelegateInvoke(InvocationExpressionSyntax node, IMethodSymbol symbol)
        {
            //先判断是否事件
            var isEvent = false;
            var isConditionalAceess = false;
            if (node.Parent is ConditionalAccessExpressionSyntax accessExpressionSyntax)
            {
                isEvent = SemanticModel.GetSymbolInfo(accessExpressionSyntax.Expression)
                    .Symbol is IEventSymbol;
                isConditionalAceess = true;
            }
            else
            {
                isEvent = SemanticModel.GetSymbolInfo(node.Expression).Symbol is IEventSymbol;
            }

            if (isEvent || (node.Expression.GetLastToken().Text == "Invoke" &&
                            symbol.ContainingType.IsInherits(TypeOfDelegate)))
            {
                if (!isEvent)
                {
                    Visit(node.Expression);
                    //替换名称 Invoke => call
                    RemoveLast(symbol.Name.Length);
                    Write("call");
                }
                else
                {
                    if (isConditionalAceess) RemoveLast(1); //remove '?'
                    else Visit(node.Expression);
                    Write(".Invoke");
                }

                VisitToken(node.ArgumentList.OpenParenToken);
                if (!isEvent)
                    Write(node.ArgumentList.Arguments.Count > 0 ? "this, " : "this");
                VisitSeparatedList(node.ArgumentList.Arguments);
                VisitToken(node.ArgumentList.CloseParenToken);

                return true;
            }

            return false;
        }

        private bool TryEmitInvokeAppBoxService(InvocationExpressionSyntax node, IMethodSymbol symbol)
        {
            if (!symbol.IsAppBoxServiceMethod()) return false;

            //需要检查返回类型内是否包含实体，是则加入引用模型列表内
            if (AppBoxContext != null && !symbol.ReturnsVoid)
                symbol.ReturnType.CheckTypeHasAppBoxModel(AppBoxContext.FindModel, AppBoxContext.AddUsedModel);

            //开始转换为前端服务调用
            AddUsedModule("AppBoxClient");
            Write("AppBoxClient.Channel.Invoke('");
            //参数1: 服务名称 eg: sys.HelloService.SayHello
            Write(symbol.ContainingNamespace.ContainingNamespace.Name);
            Write('.');
            Write(symbol.ContainingType.Name);
            Write('.');
            Write(symbol.Name);
            Write("'");
            //参数2: 服务方法参数列表
            if (node.ArgumentList.Arguments.Count > 0)
            {
                Write(", [");
                VisitSeparatedList(node.ArgumentList.Arguments);
                Write(']');
            }
            else
            {
                Write(", null");
            }

            //参数3: 传入根据视图模型引用的实体模型所生成的EntityFactory
            Write(", EntityFactories");

            Write(')');

            return true;
        }

        private bool TryEmitEntityObserve(InvocationExpressionSyntax node, IMethodSymbol symbol)
        {
            var typeFullName = symbol.ContainingType.ToString();
            if (typeFullName != "AppBoxClient.EntityExtensions" && !typeFullName.StartsWith("AppBoxClient.RxEntity<"))
                return false;

            //方法参数暂只支持指向实体成员的表达式, eg: e => e.Name
            var arg = node.ArgumentList.Arguments[0];
            if (arg.Expression is not LambdaExpressionSyntax argLambda)
                throw new Exception("Only support LambdaExpression now.");
            if (argLambda.ExpressionBody is not MemberAccessExpressionSyntax memberAccess)
                throw new Exception("Only support MemberAccess now.");

            var propSymbol = (IPropertySymbol)SemanticModel.GetSymbolInfo(memberAccess).Symbol!;
            if (!propSymbol.ContainingType.IsAppBoxEntity(AppBoxContext!.FindModel))
                throw new Exception("Must be a Entity");

            var entityFullName = propSymbol.ContainingType.ToString();
            var memberName = memberAccess.Name.Identifier.Text;
            var memberId = AppBoxContext!.FindEntityMemberId(entityFullName, memberName);

            Visit(node.Expression);
            Write('(');
            Write(memberId.ToString());
            Write(',');
            Write($"e=>e.{memberName},");
            Write($"(e,v)=>e.{memberName}=v");
            Write(')');
            WriteTrailingTrivia(node);

            return true;
        }

        private bool TryEmitExtensionMethod(InvocationExpressionSyntax node, IMethodSymbol symbol)
        {
            if (!symbol.IsExtensionMethod || symbol.IsSystemNamespace() /*暂排除系统库*/)
                return false;
            if (node.Expression is not MemberAccessExpressionSyntax memberAccess)
                return false;

            WriteTypeSymbol(symbol.ContainingType, true);
            Write('.');
            Visit(memberAccess.Name);

            VisitToken(node.ArgumentList.OpenParenToken);
            Visit(memberAccess.Expression);
            EmitInvocationArgs(node, symbol, true);
            VisitToken(node.ArgumentList.CloseParenToken);

            return true;
        }
    }
}