namespace PfannenkuchenBot;
class Inventory{
    Dictionary<ItemIndex, int> data = new Dictionary<ItemIndex, int>();
    public void Add(ItemIndex item, int count = 1)
    {
        if (data.ContainsKey(item))
        {
            data[item] += count;
        }
        else data.Add(item, count);
    }
    public void Remove(ItemIndex item, int count = 1)
    {
        if (data.ContainsKey(item) && data[item]<=count)
        {
            data[item] -= count;
        }
        else data.Remove(item);
    }
    public void Clear()
    {
        data = new Dictionary<ItemIndex,int>();
    }
}