namespace PfannenkuchenBot;
class Inventory{
    Dictionary<string, int> data = new Dictionary<string, int>();
    public void Add(string item, int count = 1)
    {
        if (data.ContainsKey(item))
        {
            data[item] += count;
        }
        else data.Add(item, count);
    }
    public void Remove(string item, int count = 1)
    {
        if (data.ContainsKey(item) && data[item]<=count)
        {
            data[item] -= count;
        }
        else data.Remove(item);
    }
    public void Clear()
    {
        data = new Dictionary<string,int>();
    }
}