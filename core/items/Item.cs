using Newtonsoft.Json;

namespace PfannenkuchenBot;
class Item : GameObject
{
    static Item()
    {
        loadedItems = new Dictionary<string, Item>();
        LoadAllItems();
    }

    static void LoadAllItems()
    {
        string itemsDirectory = "content/items";
        foreach (string itemPath in Directory.GetFiles(itemsDirectory, "*json", SearchOption.AllDirectories))
        {
            LoadItem(itemPath);
        }

        static void LoadItem(string itemPath)
        {
            Item? item = JsonConvert.DeserializeObject<Item>(File.ReadAllText(itemPath));
            if (item == null) throw new JsonSerializationException($"Tried to laod invalid item from file: {itemPath}");
            loadedItems.Add(item.Name, item);
        }
    }
    static public bool GetItem(string itemName, out Item item)
    {
        bool successfull = !loadedItems.TryGetValue(itemName, out Item? _item);
        item = (_item is not null) ? _item : throw new KeyNotFoundException($"Invalid item detected at: {itemName}");
        return successfull;
    }
    static Dictionary<string, Item> loadedItems;

    public Rarity Rarity
    { get; private set; }
}