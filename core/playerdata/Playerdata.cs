using Newtonsoft.Json;
namespace PfannenkuchenBot.Core;
class Playerdata
{
    public Playerdata(string _userId)
    {
        this.inventory = new Inventory();
        this.userId = _userId;
        this.inventory = new Inventory();
        this.Stats = new Dictionary<Stat, int>();
    }
    ~Playerdata()
    {
        Save();
    }
    public static Playerdata GetPlayerdata(string userId)
    {
        if (File.Exists($"playerdata/{userId}.dat")) return DeserializePlayerdata(userId);
        else return new Playerdata(userId);

        Playerdata DeserializePlayerdata(string userId)
        {
            Playerdata? playerdata = JsonConvert.DeserializeObject<Playerdata>(File.ReadAllText($"playerdata/{userId}.dat"));
            playerdata ??= new Playerdata(userId);
            return playerdata;
        }
    }
    public void Gain(string item, uint amount = 1)
    {
        inventory.Add(item, amount);
    }
    public bool Lose(string item, uint amount = 1)
    {
        return inventory.Remove(item, amount);
    }
    public void Gain(int amount)
    {
        Balance += amount;
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
        inventory.Clear();
        balance = 0;
    }
    public string PrintContent() => inventory.PrintContent();   
    
    void Save()
    {
        File.WriteAllText($"playerdata/{userId}.dat", JsonConvert.SerializeObject(this));
    }
    public Dictionary<Stat, int> Stats
    {get;set;}
    public string userId;
    public long Balance
    { get; private set; }
    Inventory inventory;
    long balance;
}