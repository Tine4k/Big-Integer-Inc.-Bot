namespace PfannenkuchenBot;
using System.Text;
class Inventory
{
    Dictionary<Item, ulong> data = new Dictionary<Item, ulong>();
    public void Add(Item item, uint amount = 1)
    {
        if (data.ContainsKey(item)) data[item] += amount;
        else data.Add(item, amount);
    }
    public void Add(string itemName, uint amount = 1)
    {
        if (!Item.GetItem(itemName, out Item item)) throw new KeyNotFoundException();
        Add(item,amount);
    }
    public bool Remove(Item item, uint amount = 1) // Returns true if player has enough Items;
    {
        if (!data.ContainsKey(item)) return false;
        if (amount > data[item])
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
    public bool Remove(string itemName, uint amount = 1) // Returns true if player has enough Items;
    {
        if (!Item.GetItem(itemName, out Item item)) throw new KeyNotFoundException();
        return Remove(item,amount);
    }

    public void Clear()
    {
        data = new Dictionary<Item, ulong>();
    }
    public string PrintContent()
    {
        StringBuilder message = new StringBuilder();
        foreach (KeyValuePair<Item, ulong> pair in data) message.Append($"\n{pair.Value} of {pair.Key}");
        return message.ToString();
    }
    public IEnumerator<KeyValuePair<Item,ulong>> GetEnumerator()
    {
        return data.GetEnumerator();
    }
}