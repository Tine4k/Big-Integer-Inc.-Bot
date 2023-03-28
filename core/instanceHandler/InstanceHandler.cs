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
    static async void StartAutoUnload(ushort autoSaveTime)
    {
        while (await new PeriodicTimer(TimeSpan.FromSeconds(autoSaveTime)).WaitForNextTickAsync())
        {
            await UnloadAllInstanceHandlers();
        }
    }
    public static Task UnloadAllInstanceHandlers()
    {
        foreach (InstanceHandler pd in LoadedInstanceHandlers.Values)
        {
            if (Config.forceUnload || pd.lastReferenced - DateTime.Now < TimeSpan.FromSeconds(Config.idleUnloadTime)) pd.Unload();
        }
        return Task.CompletedTask;
    }
    static Dictionary<string, InstanceHandler> LoadedInstanceHandlers;

    public static InstanceHandler GetInstanceHandler(string userId)
    {
        if (!LoadedInstanceHandlers.TryGetValue(userId, out InstanceHandler? instanceHandler)) return new InstanceHandler(userId);
        instanceHandler ??= new InstanceHandler(userId);
        instanceHandler.lastReferenced = DateTime.Now;
        return instanceHandler;
    }
    // * INSTANCES BEGIN HERE !!!

    InstanceHandler(string userId)
    {
        this.playerdata = Playerdata.GetPlayerdata(userId);
        this.lastReferenced = DateTime.Now;
        this.Load();
    }
    void Load()
    {
        if (!LoadedInstanceHandlers.ContainsKey(playerdata.userId)) LoadedInstanceHandlers.Add(playerdata.userId, this);
    }
    void Unload()
    {
        LoadedInstanceHandlers.Remove(this.playerdata.userId);
    }
    public DateTime lastReferenced;
    public Playerdata playerdata;
}