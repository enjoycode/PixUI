using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynUtils;

namespace PixUI.CS2TS
{
    internal static class SymbolExtensions
    {
        internal static INamespaceSymbol? GetRootNamespace(this ISymbol symbol)
        {
            var temp = symbol.ContainingNamespace;
            while (!temp.IsGlobalNamespace)
            {
                if (temp.ContainingNamespace.IsGlobalNamespace)
                    return temp;
                temp = temp.ContainingNamespace;
            }

            return null;
        }

        internal static bool IsSystemNamespace(this ISymbol symbol)
        {
            var root = symbol.GetRootNamespace();
            return root is { Name: "System" };
        }

        internal static bool IsClassType(this BaseTypeSyntax baseType, SemanticModel semanticModel)
        {
            var symbolInfo = semanticModel.GetSymbolInfo(baseType.Type);
            var symbol = (ITypeSymbol)symbolInfo.Symbol!;
            return symbol.TypeKind == TypeKind.Class;
        }

        internal static bool IsNullableType(this ITypeSymbol symbol, INamedTypeSymbol nullableType)
        {
            return symbol.IsValueType &&
                   SymbolEqualityComparer.Default.Equals(nullableType, symbol.OriginalDefinition);
        }

        internal static bool IsStateType(this ITypeSymbol symbol, INamedTypeSymbol stateType)
        {
            //TODO:考虑判断继承自
            return symbol.IsReferenceType &&
                   SymbolEqualityComparer.Default.Equals(stateType, symbol.OriginalDefinition);
        }

        internal static bool IsTSInterfaceOfAttribute(this ITypeSymbol symbol, INamedTypeSymbol interfaceOfType)
        {
            return symbol.TryGetAttribute(interfaceOfType) != null;
        }

        internal static bool IsImplements(this ITypeSymbol symbol, ITypeSymbol type)
        {
            //注意擦除范型类型
            return symbol.AllInterfaces.Any(t => t.IsGenericType
                ? SymbolEqualityComparer.Default.Equals(t.OriginalDefinition, type)
                : SymbolEqualityComparer.Default.Equals(t, type));
        }

        /// <summary>
        /// 是否需要隐式转换为State<T>类型
        /// </summary>
        internal static bool IsImplictConversionToState(this TypeInfo typeInfo, INamedTypeSymbol stateType)
        {
            if (typeInfo.Type == null || typeInfo.ConvertedType == null) return false;

            var same = SymbolEqualityComparer.Default.Equals(typeInfo.Type, typeInfo.ConvertedType);
            if (!same)
            {
                return typeInfo.ConvertedType!.IsStateType(stateType) &&
                       !typeInfo.Type.IsInherits(typeInfo.ConvertedType!);

                //判断是否全是数字类型，是则不需要转换
                //return !(typeInfo.Type.IsNumber() && typeInfo.ConvertedType.IsNumber());
            }

            return false;
        }

        /// <summary>
        /// 是否结构体类型，排除系统内置(eg: number or boolean)类型
        /// </summary>
        internal static bool IsStructType(this TypeInfo typeInfo, out bool isNullable, out bool isReadonly,
            INamedTypeSymbol nullableType)
        {
            isNullable = false;
            isReadonly = false;
            if (typeInfo.Type == null) return false;

            isNullable = typeInfo.Type.IsNullableType(nullableType);
            var type = isNullable
                ? ((INamedTypeSymbol)typeInfo.Type).TypeArguments[0]
                : typeInfo.Type;

            if (type.IsNumber() || type.SpecialType == SpecialType.System_Boolean) return false;

            isReadonly = type.IsReadOnly;
            return type.TypeKind == TypeKind.Struct;
        }

        public static bool IsNumber(this ITypeSymbol symbol)
        {
            return symbol.SpecialType switch
            {
                SpecialType.System_SByte => true,
                SpecialType.System_Byte => true,
                SpecialType.System_Char => true,
                SpecialType.System_Single => true,
                SpecialType.System_Double => true,
                SpecialType.System_Int16 => true,
                SpecialType.System_Int32 => true,
                SpecialType.System_Int64 => true,
                SpecialType.System_UInt16 => true,
                SpecialType.System_UInt32 => true,
                SpecialType.System_UInt64 => true,
                _ => false
            };
        }

        internal static AttributeData? TryGetAttribute(this ISymbol symbol, INamedTypeSymbol attributeTypeSymbol)
        {
            return symbol.GetAttributes().FirstOrDefault(s =>
                SymbolEqualityComparer.Default.Equals(s.AttributeClass, attributeTypeSymbol));
        }
    }
}