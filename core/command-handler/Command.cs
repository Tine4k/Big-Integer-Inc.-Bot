namespace PfannenkuchenBot.Commands;
using System.Text;
using Discord.WebSocket;
using Discord;

partial class Command
{
    public Command(string username)
    {
        this.player = Playerdata.GetPlayerdata(username);
        this.lastReferenced = DateTime.Now;
        this.currentSocketMessage = null!;
        this.currentCommandMessage = null!;
        this.Load();
    }
    public static Command GetCommand(string username)
    {
        if (!loadedCommands.TryGetValue(username, out Command? command)) return new Command(username);
        if (command is null) throw new NullReferenceException("Something went wrong");
        command.lastReferenced = DateTime.Now;
        return command;
    }
    public static Dictionary<string, Command> loadedCommands;
    public static void Unknown(ISocketMessageChannel channel) => 
    channel.SendMessageAsync(Format.BlockQuote($"Unknown Command, please use {Config.prefix}help for a list of available commands!"));
    public virtual void Send()
    {
        currentSocketMessage.Channel.SendMessageAsync(message.ToString());
        message.Insert(0, $"{currentSocketMessage.Author.Username} issued \'{String.Join(' ', currentCommandMessage)}\'\n");
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
        loadedCommands.Add(player.username, this);
    }

    void Unload()
    {
        player.Save();
        loadedCommands.Remove(this.player.username);
    }
    
    // * Field declarations
    public SocketUserMessage currentSocketMessage;
    public String[] currentCommandMessage;
    protected readonly Playerdata player;
    protected DateTime lastReferenced;
    protected readonly StringBuilder message = new StringBuilder();


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