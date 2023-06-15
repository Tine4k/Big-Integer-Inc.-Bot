namespace PfannenkuchenBot;
public class InvalidGameObjectException : System.Exception
{
    public InvalidGameObjectException() { }
    public InvalidGameObjectException(string message) : base(message) { }
    public InvalidGameObjectException(string message, System.Exception inner) : base(message, inner) { }
    protected InvalidGameObjectException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}