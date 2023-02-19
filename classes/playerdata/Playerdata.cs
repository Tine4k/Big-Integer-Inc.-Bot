namespace PfannenkuchenBot;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
class Playerdata
{
    static Playerdata()
    {
        LoadedPlayerdatas = new Dictionary<string, Playerdata>();
        if (Config.autoUnload) StartAutoUnload(Config.autoUnloadInterval);
    }
    public static Playerdata GetPlayerdata(string id)
    {
        if (LoadedPlayerdatas.ContainsKey(id))
        {
            Playerdata pd = LoadedPlayerdatas[id];
            pd.lastReferenced = DateTime.Now;
            return pd;
        }
        else if (File.Exists($"playerdata/{id}.dat"))
        {
            Playerdata? pd = JsonConvert.DeserializeObject<Playerdata>(File.ReadAllText($"playerdata/{id}.dat"));
            if (pd == null) pd = new Playerdata(id);
            return pd;
        }
        else return new Playerdata(id);
    }
    static async void StartAutoUnload(ushort autoSaveTime)
    {
        while (await new PeriodicTimer(TimeSpan.FromSeconds(autoSaveTime)).WaitForNextTickAsync())
        {
            await UnloadAllPlayerdatas();
        }
    }
    static Task UnloadAllPlayerdatas()
    {
        foreach (Playerdata pd in LoadedPlayerdatas.Values)
        {
            if (Config.forceUnload || pd.lastReferenced - DateTime.Now < TimeSpan.FromSeconds(Config.idleUnloadTime)) pd.Unload();
        }
        return Task.CompletedTask;
    }
    static Dictionary<string, Playerdata> LoadedPlayerdatas;

    // * INSTANCES BEGIN HERE !!!

    Playerdata(string _userId)
    {
        this.userId = _userId;
        this.Inventory = new Dictionary<Item, int>()
        {
            // TODO Standard Items
        };
        this.Stats = new Dictionary<Stat, int>();
        this.lastReferenced = DateTime.Now;
        this.Load();
    }
    void Load()
    {
        if (!LoadedPlayerdatas.ContainsKey(userId))
        {
            LoadedPlayerdatas.Add(userId, this);
        }
    }
    void Unload()
    {
        LoadedPlayerdatas.Remove(this.userId);
        Save();
    }
    void Save()
    {
        File.WriteAllText($"playerdata/{userId}.dat", JsonConvert.SerializeObject(this));
    }
    public Dictionary<Stat, int> Stats
    { get; set; }
    public Dictionary<Item, int> Inventory
    { get; private set; }
    public string userId;
    public DateTime lastReferenced;
    public long balance;
}