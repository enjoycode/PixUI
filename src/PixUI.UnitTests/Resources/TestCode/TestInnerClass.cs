public class BaseClass {}

public class OuterClass
{
    class InnerClass<T>: BaseClass
    {
        public class LeafClass { }
    }

    public void Test()
    {
        var c1 = new InnerClass<string>();
        var c2 = new OuterClass.InnerClass<string>();
        var c3 = new InnerClass<string>.LeafClass();
        var c4 = new OuterClass.InnerClass<string>.LeafClass();
    }
    
}