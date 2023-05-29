enum ModelType
{
    A,B,C
}

class TestSwitchExpression
{
    int Test(ModelType type)
    {
        int v = type switch
        {
            ModelType.A => 1,
            ModelType.B => 2,
            ModelType.C => 3,
            _ => 0
        };
        return v;
    }
}