using System.Text.Json;
using System.Text.Json.Serialization;
namespace PfannenkuchenBot;
class Playerdata
{
    [JsonConstructor]
    public Playerdata(string username, Inventory inventory, Dictionary<Stat,int> stats, long balance, Dictionary<string, DateTime> timestamps)
    {
        this.username = username;
        this.Inventory = inventory ?? new Inventory();
        this.Stats = stats ?? new Dictionary<Stat, int>();
        this.Balance = balance;
        this.Timestamps = timestamps ?? new Dictionary<string, DateTime>();
    }
    Playerdata(string _userId)
    {
        this.username = _userId;
        this.Inventory = new Inventory();
        this.Stats = new Dictionary<Stat, int>();
        this.Timestamps = new Dictionary<string, DateTime>();
    }
    public static Playerdata GetPlayerdata(string username)
    {
        if (File.Exists($"playerdata/{username}.dat")) 
        {
            Playerdata playerdata = DeserializePlayerdata(username);
            if (Config.logPlayerdataLoads) Program.Log($"Loaded Playerdata with username {username}", "Playerdata");
            return playerdata;
        }
        else
        {
            if (Config.logPlayerdataCreations) Program.Log($"Created new playerdata with username {username}", "Playerdata");
            return new Playerdata(username);
        }

        Playerdata DeserializePlayerdata(string username)
        {
            string playerdataJson = File.ReadAllText($"playerdata/{username}.dat");
            if (String.IsNullOrWhiteSpace(playerdataJson)) throw new Exception($"Failed to deserialize playerdata of {username}");
            Playerdata? playerdata = JsonSerializer.Deserialize<Playerdata>(playerdataJson);
            if (playerdata is null) throw new Exception($"Failed to deserialize playerdata of {username}");
            return playerdata;
        }
    }

    public void Gain(string id, uint amount = 1)
    {
        Inventory.Add(id, amount);
    }
    public void Gain(Item item, uint amount = 1)
    {
        Inventory.Add(item, amount);
    }
    public void Gain(Inventory items)
    {
        Inventory.Add(items);
    }
    public void Gain(uint amount)
    {
        Balance += amount;
    }

    public bool TryLose(string id, uint amount = 1) // Returns true if player has enough Items and removes amount
    {
        return Inventory.TryRemove(id, amount);
    }
    public bool TryLose(Item item, uint amount = 1)
    {
        return Inventory.TryRemove(item, amount);
    }
    public bool TryLose(Inventory items)
    {
        return Inventory.TryRemove(items);
    }
    public bool TryLose(uint amount, bool causeDepths = true)
    {
        if (Balance < amount || !causeDepths) return false;
        Balance -= amount;
        return true;
    }
    
    public void ForceLose(string id, uint amount = 1) // Removes amount even if player doesn't have enough Items 
    {
        Inventory.Remove(id, amount);
    }
    public void ForceLose(Item item, uint amount = 1) 
    {
        Inventory.Remove(item, amount);
    }
    public void ForceLose(Inventory items)
    {
        Inventory.Remove(items);
    }
    public void ForceLose(uint amount)
    {
        if (Balance < amount) Balance = 0;
        else Balance -= amount;        
    }

    public void Reset()
    {
        Inventory.Clear();
        Balance = 0;
        Stats = new Dictionary<Stat, int>();
    }

    public string PrintContent() => Inventory.PrintContent();
    
    public void Save()
    {
        File.WriteAllText($"playerdata/{username}.dat", JsonSerializer.Serialize(this));
        if (Config.logPlayerdataUnloads) Program.Log($"Playerdata with id {username} was saved to file \"playerdata/{username}.dat\"", "Playerdata");
    }
    
    
    // * All fields that should be serialized have to either be 
    // * a public field (in which case it is recommended to add a [JsonProperty("[fieldname in CamelCase]")] attribute) or 
    // * a property that has a public setter or that is marked with the [JsonProperty] attribute
    // * The order in which the members are displayed here results in the order of serialization (hence userId is the first entry in a .dat file)
    
    
    public string UserName => username;
    public readonly string username;
    
    public Inventory Inventory
    {get; private set;}
    
    public Dictionary<Stat, int> Stats  
    { get; set; }
    
    public long Balance
    { get; private set; }
    
    public Dictionary<string, DateTime> Timestamps 
    { get; private set;}
   
}