namespace PfannenkuchenBot;
public class Item : GameElement
{
    public static bool Get(string id, out Item item)
    {
        bool successful = GameElementLoader.TryGet(id, out item);
        return successful;
    }
    public override string Describe() => $"""
    **{Name}**
    Description: {Description}
    Rarity: {Enum.GetName(typeof(Rarity), Rarity)}{((BuyPrice == 0) ? string.Empty : $"\nBuy Price: {BuyPrice}")}{((SellPrice == 0) ? string.Empty : $"\nSell Price: {SellPrice}")}
    """;
    
    public uint BuyPrice
    { get; protected set; }

    public uint SellPrice
    { get; protected set; }

}