namespace PfannenkuchenBot.Core;
using System.Collections.Generic;
using System.Threading;
class InstanceHandler
{
    static InstanceHandler()
    {
        loadedInstanceHandlers = new Dictionary<string, InstanceHandler>();
        if (Config.autoUnload) AutoUnloader.Start();
    }
    static Dictionary<string, InstanceHandler> loadedInstanceHandlers;

    public static InstanceHandler GetInstanceHandler(string userId)
    {
        if (!loadedInstanceHandlers.TryGetValue(userId, out InstanceHandler? instanceHandler)) return new InstanceHandler(userId);
        if (instanceHandler is null) throw new Exception("Something went wrong");
        instanceHandler.lastReferenced = DateTime.Now;
        return instanceHandler;
    }
    InstanceHandler(string userId)
    {
        this.playerdata = Playerdata.GetPlayerdata(userId);
        this.lastReferenced = DateTime.Now;
        this.Load();
    }
    void Load()
    {
        loadedInstanceHandlers.Add(playerdata.userId, this);
    }
    void Unload()
    {
        playerdata.Save();
        loadedInstanceHandlers.Remove(this.playerdata.userId);
    }
    public DateTime lastReferenced;
    public Playerdata playerdata;

    private static class AutoUnloader
    {
        public static async void Start()
        {
            while (await new PeriodicTimer(TimeSpan.FromSeconds(Config.autoUnloadInterval)).WaitForNextTickAsync())
            {
                await UnloadAllInstanceHandlers();
            }
        }
        public static Task UnloadAllInstanceHandlers()
        {
            foreach (InstanceHandler pd in loadedInstanceHandlers.Values)
            {
                if (Config.forceUnload || DateTime.Now - pd.lastReferenced < TimeSpan.FromSeconds(Config.idleUnloadTime)) pd.Unload();
            }
            return Task.CompletedTask;
        }
    }
}