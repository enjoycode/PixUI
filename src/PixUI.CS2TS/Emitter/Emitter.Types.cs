using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynUtils;

namespace PixUI.CS2TS
{
    public partial class Emitter
    {
        #region ====Const TypeNames====

        internal const string PixUIProjectName = nameof(PixUI);

        internal const string TSTemplateAttributeFullName = "PixUI.TSTemplateAttribute";
        internal const string TSIgnoreMethodInvokeAttributeFullName = "PixUI.TSIgnoreMethodInvokeAttribute";
        internal const string TSCustomInterceptorAttributeFullName = "PixUI.TSCustomInterceptorAttribute";
        internal const string TSPropertyToGetSetAttributeFullName = "PixUI.TSPropertyToGetSetAttribute";

        internal static bool IsTSTypeAttribute(AttributeSyntax attribute)
            => IsAttribute(attribute, "TSType");

        internal static bool IsTSRenameAttribute(AttributeSyntax attribute)
            => IsAttribute(attribute, "TSRename");

        internal static bool IsTSInterfaceOfAttribute(AttributeSyntax attribute)
            => IsAttribute(attribute, "TSInterfaceOf");

        internal static bool IsTSIgnorePropertyDeclarationAttribute(AttributeSyntax attribute)
            => IsAttribute(attribute, "TSIgnorePropertyDeclaration");

        internal static bool IsTSRawScriptAttribute(AttributeSyntax attribute)
            => IsAttribute(attribute, "TSRawScript");

        internal static bool IsTSIndexerSetToMethodAttribute(AttributeSyntax attribute)
            => IsAttribute(attribute, "TSIndexerSetToMethod");

        private static bool IsAttribute(AttributeSyntax attribute, string shortName)
        {
            //TODO:优化
            var name = attribute.Name.ToString();
            if (name == shortName) return true;

            return name == $"{shortName}Attribute"
                   || name == $"PixUI.{shortName}"
                   || name == $"PixUI.{shortName}Attribute";
        }

        #endregion

        #region =====Type Symbols====

        private readonly TypeSymbolCache _typeSymbolCache;

        private INamedTypeSymbol TypeOfICollection =>
            _typeSymbolCache.GetTypeByName("System.Collections.ICollection");

        private INamedTypeSymbol TypeOfICollectionGeneric =>
            _typeSymbolCache.GetTypeByName("System.Collections.Generic.ICollection`1");

        private INamedTypeSymbol TypeOfIDictionary =>
            _typeSymbolCache.GetTypeByName("System.Collections.IDictionary");

        private INamedTypeSymbol TypeOfIDictionaryGeneric =>
            _typeSymbolCache.GetTypeByName("System.Collections.Generic.IDictionary`2");

        private INamedTypeSymbol TypeOfIEnumerable =>
            _typeSymbolCache.GetTypeByName("System.Collections.Generic.IEnumerable`1");

        private INamedTypeSymbol TypeOfNullable => _typeSymbolCache.GetTypeByName("System.Nullable`1");

        private INamedTypeSymbol TypeOfCallerMemberName =>
            _typeSymbolCache.GetTypeByName("System.Runtime.CompilerServices.CallerMemberNameAttribute");

        private INamedTypeSymbol TypeOfDelegate => _typeSymbolCache.GetTypeByName("System.Delegate");

        private INamedTypeSymbol TypeOfState => _typeSymbolCache.GetTypeByName("PixUI.State`1");

        private INamedTypeSymbol TypeOfTSTypeAttribute => _typeSymbolCache.GetTypeByName("PixUI.TSTypeAttribute");

        private INamedTypeSymbol TypeOfTSInterfaceOfAttribute =>
            _typeSymbolCache.GetTypeByName("PixUI.TSInterfaceOfAttribute");

        private INamedTypeSymbol TypeOfTSRenameAttribute
            => _typeSymbolCache.GetTypeByName("PixUI.TSRenameAttribute");

        private INamedTypeSymbol TypeOfTSInterceptorAttribute =>
            _typeSymbolCache.GetTypeByName("PixUI.TSInterceptorAttribute");

        private INamedTypeSymbol TypeOfTSIndexerSetToMethodAttribute =>
            _typeSymbolCache.GetTypeByName("PixUI.TSIndexerSetToMethodAttribute");

        #endregion

        internal bool IsCollectionType(INamedTypeSymbol type)
        {
            //先判断本身是否ICollection
            var isCollection = type.TypeKind == TypeKind.Interface &&
                               (
                                   SymbolEqualityComparer.Default.Equals(type, TypeOfICollection)
                                   ||
                                   SymbolEqualityComparer.Default.Equals(type.OriginalDefinition,
                                       TypeOfICollectionGeneric)
                               );
            //再判断有无实现ICollection接口
            if (!isCollection)
                isCollection = type.AllInterfaces.Any(t =>
                    SymbolEqualityComparer.Default.Equals(t, TypeOfICollection)
                    ||
                    SymbolEqualityComparer.Default.Equals(t.OriginalDefinition, TypeOfICollectionGeneric)
                );
            return isCollection;
        }

        private bool IsDictionayType(ITypeSymbol? type)
        {
            if (type == null) return false;

            //先判断本身是否
            var isDictionayType = type.TypeKind == TypeKind.Interface &&
                                  (
                                      SymbolEqualityComparer.Default.Equals(type, TypeOfIDictionary)
                                      ||
                                      SymbolEqualityComparer.Default.Equals(type.OriginalDefinition,
                                          TypeOfIDictionaryGeneric)
                                  );
            //再判断有无实现接口
            if (!isDictionayType)
                isDictionayType = type.AllInterfaces.Any(t =>
                    SymbolEqualityComparer.Default.Equals(t, TypeOfIDictionary)
                    ||
                    SymbolEqualityComparer.Default.Equals(t.OriginalDefinition, TypeOfIDictionaryGeneric)
                );
            return isDictionayType;
        }

        private bool IsIEnumerableType(ITypeSymbol? type)
        {
            if (type == null) return false;
            
            return type.TypeKind == TypeKind.Interface &&
                   (
                       SymbolEqualityComparer.Default.Equals(type, TypeOfIEnumerable)
                       ||
                       SymbolEqualityComparer.Default.Equals(type.OriginalDefinition, TypeOfIEnumerable)
                   );
        }

        private bool HasCallerMemberNameAttribute(IParameterSymbol symbol)
            => symbol.TryGetAttribute(TypeOfCallerMemberName) != null;

        private bool TryGetInterceptor(ISymbol? symbol, out ITSInterceptor? interceptor)
        {
            interceptor = null;
            if (symbol == null || symbol.IsSystemNamespace()) return false;

            var interceptorAttribute = symbol.GetAttributes()
                .SingleOrDefault(t => t.AttributeClass != null &&
                                      t.AttributeClass.IsInherits(TypeOfTSInterceptorAttribute));

            if (interceptorAttribute == null) return false;

            interceptor = TSInterceptorFactory.Make(interceptorAttribute);
            return true;
        }

        /// <summary>
        /// 尝试重命名Symbol
        /// 1.TSRenameAttribute的成员
        /// 2.ToString()
        /// </summary>
        internal void TryRenameSymbol(ISymbol symbol, ref string name)
        {
            if (symbol is not IPropertySymbol && symbol is not IFieldSymbol && symbol is not IMethodSymbol &&
                symbol is not INamedTypeSymbol)
                return;

            if (symbol is IMethodSymbol method && name == "ToString" && method.Parameters.Length == 0)
            {
                name = "toString";
                return;
            }

            if (symbol.IsSystemNamespace()) return;

            var renameAttribute = symbol.TryGetAttribute(TypeOfTSRenameAttribute);
            if (renameAttribute == null) return;

            name = renameAttribute.ConstructorArguments[0].Value!.ToString();
        }

        /// <summary>
        /// 尝试重命名具备TSRenameAttribute的类型定义
        /// </summary>
        private static bool TryRenameDeclaration(SyntaxList<AttributeListSyntax> attributes, ref string name)
        {
            var attribute = SyntaxExtensions.TryGetAttribute(attributes, IsTSRenameAttribute);
            if (attribute == null) return false;

            var nameLiteral = (LiteralExpressionSyntax)attribute.ArgumentList!.Arguments[0].Expression;
            name = nameLiteral.Token.ValueText;
            return true;
        }

        /// <summary>
        /// 检查类或结构体的成员重载，如果存在且未标记为TSRenameAttribute则抛出异常
        /// </summary>
        private static void CheckTypeMemberOverloads(TypeDeclarationSyntax typeDeclaration)
        {
            var ctors = typeDeclaration.Members.OfType<ConstructorDeclarationSyntax>().Count();
            if (ctors > 1)
                throw new Exception($"类型[{typeDeclaration.Identifier}]具备构造重载,请重写为工厂方法");
            // if (ctors.Length > 1)
            // {
            //     var renamedCount = ctors.Count(c =>
            //         SyntaxExtensions.TryGetAttribute(c.AttributeLists, IsTSRenameAttribute) != null);
            //     if (renamedCount < ctors.Length - 1)
            //         throw new Exception($"类型[{typeDeclaration.Identifier}]具备构造重载,请用TSRenameAttribute改为工厂方法");
            // }

            var methods = typeDeclaration.Members.OfType<MethodDeclarationSyntax>()
                .Where(m => m.Identifier.Text != "Equals") //暂排除
                .GroupBy(m => m.Identifier.Text)
                .Where(g => g.Count() > 1);
            foreach (var group in methods)
            {
                var renamedCount = group.Count(m =>
                    SyntaxExtensions.TryGetAttribute(m.AttributeLists, IsTSRenameAttribute) != null);
                if (renamedCount < group.Count() - 1)
                    throw new Exception(
                        $"方法[{typeDeclaration.Identifier}.{group.Key}]具备重载,请用TSRenameAttribute改变名称");
            }
        }
    }
}