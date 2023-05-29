
public class TestLiteral
{
    void Test()
    {
        string a = @"line1
line2";
        string b = "str";
        string c = $"{a}, {b}";
    }

    void TestLong()
    {
        long a = long.MinValue;
    }
}