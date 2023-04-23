using System.Text.Json.Serialization;

namespace PfannenkuchenBot;
abstract class GameElement
{
    [JsonConstructor]
    public GameElement()
    {
        Name = String.Empty;
        Rarity = 0;
        Description = String.Empty;
    }
    // public GameElement( [JsonPropertyName("Name")] string? name = null, string? description = null, Rarity? rarity = null)
    // {
    //     this.Name = name ?? throw new InvalidGameObjectException(nameof(name));
    //     this.Description = description ?? throw new InvalidGameObjectException(nameof(description));
    //     this.Rarity = rarity ?? throw new InvalidGameObjectException(nameof(rarity));
    // }
    [JsonPropertyName("Name")]
    public string Name
    { get; private set; }
    [JsonPropertyName("Rarity")]
    public Rarity Rarity
    { get; private set; }
    [JsonPropertyName("Description")]
    public string Description
    { get; private set; }
    public override string ToString() => Name;
    public virtual string Describe() => $"Name: {Name}\nDescription: {Description}";
}