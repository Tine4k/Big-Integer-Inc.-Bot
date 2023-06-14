namespace PfannenkuchenBot.Commands;
using System.Text;
using Discord.WebSocket;
using Discord;

partial class Command
{
    public Command(string userId)
    {
        this.message = new StringBuilder();
        this.playerdata = Playerdata.GetPlayerdata(userId);
        this.lastReferenced = DateTime.Now;
        this.socketMessage = null!;
        this.commandMessage = null!;
        this.Load();
    }
    public static Command GetCommand(string userId)
    {
        if (!loadedCommands.TryGetValue(userId, out Command? command)) return new Command(userId);
        if (command is null) throw new NullReferenceException("Something went wrong");
        command.lastReferenced = DateTime.Now;
        return command;
    }
    public static Dictionary<string, Command> loadedCommands;
    public static void Unknown(ISocketMessageChannel channel) => 
    channel.SendMessageAsync(Format.BlockQuote($"Unknown Command, please use {Config.prefix}help for a list of available commands!"));
    public virtual void Send()
    {
        socketMessage.Channel.SendMessageAsync(message.ToString());
        message.Insert(0, $"{socketMessage.Author.Username} issued \'{String.Join(' ', commandMessage)}\'\n");
        if (Config.logAllCommands) Program.Log(message.ToString(), "Commands");
        this.lastReferenced = DateTime.Now;
        this.message.Clear();
    }
    // * Loading relevant
    static Command()
    {
        loadedCommands = new Dictionary<string, Command>();
        if (Config.autoUnload) AutoUnloader.Start();
    }

    void Load()
    {
        loadedCommands.Add(playerdata.userId, this);
    }

    void Unload()
    {
        playerdata.Save();
        loadedCommands.Remove(this.playerdata.userId);
    }
    
    // * Field declarations
    public SocketUserMessage socketMessage;
    public String[] commandMessage;
    protected readonly Playerdata playerdata;
    protected DateTime lastReferenced;
    protected readonly StringBuilder message;


    private static class AutoUnloader
    {
        public static async void Start()
        {
            while (await new PeriodicTimer(TimeSpan.FromSeconds(Config.autoUnloadInterval)).WaitForNextTickAsync())
            {
                await UnloadAllCommands();
            }
        }
        public static Task UnloadAllCommands()
        {
            foreach (Command command in loadedCommands.Values)
            {
                if (Config.forceUnload || DateTime.Now - command.lastReferenced >= TimeSpan.FromSeconds(Config.idleUnloadTime)) command.Unload();
            }
            return Task.CompletedTask;
        }
    }
}