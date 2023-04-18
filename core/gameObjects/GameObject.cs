using Newtonsoft.Json;

namespace PfannenkuchenBot;
abstract class GameObject
{
    [JsonConstructor]
    public GameObject(string? name = null, string? description = null)
    {
        this.Name = name ?? throw new InvalidContentException(nameof(name));
        this.Description = description ?? throw new InvalidContentException(nameof(description));
    }
    [JsonProperty]
    public string Name
    { get; private set; }
    [JsonProperty]
    public string Description
    { get; private set; }
    public virtual string Describe()
    {
        return $"Name: {Name}\nDescription: {Description}";
    }
}
public class InvalidContentException : System.Exception
{
    public InvalidContentException() { }
    public InvalidContentException(string message) : base(message) { }
    public InvalidContentException(string message, System.Exception inner) : base(message, inner) { }
    protected InvalidContentException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}