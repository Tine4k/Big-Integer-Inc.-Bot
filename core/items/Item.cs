namespace PfannenkuchenBot;
class Item : GameElement
{
    public static bool Get(string id, out Item item)
    {
        bool successful = ItemLoader.Get(id, out item);
        return successful;
    }
    
    public uint BuyPrice
    { get; protected set; }

    public uint SellPrice
    { get; protected set; }

}