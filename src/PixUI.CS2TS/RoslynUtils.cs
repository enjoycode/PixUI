using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace RoslynUtils
{
    // PixUI.CS2TS及Design工程通用

    internal sealed class TypeSymbolCache
    {
        internal TypeSymbolCache(SemanticModel semanticModel)
        {
            _semanticModel = semanticModel;
        }

        private readonly SemanticModel _semanticModel;
        private readonly Dictionary<string, INamedTypeSymbol> _typesCache = new();

        //internal INamedTypeSymbol TypeOfEntity => GetTypeByName("AppBoxCore.Entity");
        internal INamedTypeSymbol TypeOfIListGeneric => GetTypeByName("System.Collections.Generic.IList`1");
        internal INamedTypeSymbol TypeOfListGeneric => GetTypeByName("System.Collections.Generic.List`1");

        internal INamedTypeSymbol GetTypeByName(string typeName)
        {
            var type = TryGetTypeByName(typeName);
            if (type == null)
                throw new ArgumentException($"Can't find type by name: {typeName}");
            return type;
        }

        internal INamedTypeSymbol? TryGetTypeByName(string typeName)
        {
            if (_typesCache.TryGetValue(typeName, out var typeSymbol))
                return typeSymbol;

            var type = _semanticModel.Compilation.GetTypeByMetadataName(typeName);
            if (type != null) _typesCache[typeName] = type;
            return type;
        }
    }

    public static class RoslynExtensions
    {
        /// <summary>
        /// 判断当前类型是否继承自指定类型
        /// </summary>
        internal static bool IsInherits(this ITypeSymbol symbol, ITypeSymbol type)
        {
            var baseType = symbol.BaseType;
            while (baseType != null)
            {
                if (SymbolEqualityComparer.Default.Equals(type, baseType))
                    return true;

                baseType = baseType.BaseType;
            }

            return false;
        }

        /// <summary>
        /// 用于检查类型是否包含模型类
        /// </summary>
        internal static void CheckTypeHasAppBoxModel(this ITypeSymbol typeSymbol,
            Func<string, bool> findModel, Action<string> addAction)
        {
            //检查Entity数组
            if (typeSymbol is IArrayTypeSymbol arrayTypeSymbol)
            {
                CheckTypeHasAppBoxModel(arrayTypeSymbol.ElementType, findModel, addAction);
                return;
            }

            //检查其他范型集合
            if (typeSymbol is INamedTypeSymbol { IsGenericType: true } namedTypeSymbol)
            {
                foreach (var typeArgument in namedTypeSymbol.TypeArguments)
                {
                    CheckTypeHasAppBoxModel(typeArgument, findModel, addAction);
                }

                return;
            }

            if (typeSymbol.IsAppBoxEntity(findModel) || typeSymbol.IsAppBoxView(findModel))
                addAction(typeSymbol.ToString());
        }

        /// <summary>
        /// 是否AppBox实体类型
        /// </summary>
        internal static bool IsAppBoxEntity(this ISymbol symbol, Func<string, bool> findModel)
        {
            if (symbol is INamedTypeSymbol typeSymbol &&
                typeSymbol.ContainingNamespace.Name == "Entities")
            {
                var fullName = symbol.ToString();
                return findModel(fullName);
            }

            return false;
        }

        /// <summary>
        /// 是否AppBox视图模型
        /// </summary>
        internal static bool IsAppBoxView(this ISymbol symbol, Func<string, bool> findModel)
        {
            if (symbol is INamedTypeSymbol typeSymbol && typeSymbol.ContainingNamespace.Name == "Views")
            {
                var fullName = symbol.ToString();
                if (fullName.Count(c => c == '.') != 2) //maybe eg: sys.Views.Menu.MenuItem
                    return false;
                return findModel(fullName);
            }

            return false;
        }

        /// <summary>
        /// 是否AppBox服务方法
        /// </summary>
        internal static bool IsAppBoxServiceMethod(this IMethodSymbol symbol)
        {
            if (symbol.ContainingNamespace.Name != "Services") return false;
            var interceptorAttribute = symbol.GetAttributes()
                .SingleOrDefault(t => t.AttributeClass != null &&
                                      t.AttributeClass.ToString() ==
                                      "AppBoxCore.InvocationInterceptorAttribute");
            return interceptorAttribute != null;
        }
    }
}