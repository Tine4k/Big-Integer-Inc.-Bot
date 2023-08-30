[System.Serializable]
public class InvalidSyntaxException : System.Exception
{
    public InvalidSyntaxException() { }
    public InvalidSyntaxException(string message) : base(message) { }
    public InvalidSyntaxException(string message, System.Exception inner) : base(message, inner) { }
    protected InvalidSyntaxException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}