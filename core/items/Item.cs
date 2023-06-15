namespace PfannenkuchenBot;
class Item : GameElement
{
    public static bool Get(string id, out Item item)
    {
        bool successful = GameElementLoader<Item>.Get(id, out item);
        return successful;
    }
    public override string Describe() => $"""
    **{Name}**
    Description: {Description}
    Rarity: {Enum.GetName(typeof(Rarity), Rarity)}
    {((BuyPrice == 0) ? String.Empty : $"Buy Price: {BuyPrice}")}
    {((SellPrice == 0) ? String.Empty : $"Sell Price: {SellPrice}")}
    """;
    
    public uint BuyPrice
    { get; protected set; }

    public uint SellPrice
    { get; protected set; }

}