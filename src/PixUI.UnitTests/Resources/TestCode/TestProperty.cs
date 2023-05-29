abstract class Nameable
{
    public abstract string Name { get; set; }
}

class Person : Nameable
{
    public Person()
    {
        //构造内允许初始化readonly AutoProperty
        GetOnlyProperty = "NewValue";
        IsMan = false;
    }
    
    public string GetOnlyProperty { get; }
    
    public override string Name { get; set; }
    
    public int Age { get; private set; }
    
    public bool IsMan { get; } = true;
    
    private int _score;
    public int Score
    {
        get => _score;
        protected set => _score = value;
    }
    
    public int Score2
    {
        get
        {
            System.Console.Write("Getter");
            return _score;
        }
        set
        {
            System.Console.Write("Setter");
            _score = value;
        }
    }
    
}