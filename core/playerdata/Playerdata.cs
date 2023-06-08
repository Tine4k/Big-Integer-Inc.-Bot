using System.Text.Json;
using System.Text.Json.Serialization;
namespace PfannenkuchenBot;
class Playerdata
{
    Playerdata()
    {
        this.userId = String.Empty;
        this.Inventory = Inventory.Empty;
        this.Stats = new Dictionary<Stat, int>();
        this.Timestamps = new Dictionary<string, DateTime>();
    }
    [JsonConstructor]
    public Playerdata(string userId, Inventory inventory, Dictionary<Stat,int> stats, long balance)
    {
        this.userId = userId;
        this.Inventory = inventory;
        this.Stats = stats;
        this.Balance = balance;
    }
    Playerdata(string _userId)
    {
        this.userId = _userId;
        this.Inventory = new Inventory();
        this.Stats = new Dictionary<Stat, int>();
    }
    public static Playerdata GetPlayerdata(string userId)
    {
        if (File.Exists($"playerdata/{userId}.dat")) 
        {
            Playerdata playerdata = DeserializePlayerdata(userId);
            if (Config.logPlayerdataLoads) Program.Log($"Loaded Playerdata with id {userId}");
            return playerdata;
        }
        else
        {
            if (Config.logPlayerdataCreations) Program.Log($"Created new playerdata with id {userId}");
            return new Playerdata(userId);
        }

        Playerdata DeserializePlayerdata(string userId)
        {
            string playerdataJson = File.ReadAllText($"playerdata/{userId}.dat");
            if (String.IsNullOrWhiteSpace(playerdataJson)) throw new Exception($"Failed to deserialize playerdata with id {userId}");
            Playerdata? playerdata = JsonSerializer.Deserialize<Playerdata>(playerdataJson);
            if (playerdata is null) throw new Exception($"Failed to deserialize playerdata with id {userId}");
            return playerdata;
        }
    }

    public void Gain(string itemName, uint amount = 1)
    {
        Inventory.Add(itemName, amount);
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

    public bool Lose(string itemName, uint amount = 1) // Returns true if player has enough Items and removes amount
    {
        return Inventory.TryRemove(itemName, amount);
    }
    public bool Lose(Inventory items)
    {
        return Inventory.TryRemove(items);
    }
    public bool Lose(uint amount, bool causeDepths = true)
    {
        if (Balance < amount || !causeDepths) return false;
        Balance -= amount;
        return true;
    }
    
    public void ForceLose(string itemName, uint amount = 1) // Removes amount even if player doesn't have enough Items 
    {
        Inventory.Remove(itemName, amount);
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

    public string PrintInventory() => Inventory.PrintContent();
    
    public void Save()
    {
        File.WriteAllText($"playerdata/{userId}.dat", JsonSerializer.Serialize(this));
        if (Config.logPlayerdataUnloads) Program.Log($"Playerdata with id {userId} was saved to file \"playerdata/{userId}.dat\"");
    }
    
    
    // * All fields that should be serialized have to either be 
    // * a public field (in which case it is recommended to add a [JsonProperty("[fieldname in CamelCase]")] attribute) or 
    // * a property that has a public setter or that is marked with the [JsonProperty] attribute
    // * The order in which the members are displayed here results in the order of serialization (hence userId is the first entry in a .dat file)
    
    
    [JsonPropertyName("UserId")]
    public string UserId => userId;
    readonly string userId;
    
    [JsonPropertyName("Inventory")]
    public Inventory Inventory
    {get; private set;}
    
    public Dictionary<Stat, int> Stats  
    { get; set; }
    
    [JsonPropertyName("Balance")]
    public long Balance
    { get; private set; }
    
    [JsonPropertyName("Timestamps")]
    public Dictionary<string, DateTime> Timestamps 
    { get; private set;}
   
    
}