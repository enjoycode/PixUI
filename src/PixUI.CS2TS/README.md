# 前端配置

## TypeScript必须配置 
* "strictNullChecks": false
* "noImplicitAny": false

# TODO:

### 同一文件定义多个类型时如果有依赖关系需要先排序

```c#
//file.cs
class B : A {}
class A {}
```

# Limits (Not supported C#)

* namespace转module

* 事件最多支持一个参数

```c#
public event Action<string/*这里最多一个参数*/>? ValueChanged;
```

* 静态范型方法必须显示指明范型类型

```csharp
class Node<T>
{
    public static SomeMethod<TNode/*这里必须且不要与类型范型参数同名*/>(Node<TNode> node){}
}
```

* 变量名称不支持@XXX

```c#
void SayHello(string @string) {}
```

* 命名的参数

```c#
CallSomeMethod(namedarg: 32);
```

* 简单Lambda不支持 =>前换行

```c#
let lambda = (a, b)
   /*在箭头前换行不支持*/ => return a + b;
```

* Override操作符暂只支持+ - * / > >= < <= == !=，且不支持重载

* default(xxxx) 不支持

* 静态范型方法不能省略范型参数

## 暂不支持

* partial (TODO)

```c#
//file1.cs
partial class Person {}
//file2.cs
partial class Person {}
```

* Generic type overloads (TODO)

```c#
delegate void Action<T>(T arg);
delegate void Action<T1, T2>(T1 arg1, T2 arg2);
```

* constructor & method overloads

```c#
class Person {
    void Hello() {}
    void Hello(string name) {}
}
```

临时解决方案(仅适用于方法，构造暂不支持，考虑Factory标记转为静态方法):

```c#
class Person {
    [TSRename("Hello")] void Hello() {}
    [TSRename("HelloByName")] void Hello(string name) {}
}
```

* pattern

```c#
var isPerson = obj is Person person;
```

```c#
if (obj is not Expanded expanded) {}
```

```c#
if (widget.Parent /*MemberAccess不支持*/ is IScrollable scrollable) {}
```

* List<T>

1. BinarySearch(T: item, IComparer<T>)的其他重载不支持
2. Sort(Comparison<T> comparison)的其他重载不支持

### 不支持的系统类型或成员

* WeakReference的范型版
* Thread & Task