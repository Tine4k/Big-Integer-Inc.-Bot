namespace PfannenkuchenBot;
class Inventory
{
    Dictionary<string, ulong> data = new Dictionary<string, ulong>();
    public void Add(string item, ulong amount = 1)
    {
        if (data.ContainsKey(item))
        {
            data[item] += amount;
        }
        else data.Add(item, amount);
    }
    public bool Remove(string item, ulong amount = 1) // Returns true if player has enough Items;
    {
        if (!Item.LoadedItems.ContainsKey(item)) throw new KeyNotFoundException();
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
    public void Clear()
    {
        data = new Dictionary<string, ulong>();
    }
}