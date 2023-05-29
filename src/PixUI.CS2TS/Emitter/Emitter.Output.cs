using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynUtils;

namespace PixUI.CS2TS
{
    public partial class Emitter
    {
        private readonly Stack<StringBuilder> _outputs = new(new[] { new StringBuilder(1024) });

        internal void UseTempOutput() => _outputs.Push(new StringBuilder(100));

        internal string GetTempOutput() => _outputs.Pop().ToString();

        /// <summary>
        /// 获取生成的TypeScript代码
        /// </summary>
        public string GetTypeScriptCode()
        {
            var sb = _outputs.Pop()!;

            //注意使用到的模型由调用者处理，这里不处理

            //输出import使用到的模块
            foreach (var module in _usedModules)
            {
                if (AppBoxContext == null)
                {
                    //used normal
                    sb.Insert(0, $"import * as {module} from '@/{module}'\n");
                }
                else
                {
                    //used by AppBox translate Model's js code
#if DEBUG
                    if (AppBoxContext.ForPreview && AppBoxContext.ForViteDev)
                        sb.Insert(0, $"import * as {module} from '/src/{module}/index.ts'\n");
                    else
                        sb.Insert(0, $"import * as {module} from '/{module}.js'\n");
#else
                    sb.Insert(0, $"import * as {module} from '/{module}.js'\n");
#endif
                }
            }

            return sb.ToString();
        }

        internal void Write(char ch) => _outputs.Peek().Append(ch);

        internal void Write(char ch, int repeatCount) => _outputs.Peek().Append(ch, repeatCount);

        internal void Write(string str) => _outputs.Peek().Append(str);

        private StringBuilder GetCurrentOutput() => _outputs.Peek();

        private int GetCurrentOutputPosition() => _outputs.Peek().Length;

        private void RemoveNewLineBefore(int position)
        {
            var output = _outputs.Peek()!;
            position--;
            while (true)
            {
                if (output[position] == '\n')
                {
                    output.Remove(position, 1);
                    position--;
                }
                else if (char.IsWhiteSpace(output[position]))
                {
                    position--;
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 从开始处移除换行符，用于ReturnStatement及SimpleLambda移除之间的换行符
        /// </summary>
        private void RemoveNewLineAfter(int position)
        {
            var output = _outputs.Peek()!;
            while (true)
            {
                if (output[position] == '\n')
                {
                    output.Remove(position, 1);
                }
                else if (char.IsWhiteSpace(output[position]))
                {
                    position++;
                }
                else
                {
                    break;
                }
            }
        }

        internal void RemoveLast(int length)
        {
            var sb = _outputs.Peek();
            sb.Remove(sb.Length - length, length);
        }

        internal void WriteLeadingTrivia(SyntaxNode node)
        {
            if (!node.HasLeadingTrivia || _disableVisitLeadingTrivia) return;

            foreach (var trivia in node.GetLeadingTrivia())
            {
                VisitTrivia(trivia);
            }
        }

        internal void WriteLeadingWhitespaceOnly(SyntaxNode node)
        {
            if (!node.HasLeadingTrivia || _disableVisitLeadingTrivia) return;

            var whitespace = node.GetLeadingTrivia()
                .Last(t => t.Kind() == SyntaxKind.WhitespaceTrivia);
            Write(' ', whitespace.Span.Length);
        }

        internal void WriteTrailingTrivia(SyntaxNode node)
        {
            if (!node.HasTrailingTrivia || _disableVisitTrailingTrivia) return;

            foreach (var trivia in node.GetTrailingTrivia())
            {
                VisitTrivia(trivia);
            }
        }

        internal void WriteModifiers(SyntaxTokenList modifiers)
        {
            if (!modifiers.Any()) return;

            if (!ToJavaScript)
            {
                //排除override的
                if (modifiers.All(m => m.Kind() != SyntaxKind.OverrideKeyword))
                {
                    if (modifiers.Any(m =>
                            m.Kind() == SyntaxKind.PublicKeyword ||
                            m.Kind() == SyntaxKind.InternalKeyword))
                        Write("public ");
                    else if (modifiers.Any(m => m.Kind() == SyntaxKind.ProtectedKeyword))
                        Write("protected ");
                    else
                        Write("private ");
                }
            }

            if (modifiers.Any(m => m.Kind() == SyntaxKind.ConstKeyword))
            {
                Write(ToJavaScript ? "static " : "static readonly ");
            }
            else
            {
                if (modifiers.Any(m => m.Kind() == SyntaxKind.StaticKeyword))
                    Write("static ");
                if (!ToJavaScript && modifiers.Any(m => m.Kind() == SyntaxKind.ReadOnlyKeyword))
                    Write("readonly ");
            }

            if (!ToJavaScript && modifiers.Any(m => m.Kind() == SyntaxKind.AbstractKeyword))
                Write("abstract ");

            if (modifiers.Any(m => m.Kind() == SyntaxKind.AsyncKeyword))
                Write("async ");
        }

        /// <summary>
        /// 输出类、结构体、接口定义的继承及实现接口列表
        /// </summary>
        internal void WriteBaseList(BaseListSyntax? baseList, TypeDeclarationSyntax typeDeclaration)
        {
            if (baseList == null) return;

            //extends
            BaseTypeSyntax? baseClass = null;
            if (typeDeclaration is ClassDeclarationSyntax classDeclaration)
            {
                baseClass = classDeclaration.GetBaseType(SemanticModel);
                if (baseClass != null)
                {
                    Write(" extends ");
                    Visit(baseClass.Type);
                }
            }

            //implements
            var interfaces = baseList.Types
                .Where(t => !ReferenceEquals(t, baseClass));

            var sep = false;
            var implKeyword = typeDeclaration is InterfaceDeclarationSyntax
                ? " extends"
                : " implements";
            foreach (var impl in interfaces)
            {
                if (sep)
                {
                    Write(", ");
                }
                else
                {
                    sep = true;
                    if (baseClass != null) Write(' ');
                    Write(implKeyword);
                    Write(' ');
                }

                Visit(impl.Type);
            }
        }

        /// <summary>
        /// 尝试写入范型参数及约束
        /// </summary>
        internal void WriteTypeParameters(TypeParameterListSyntax? types,
            SyntaxList<TypeParameterConstraintClauseSyntax> constraintClauses)
        {
            if (types == null) return;

            Write('<');
            var sep = false;
            foreach (var typesParameter in types.Parameters)
            {
                if (sep) Write(", ");
                else sep = true;

                Visit(typesParameter);

                var constraints = constraintClauses
                    .Where(c => c.Name.Identifier.Value == typesParameter.Identifier.Value)
                    .SelectMany(c => c.Constraints);
                var and = false;
                foreach (var item in constraints)
                {
                    if (item is ConstructorConstraintSyntax)
                        throw new EmitException($"不支持范型约束: new() at File: {item.SyntaxTree.FilePath}", item.Span);

                    if (and)
                    {
                        Write(" & ");
                    }
                    else
                    {
                        and = true;
                        Write(" extends ");
                    }

                    //TODO:暂只支持TypeConstraintSyntax, 其他待实现
                    //https://www.typescriptlang.org/docs/handbook/2/generics.html

                    DisableVisitTrailingTrivia(); //移除尾部Trivia
                    //特殊处理 where T: class
                    if (item is ClassOrStructConstraintSyntax classOrStructConstraintSyntax)
                    {
                        if (classOrStructConstraintSyntax.ClassOrStructKeyword.ValueText == "class")
                            Write("object");
                        else
                            throw new NotImplementedException(
                                $"范型约束为: {classOrStructConstraintSyntax.ClassOrStructKeyword.ValueText}未实现");
                    }
                    else
                    {
                        Visit(item);
                    }

                    EnableVisitTrailingTrivia();
                }
            }

            Write('>');
        }

        /// <summary>
        /// 尝试写入非空值类型所对应的默认值
        /// </summary>
        internal void TryWriteDefaultValueForValueType(TypeSyntax typeSyntax, SyntaxNode from, bool withAsign = true)
        {
            if (typeSyntax is NullableTypeSyntax) return;

            if (typeSyntax is PredefinedTypeSyntax predefined)
            {
                if (withAsign)
                    Write(" = ");
                WritePredefinedTypeDefaultValue(predefined);
                return;
            }

            var type = SemanticModel.GetTypeInfo(typeSyntax).Type!;
            var typeKind = type.TypeKind;
            if (typeKind == TypeKind.Enum)
            {
                if (withAsign)
                    Write(" = ");
                Write("0"); //TODO: convert to enum first value
            }
            else if (typeKind == TypeKind.Struct)
            {
                //因为结构体构造带参数，所以使用默认的XXX.Empty作为初始化值
                if (withAsign)
                    Write(" = ");
                WriteTypeSymbol(type, !from.InSameSourceFile(type));
                Write(".Empty.Clone()");
            }
        }

        /// <summary>
        /// 写入类型的包名称，如果是AppBox的模型则写入应用名称作为前缀
        /// </summary>
        private void TryWritePackageName(NameSyntax node, ISymbol symbol)
        {
            //先判断是否模型类，是则特殊处理
            if (AppBoxContext != null &&
                (symbol.IsAppBoxEntity(AppBoxContext.FindModel) || symbol.IsAppBoxView(AppBoxContext.FindModel)))
            {
                //写入应用名称作为前缀防止同名, eg: sys_EntityName
                var appName = symbol.ContainingNamespace.ContainingNamespace.Name;
                Write(appName);
                Write('_');

                AppBoxContext.AddUsedModel(symbol.ToString());
                return;
            }

            if (node.InSameSourceFile(symbol)) return;

            var root = symbol.GetRootNamespace();
            if (root == null) return;

            AddUsedModule(root.Name);
            Write(root.Name);
            Write('.');
        }

        /// <summary>
        /// 尝试写嵌套类的上级类的名称, eg: new InnerClass() 转换为 new OuterClass.InnerClass()
        /// </summary>
        private void TryWriteParentTypeOfInnerClass(NameSyntax node, ISymbol symbol)
        {
            if (symbol.ContainingType is { TypeKind: TypeKind.Class } parent)
            {
                if (node.Parent is not QualifiedNameSyntax /*eg: new InnerClass()*/
                    ||
                    node.Parent is QualifiedNameSyntax qualified && qualified.Left == node
                   )
                {
                    WriteTypeSymbol(parent, false);
                    Write('.');
                }
            }
        }

        internal void WriteTypeSymbol(ITypeSymbol typeSymbol, bool withNamespace, bool withGeneric = false)
        {
            if (typeSymbol is ITypeParameterSymbol typeParameter)
            {
                Write(typeParameter.Name);
                return;
            }

            //System type first, eg: string
            if (TryWritePredefinedTypeSymbol(typeSymbol))
                return;

            if (withNamespace && typeSymbol.Name != "Nullable")
            {
                var rootNamespace = typeSymbol.GetRootNamespace();
                if (rootNamespace != null)
                {
                    var moduleName = rootNamespace.Name;
                    AddUsedModule(moduleName);
                    Write(moduleName);
                    Write('.');
                }
            }

            var name = typeSymbol.Name;
            TryRenameSymbol(typeSymbol, ref name);
            Write(name);
            
            if (!ToJavaScript && withGeneric && typeSymbol is INamedTypeSymbol { IsGenericType: true } namedType)
            {
                Write('<');
                var first = true;
                foreach (var typeArgument in namedType.TypeArguments)
                {
                    if (first) first = false;
                    else Write(',');
                    
                    WriteTypeSymbol(typeArgument, true, true);
                }
                Write('>');
            }
        }

        private bool TryWritePredefinedTypeSymbol(ITypeSymbol typeSymbol)
        {
            var typeName = typeSymbol.SpecialType switch
            {
                SpecialType.System_Boolean => "boolean",
                SpecialType.System_Byte => "number",
                SpecialType.System_Char => "number",
                SpecialType.System_Single => "number",
                SpecialType.System_Double => "number",
                SpecialType.System_Int16 => "number",
                SpecialType.System_Int32 => "number",
                SpecialType.System_Int64 => "bigint",
                SpecialType.System_UInt16 => "number",
                SpecialType.System_UInt32 => "number",
                SpecialType.System_UInt64 => "bigint",
                SpecialType.System_String => "string",
                SpecialType.System_Object => "any",
                _ => null
            };
            if (typeName == null) return false;

            if (typeSymbol.NullableAnnotation == NullableAnnotation.Annotated && typeName != "any")
                typeName = $"Nullable<{typeName}>";
            Write(typeName);
            return true;
        }

        internal void TryWriteThisOrStaticMemberType(NameSyntax node, ISymbol symbol)
        {
            //排除一些特例
            if (node.Parent is MemberBindingExpressionSyntax)
                return;

            if (node.Parent is MemberAccessExpressionSyntax memberAccess)
            {
                if (ReferenceEquals(node, memberAccess.ChildNodes().First()))
                {
                    WriteThisOrStaticMemberType(node, symbol);
                }
            }
            else
            {
                //TODO:考虑判断是否成员
                WriteThisOrStaticMemberType(node, symbol);
            }
        }

        private void WriteThisOrStaticMemberType(SyntaxNode node, ISymbol symbol)
        {
            if (symbol.IsStatic)
            {
                var typeSymbol = symbol.ContainingType;
                //注意已忽略范型类型参数 eg: GenericType<T>.SomeMethod()
                if (!node.InSameSourceFile(symbol))
                {
                    var moduleName = typeSymbol.GetRootNamespace()!.Name;
                    AddUsedModule(moduleName);
                    Write(moduleName);
                    Write('.');
                }

                Write(typeSymbol.Name);
                Write('.');
            }
            else
            {
                Write("this.");
            }
        }

        private void WritePredefinedTypeDefaultValue(PredefinedTypeSyntax node)
        {
            switch (node.Keyword.Kind())
            {
                case SyntaxKind.StringKeyword:
                    Write("\"\"");
                    break;
                case SyntaxKind.ByteKeyword:
                case SyntaxKind.SByteKeyword:
                case SyntaxKind.ShortKeyword:
                case SyntaxKind.UShortKeyword:
                case SyntaxKind.IntKeyword:
                case SyntaxKind.UIntKeyword:
                case SyntaxKind.FloatKeyword:
                case SyntaxKind.DoubleKeyword:
                    Write("0");
                    break;
                case SyntaxKind.LongKeyword:
                    Write("0n");
                    break;
                case SyntaxKind.BoolKeyword:
                    Write("false");
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 类或结构是否实现了标为[TSInterfaceOf]的接口，是则生成标记成员
        /// </summary>
        private void TryWriteInterfaceOfMeta(TypeDeclarationSyntax node)
        {
            if (node.BaseList == null) return;


            foreach (var baseType in node.BaseList.Types)
            {
                var typeInfo = SemanticModel.GetTypeInfo(baseType.Type);
                var type = typeInfo.Type!;
                if (type is not { TypeKind: TypeKind.Interface })
                    continue;
                //注意目前实现所有系统接口同样写入类型信息
                if (!(type.IsSystemNamespace() || type.IsTSInterfaceOfAttribute(TypeOfTSInterfaceOfAttribute)))
                    continue;

                Write("private static readonly $meta_");
                var rootNamespace = type.GetRootNamespace();
                if (rootNamespace != null)
                {
                    AddUsedModule(rootNamespace.Name);
                    Write(rootNamespace.Name);
                    Write('_');
                }

                Write(type.Name);
                Write(" = true;\n");
            }
        }

        // private void WriteType(TypeDeclarationSyntax node)
        // {
        //     Write(node.Identifier.Text);
        //     if (node.TypeParameterList is not { Parameters: { Count: > 0 } }) return;
        //
        //     Write('<');
        //     for (var i = 0; i < node.TypeParameterList.Parameters.Count; i++)
        //     {
        //         if (i != 0) Write(", ");
        //         Write(node.TypeParameterList.Parameters[i].Identifier.Text);
        //     }
        //
        //     Write('>');
        // }

        /// <summary>
        /// 转换 eg: obj is string
        /// </summary>
        /// <param name="name">maybe ExpressionSyntax or string</param>
        /// <param name="type">注意不支持范型类型</param>
        internal void WriteIsExpression(object name, ExpressionSyntax type)
        {
            //TODO: short path for predefined type
            //obj is null 或 obj is not null 由IsPatternExpressionEmitter预先处理掉
            var typeInfo = SemanticModel.GetTypeInfo(type);
            if (typeInfo.Type == null) throw new Exception();
            if (typeInfo.Type is ITypeParameterSymbol)
                throw new Exception($"不支持is 范型类型,eg: obj is T at File:{type.SyntaxTree.FilePath}");
            if (typeInfo.Type is INamedTypeSymbol namedType && namedType.IsGenericType)
            {
                //只支持非指定类型的范型参数, eg: obj is GenericType<T>可以但obj is GenericType<string>不支持
                if (namedType.TypeArguments.Any(t => t is not ITypeParameterSymbol))
                    throw new Exception($"不支持 obj is GenericType<XXX> at File:{type.SyntaxTree.FilePath}");
            }

            void VisitName()
            {
                if (name is ExpressionSyntax nameExpression)
                {
                    Visit(nameExpression);
                }
                else
                {
                    Write(name.ToString());
                    Write(' ');
                }
            }

            switch (typeInfo.Type.TypeKind)
            {
                case TypeKind.Class:
                case TypeKind.Struct:
                {
                    VisitName();

                    Write("instanceof ");
                    Visit(type);

                    break;
                }
                case TypeKind.Interface:
                    //TODO:检查指定Interface是否支持，不支持报错并提示需要[TSInterfaceOfAttribute]
                    var rootNamespace = typeInfo.Type.GetRootNamespace();
                    if (rootNamespace != null)
                    {
                        var moduleName = typeInfo.Type.GetRootNamespace()!.Name;
                        AddUsedModule(moduleName);
                        Write(moduleName);
                        Write('.');
                    }

                    Write("IsInterfaceOf");
                    var intefaceTypeName = typeInfo.Type.Name;
                    TryRenameSymbol(typeInfo.Type, ref intefaceTypeName);
                    Write(intefaceTypeName); //Write(typeInfo.Type.Name);
                    
                    Write('(');
                    VisitName();
                    Write(')');
                    break;
            }
        }

        /// <summary>
        /// 赋值或传参时尝试隐式转换或结构体Clone
        /// </summary>
        internal void TryImplictConversionOrStructClone(TypeInfo typeInfo, ExpressionSyntax expression)
        {
            if (typeInfo.IsImplictConversionToState(TypeOfState))
            {
                ImplictConversionToState(expression, typeInfo);
                return;
            }

            var isNullable = false;
            var isReadonly = false;
            var needCloneStruct = expression is not ObjectCreationExpressionSyntax &&
                                  expression is not InvocationExpressionSyntax &&
                                  expression is not LiteralExpressionSyntax &&
                                  typeInfo.IsStructType(out isNullable, out isReadonly, TypeOfNullable);
            if (isReadonly)
            {
                // readonly的结构体始终不需要复制
                needCloneStruct = false;
            }
            else
            {
                //排除除??外的BinaryExpression
                if (needCloneStruct &&
                    expression is BinaryExpressionSyntax binaryExpressionSyntax &&
                    binaryExpressionSyntax.OperatorToken.Kind() !=
                    SyntaxKind.QuestionQuestionToken)
                {
                    needCloneStruct = false;
                }

                //排除 aa?.GetSomeStructValue()
                if (needCloneStruct && expression is ConditionalAccessExpressionSyntax
                    {
                        WhenNotNull: InvocationExpressionSyntax
                    })
                {
                    needCloneStruct = false;
                }

                //特殊排除一些方法传参不需要clone, eg: Canvas.DrawRect(in Rect rect)
                if (needCloneStruct && expression.Parent is ArgumentSyntax argumentSyntax)
                {
                    var argList = (ArgumentListSyntax)argumentSyntax.Parent!;
                    var argIndex = argList.Arguments.IndexOf(argumentSyntax);
                    var symbol = SemanticModel.GetSymbolInfo(argumentSyntax.Parent!.Parent!).Symbol;
                    if (symbol is IMethodSymbol methodSymbol &&
                        methodSymbol.Parameters[argIndex].RefKind == RefKind.In)
                        needCloneStruct = false;
                }
            }

            //TODO:减少不必要的()对
            if (needCloneStruct)
                Write('(');

            //特殊处理long to number的转换
            var isLongToNumber = typeInfo is
            {
                Type: { SpecialType: SpecialType.System_Int64 or SpecialType.System_UInt64 },
                ConvertedType:
                {
                    SpecialType: SpecialType.System_Int16
                    or SpecialType.System_Int32 or SpecialType.System_UInt16
                    or SpecialType.System_UInt32 or SpecialType.System_Single
                    or SpecialType.System_Double
                }
            };
            if (isLongToNumber)
                Write("Number(");

            Visit(expression);

            if (isLongToNumber)
                Write(')');
            if (needCloneStruct)
                Write(isNullable ? ")?.Clone()" : ").Clone()");
        }

        /// <summary>
        /// 隐式转换类型
        /// </summary>
        private void ImplictConversionToState(ExpressionSyntax expression, TypeInfo typeInfo)
        {
            // var isStateType = typeInfo.ConvertedType!.IsStateType(TypeOfState);
            // if (!isStateType)
            //     throw new NotImplementedException();

            Write("PixUI.State.op_Implicit_From(");
            // DisableVisitTrailingTrivia();
            Visit(expression);
            // EnableVisitTrailingTrivia();
            Write(')');
            WriteTrailingTrivia(expression);
        }
    }
}