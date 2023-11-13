using System.Text.Json;
using System.Text.Json.Serialization;
using PfannenkuchenBot.Commands;
namespace PfannenkuchenBot;
public class Playerdata
{
    [JsonConstructor]
    public Playerdata(string Username, Inventory Inventory, Dictionary<Stat, int> Stats, long Balance, Dictionary<string, DateTime> Timestamps)
    {
        this.Username = Username;
        this.Inventory = Inventory ?? new Inventory();
        this.Stats = Stats ?? new Dictionary<Stat, int>();
        this.Balance = Balance;
        this.Timestamps = Timestamps ?? new Dictionary<string, DateTime>();
    }
    Playerdata(string Username, uint Id = 0)
    {
        this.Username = Username;
        this.Id = Id;
        this.Inventory = new Inventory();
        this.Stats = new Dictionary<Stat, int>();
        this.Timestamps = new Dictionary<string, DateTime>();
    }

    public static Playerdata GetPlayerdata(string Username)
    {
        if (File.Exists($"playerdata/{Username}.dat"))
        {
            Playerdata playerdata = DeserializePlayerdata(Username) ?? new(Username);
            if (Config.logPlayerdataLoads) Logger.Log($"Loaded Playerdata with username {Username}", "Playerdata");
            return playerdata;
        }
        else
        {
            if (Config.logPlayerdataCreations) Logger.Log($"Created new playerdata with username {Username}", "Playerdata");
            return new Playerdata(Username);
        }
    }

    public static bool TryGetPlayerdata(string Username, out Playerdata playerdata)
    {
        // * Die GetPlayerdata Method return eine nullable class
        playerdata = DeserializePlayerdata(Username)!;
        return playerdata is not null;
    }
    
    static Playerdata? DeserializePlayerdata(string username)
    {
        string path = $"playerdata/{username}.dat";
        if (!Directory.Exists(path)) return null; 
        string playerdataJson = File.ReadAllText(path);
        if (String.IsNullOrWhiteSpace(playerdataJson)) return null;
        Playerdata? playerdata = JsonSerializer.Deserialize<Playerdata>(playerdataJson);
        return playerdata;
    }

    public bool Has(Item item, uint amount = 1) => Inventory.Contains(item, amount);
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

    public void Save()
    {
        File.WriteAllText($"playerdata/{Username}.dat", JsonSerializer.Serialize(this));
        if (Config.logPlayerdataUnloads) Logger.Log($"Playerdata with id {Username} was saved to file \"playerdata/{Username}.dat\"", "Playerdata");
    }

    public string PrintContent() => Inventory.PrintContent();
    public string Mention() => CommandHandler.MentionUser(Id);

    // * The order in which the members are displayed here results in the order of serialization (hence Username is the first entry in a .dat file)


    public string Username { get; init; }
    public uint Id { get; init; }

    public Inventory Inventory { get; private set; }

    public long Balance { get; private set; }

    public Dictionary<Stat, int> Stats { get; set; }

    public Dictionary<string, DateTime> Timestamps { get; private set; }

}