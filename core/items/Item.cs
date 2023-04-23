namespace PfannenkuchenBot;
class Item : GameElement
{
    static Item()
    {
        helper = new GameElementHelper<Item>();
    }
    public static bool Get(string name, out Item? item)
    {
        if (helper is null) throw new Exception("Ka was los is");
        bool successful = helper.Get(name, out item);
        return successful;
    }
    public static GameElementHelper<Item> helper;
}