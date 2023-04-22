namespace PfannenkuchenBot;
using System.Text;
using Newtonsoft.Json;

class Inventory
{
    public Inventory() : this(new Dictionary<Item, ulong>())
    {
    }
    public void Add(Item item, uint amount = 1)
    {
        if (data.ContainsKey(item)) data[item] += amount;
        else data.Add(item, amount);
    }
    public void Add(string itemName, uint amount = 1)
    {
        if (!Item.GetItem(itemName, out Item item)) throw new KeyNotFoundException();
        this.Add(item, amount);
    }
    public void Add(Inventory items)
    {
        foreach (KeyValuePair<Item, ulong> pair in items) Add(pair.Key, (uint)pair.Value);
    }
    public bool Remove(Item item, uint amount = 1) // Returns true if player has enough Items;
    {
        if (!data.ContainsKey(item)) return false;
        if (amount < data[item])
        {
            data[item] -= amount;
            return true;
        }
        else if (data[item] == amount)
        {
            data.Remove(item);
            return true;
        }
        else return false;
    }
    public bool Remove(Inventory items)
    {
        foreach (KeyValuePair<Item, ulong> pair in items)
        {
            if (!this.Remove(pair.Key, (uint)pair.Value)) return false;
        }
        return true;
    }
    public bool Remove(string itemName, uint amount = 1) // Returns true if player has enough Items;
    {
        if (!Item.GetItem(itemName, out Item item)) throw new KeyNotFoundException();
        return Remove(item, amount);
    }

    public bool Contains(Inventory items)
    {
        foreach (Item item in items.Keys) if (data.Keys.Contains(item)) return false;
        return data.Count() > 0;
    }
    public bool Transfer(Inventory targetInventory, Inventory items)
    {
        if (!this.Contains(items)) return false;
        targetInventory.Add(items);
        this.Remove(items);
        return true;
    }
    public void Clear() => data = new Dictionary<Item, ulong>();

    public string PrintContent()
    {
        StringBuilder message = new StringBuilder();
        foreach (KeyValuePair<Item, ulong> pair in data) message.Append($"\n{pair.Value}x {pair.Key.Name}");
        return message.ToString();
    }
    public static readonly Inventory Empty = new Inventory(new Dictionary<Item, ulong>());
    [JsonProperty]
    Dictionary<Item, ulong> data = new Dictionary<Item, ulong>();
    // * Do not change, not relevant for game design
    public IEnumerator<KeyValuePair<Item, ulong>> GetEnumerator() => data.GetEnumerator();
    Inventory(Dictionary<Item, ulong> _data)
    {
        this.data = _data;
    }
    Dictionary<PfannenkuchenBot.Item, ulong>.KeyCollection Keys
    {
        get
        {
            return data.Keys;
        }
    }
}