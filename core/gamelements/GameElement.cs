namespace PfannenkuchenBot;
abstract partial class GameElement
{
    public GameElement()
    {
        Id = null!;
        Name = null!;
        Description = null!;
        Creator = null!;
        Tags = null!;
    }

    public string Name
    { get; protected set; }

    public string Id
    { get; protected set; }

    public Rarity Rarity
    { get; protected set; }

    public string Description
    { get; protected set; }

    public string Creator
    { get; protected set; }

    public String[] Tags
    { get; protected set; }
    
    public override string ToString() => Id;
    public virtual string Describe() => $"""
    **{Name}**
    Description: {Description}
    Rarity: {Enum.GetName(typeof(Rarity), Rarity)}
    """;
}