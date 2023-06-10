namespace PfannenkuchenBot;
class Item : GameElement
{
    public static bool Get(string name, out Item item)
    {
        bool successful = ItemLoader.Get(name, out item);
        return successful;
    }
    
    public uint BuyPrice
    { get; protected set; }

    public uint SellPrice
    { get; protected set; }

}