using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace PixUI.CS2TS
{
    /// <summary>
    /// 用于在转换前预先处理类型及类型成员的重载，并且检查不支持的语法
    /// </summary>
    public sealed partial class Scaner : CSharpSyntaxWalker
    {
        public Scaner() : base(SyntaxWalkerDepth.Token)
        {
            //TODO:加入System命名空间的重载, eg: Action<T1,T2>等
        }

        private readonly List<ScanedTypeInfo> _allTypes = new();

        private readonly Dictionary<ISymbol, string> _typeOverloads = new(SymbolEqualityComparer.Default);
        private readonly Dictionary<ISymbol, string> _typeCtorOverloads = new(SymbolEqualityComparer.Default);
        private readonly Dictionary<IMethodSymbol, string> _typeMethodOverloads = new(SymbolEqualityComparer.Default);

        private void AddTypeInfo(string name, ScanedType type, int genericTypeParas, SyntaxNode node)
        {
            _allTypes.Add(new ScanedTypeInfo(name, type, genericTypeParas, node));
        }

        public void BuildTypeOverloads()
        {
            var sameNames = _allTypes
                .GroupBy(x => x.Name, x => x)
                .Where(g => g.Count() > 1);

            foreach (var item in sameNames)
            {
                
            }
        }
    }

    internal readonly struct ScanedTypeInfo
    {
        internal ScanedTypeInfo(string name, ScanedType type, int genericTypeParas, SyntaxNode node)
        {
            Name = name;
            Type = type;
            GenericTypeParas = genericTypeParas;
            Node = node;
        }

        internal readonly string Name;
        internal readonly ScanedType Type;
        internal readonly int GenericTypeParas;
        internal readonly SyntaxNode Node;
    }

    internal enum ScanedType
    {
        Class,
        Struct,
        Enum,
        Delegate
    }
}