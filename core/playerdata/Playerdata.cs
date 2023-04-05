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
        if (Config.logUnloads) Program.Log($"Playerdata of {userId} was saved to file \"playerdata/{userId}\"");
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
    public void Gain(string itemName, uint amount = 1)
    {
        inventory.Add(itemName, amount);
    }
    public void Gain(Item item, uint amount = 1)
    {
        inventory.Add(item, amount);
    }
    public void Gain(Inventory items)
    {
        inventory.Add(items);
    }
    public void Gain(int amount)
    {
        Balance += amount;
    }
    public bool Lose(string itemName, uint amount = 1)
    {
        return inventory.Remove(itemName, amount);
    }
    public bool Lose(Inventory items)
    {
        return inventory.Remove(items);
    }
    public bool Lose(uint amount, bool causeDepths = false)
    {
        if (causeDepths || amount >= balance)
        {
            balance -= amount;
            return true;
        }
        else return false;
    }
    public void Reset()
    {
        inventory.Clear();
        balance = 0;
        Stats = new Dictionary<Stat, int>();
    }
    public string PrintInventory() => inventory.PrintContent();

    public Dictionary<Stat, int> Stats
    { get; set; }
    public string userId;
    public long Balance
    { get; private set; }
    void Save()
    {
        File.WriteAllText($"playerdata/{userId}.dat", JsonConvert.SerializeObject(this));
    }
    Inventory inventory;
    long balance;
}