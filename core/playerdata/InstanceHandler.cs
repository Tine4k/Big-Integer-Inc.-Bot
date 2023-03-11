namespace PfannenkuchenBot.Core;
using System.Collections.Generic;
using System.Threading;
class InstanceHandler
{
    static InstanceHandler()
    {
        LoadedInstanceHandlers = new Dictionary<string, InstanceHandler>();
        if (Config.autoUnload) StartAutoUnload(Config.autoUnloadInterval);
    }
    public static InstanceHandler GetInstanceHandler(string userId)
    {
        if (LoadedInstanceHandlers.ContainsKey(userId))
        {
            InstanceHandler instanceHandler = LoadedInstanceHandlers[userId];
            instanceHandler.lastReferenced = DateTime.Now;
            return instanceHandler;
        }
        else return new InstanceHandler(userId);
    }
    static async void StartAutoUnload(ushort autoSaveTime)
    {
        while (await new PeriodicTimer(TimeSpan.FromSeconds(autoSaveTime)).WaitForNextTickAsync())
        {
            await UnloadAllInstanceHandlers();
        }
    }
    static Task UnloadAllInstanceHandlers()
    {
        foreach (InstanceHandler pd in LoadedInstanceHandlers.Values)
        {
            if (Config.forceUnload || pd.lastReferenced - DateTime.Now < TimeSpan.FromSeconds(Config.idleUnloadTime)) pd.Unload();
        }
        return Task.CompletedTask;
    }
    static Dictionary<string, InstanceHandler> LoadedInstanceHandlers;

    // * INSTANCES BEGIN HERE !!!

    InstanceHandler(string userId)
    {
        this.playerdata = Playerdata.GetPlayerdata(userId);
        this.lastReferenced = DateTime.Now;
        this.Load();
    }
    void Load()
    {
        if (!LoadedInstanceHandlers.ContainsKey(playerdata.userId))
        {
            LoadedInstanceHandlers.Add(playerdata.userId, this);
        }
    }
    void Unload()
    {
        LoadedInstanceHandlers.Remove(this.playerdata.userId);
        playerdata.Save();
    }
    public DateTime lastReferenced;
    public Playerdata playerdata;
}