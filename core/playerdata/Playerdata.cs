using System.Text.Json;
using System.Text.Json.Serialization;
namespace PfannenkuchenBot.Core;
class Playerdata
{
    [JsonConstructor]
    Playerdata(string userId, Inventory inventory)
    {
        this.userId = userId;
        this.Inventory = inventory;
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
            Playerdata? playerdata = JsonSerializer.Deserialize<Playerdata>(File.ReadAllText($"playerdata/{userId}.dat"));
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
    public void Gain(int amount)
    {
        Balance += amount;
    }
    public bool Lose(string itemName, uint amount = 1)
    {
        return Inventory.Remove(itemName, amount);
    }
    public bool Lose(Inventory items)
    {
        return Inventory.Remove(items);
    }
    public bool Lose(uint amount, bool causeDepths = false)
    {
        if (causeDepths || amount >= Balance)
        {
            Balance -= amount;
            return true;
        }
        else return false;
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
    public readonly string userId;
    [JsonPropertyName("Inventory")]
    [JsonConverter(typeof(GameElementHelper<Item>.GameElementConverter<Item>))]
    public Inventory Inventory
    {get; set;}
    [JsonPropertyName("Stats")]
    public Dictionary<Stat, int>? Stats  
    { get; set; }                    /*nullable for the duration of the Stat class not being relevant */
    [JsonPropertyName("Balance")]
    public long Balance
    { get; private set; }
}