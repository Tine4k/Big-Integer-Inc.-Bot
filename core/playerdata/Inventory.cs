namespace PfannenkuchenBot;
using System.Collections;
using System.Collections.Immutable;
using System.Text;
using System.Text.Json.Serialization;
class Inventory : DictionaryBase
{
    public Inventory() : this(new Dictionary<Item, ulong>())
    {}
    [JsonConstructor]
    Inventory(Dictionary<Item, ulong> data)
    {
        this.data = data;
    }
    public void Add(Item item, uint amount = 1)
    {
        if (data.ContainsKey(item)) data[item] += amount;
        else data.Add(item, amount);
    }
    public void Add(string itemName, uint amount = 1)
    {
        if (!Item.Get(itemName, out Item? item)) throw new KeyNotFoundException();
        if (item is null) throw new NullReferenceException();
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
        if (!Item.Get(itemName, out Item? item)) throw new KeyNotFoundException();
        if (item is null) throw new NullReferenceException();
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
    public new void Clear() => data = new Dictionary<Item, ulong>();

    public string PrintContent()
    {
        StringBuilder message = new StringBuilder();
        foreach (KeyValuePair<Item, ulong> pair in data) message.Append($"\n{pair.Value}x {pair.Key.Name}");
        return message.ToString();
    }
    public static readonly Inventory Empty = new Inventory(new Dictionary<Item, ulong>());
    Dictionary<Item, ulong> data = new Dictionary<Item, ulong>();
    // * Do not change, not relevant for game design
    public new IEnumerator<KeyValuePair<Item, ulong>> GetEnumerator() => data.GetEnumerator();
    Dictionary<PfannenkuchenBot.Item, ulong>.KeyCollection Keys => data.Keys;
}