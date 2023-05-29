class TestNullable
{

    private string? name;

    private string age = null!;

    public int? SayHello(string? name)
    {
        this.name = null;
        if (name != null)
           System.Console.Write($"Hello {name}");
        return null;
    }

}