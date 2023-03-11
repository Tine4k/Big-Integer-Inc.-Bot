using Newtonsoft.Json;
namespace PfannenkuchenBot.Core;
class Playerdata
{
    public Playerdata(string _userId)
    {
        this.userId = _userId;
        this.inventory = new Inventory();
        this.stats = new Dictionary<Stat, int>();
    }
    public static Playerdata GetPlayerdata(string userId)
    {
        if (File.Exists($"playerdata/{userId}.dat"))
        {
            Playerdata? pd = JsonConvert.DeserializeObject<Playerdata>(File.ReadAllText($"playerdata/{userId}.dat"));
            if (pd == null) pd = new Playerdata(userId);
            return pd;
        }
        else return new Playerdata(userId);
    }
    public void Save()
    {
        File.WriteAllText($"playerdata/{userId}.dat", JsonConvert.SerializeObject(this));
    }
    public Inventory inventory;
    public Dictionary<Stat, int> stats;
    public string userId;
    public long balance;
}