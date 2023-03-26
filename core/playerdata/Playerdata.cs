using Newtonsoft.Json;
namespace PfannenkuchenBot.Core;
class Playerdata
{
    public Playerdata(string _userId)
    {
        this.inventory = new Inventory();
        this.userId = _userId;
        this.Inventory = new Inventory();
        this.stats = new Dictionary<Stat, int>();
    }
    ~Playerdata()
    {
        Save();
    }
    public static Playerdata GetPlayerdata(string userId)
    {
        if (File.Exists($"playerdata/{userId}.dat")) return DeserializePlayerdata(userId);
        else return new Playerdata(userId);
        static Playerdata DeserializePlayerdata(string userId)
        {
            Playerdata? pd = JsonConvert.DeserializeObject<Playerdata>(File.ReadAllText($"playerdata/{userId}.dat"));
            if (pd == null) pd = new Playerdata(userId);
            return pd;
        }
    }
    public void Gain(string item, ulong amount = 1)
    {
        Inventory.Add(item, amount);
    }
    public bool Lose(string item, ulong amount = 1)
    {
        return Inventory.Remove(item, amount);
    }
    public void Gain(int amount)
    {
        balance += amount;
    }
    public bool Lose(int amount, bool causeDepths = false)
    {
        if (causeDepths || amount >= balance) 
        {
            balance -= amount;
            return true;
        }
        else return false;
    }
    public void Clear()
    {
        Inventory.Clear();
    }
    void Save()
    {
        File.WriteAllText($"playerdata/{userId}.dat", JsonConvert.SerializeObject(this));
    }
    public Dictionary<Stat, int> stats;
    public string userId;
    public Inventory Inventory
    {get; private set;}
    public long Balance
    {get; private set;}
    Inventory inventory;
    long balance;
}