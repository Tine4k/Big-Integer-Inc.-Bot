using Newtonsoft.Json;

namespace PfannenkuchenBot;
class Item : GameObject
{
    static Item()
    {
        LoadedItems = new Dictionary<string, Item>();
        LoadAllItems();
    }

    private static void LoadAllItems()
    {
        string itemsDirectory = "content/items";
        foreach (string itemPath in Directory.GetFiles(itemsDirectory, "*json", SearchOption.AllDirectories))
        {
            LoadItem(itemPath);
        }

        static void LoadItem(string itemPath)
        {
            Item? item = JsonConvert.DeserializeObject<Item>(File.ReadAllText(itemPath));
            if (item == null) throw new JsonSerializationException($"Invalid item detected at: {itemPath}");
            LoadedItems.Add(item.Name,item);
        }
    }

    public Rarity Rarity
    {get; private set;}
    public static Dictionary<string, Item> LoadedItems
    { get; private set; }
}