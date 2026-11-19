namespace PixUI;

public sealed class StyleValueInvalidException : Exception
{
    public StyleValueInvalidException(string className, string propertyName, string validator)
    {
        ClassName = className;
        PropertyName = propertyName;
        Validator = validator;
    }

    public string ClassName { get; }
    public string PropertyName { get; }
    public string Validator { get; }
}