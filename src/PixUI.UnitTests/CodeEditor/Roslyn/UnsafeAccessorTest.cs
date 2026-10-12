using NUnit.Framework;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using PixUI.CodeEditor.Roslyn;
using System.Linq;

namespace PixUI.UnitTests.CodeEditor;

public class UnsafeAccessorTest
{
    [Test]
    public void NodeOrTokenTest()
    {
        // 1. 创建Roslyn SyntaxTree
        var tree = CSharpSyntaxTree.ParseText("int x = 5;");
        var root = tree.GetRoot();

        // 2. 获取任一SyntaxNodeOrToken
        SyntaxNodeOrToken nodeOrToken = root.DescendantNodes().First();

        // 3. 测试由UnsafeAccessor访问私有属性
        // 测试 UnderlyingNode 属性
        var resultNode = nodeOrToken.GetUnderlyingNode();
        Assert.That(resultNode, Is.Not.Null);

        // 测试 Position 属性
        var position = nodeOrToken.GetPosition();
        Assert.That(position, Is.EqualTo(0));

        // 测试 EndPosition 属性
        var endPosition = nodeOrToken.GetEndPosition();
        Assert.That(endPosition, Is.GreaterThanOrEqualTo(0));
    }
}