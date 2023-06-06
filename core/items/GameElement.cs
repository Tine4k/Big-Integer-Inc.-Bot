using System.Text.Json.Serialization;

namespace PfannenkuchenBot;
abstract partial class GameElement
{
    public GameElement()
    {
        Name = String.Empty;
        Rarity = default;
        Description = String.Empty;
        Creator = String.Empty;
        Illegal = false;
        
    }
    [JsonPropertyName("Name")]
    public string Name
    { get; protected set; }

    [JsonPropertyName("Rarity")]
    public Rarity Rarity
    { get; protected set; }

    [JsonPropertyName("Description")]
    public string Description
    { get; protected set; }

    [JsonPropertyName("Creator")]
    public string Creator
    { get; protected set; }

    [JsonPropertyName("Illegal")]
    public bool Illegal
    { get; protected set; }  
    
    public override string ToString() => Name;
    public virtual string Describe() => $"Name: {Name}\nDescription: {Description}\nRarity: {Enum.GetName(typeof(Rarity), Rarity)}";
}