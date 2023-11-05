namespace PfannenkuchenBot.Commands;
using Discord;
using Discord.WebSocket;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

public partial class CommandHandler
{
    // * All the different classes that you can use as parameters in a command 
    static readonly Type[] syntaxParameterTypes;

    // * All the methods with the Command Attributed assigned to them
    public static Dictionary<MethodBase, ParameterInfo[]> LoadedCommands { get; set; }

    // * Every player has a CommandHandler Instance designated to them that handles all of their commands
    public static Dictionary<string, CommandHandler> LoadedCommandHandlers { get; }

    static CommandHandler()
    {
        if (Config.autoUnload) AutoUnloader.Start();

        Type[] gameElementAssignableTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => typeof(GameElement).IsAssignableFrom(type) && type != typeof(GameElement)).ToArray();

        syntaxParameterTypes = gameElementAssignableTypes.Concat(
            new Type[] {
                typeof(ulong),
                typeof(string),
                typeof(bool),
                typeof(Playerdata)
                }).ToArray();

        LoadedCommandHandlers = new Dictionary<string, CommandHandler>();
        LoadedCommands = LoadCommands();
    }

    static Dictionary<MethodBase, ParameterInfo[]> LoadCommands()
    {
        Dictionary<MethodBase, ParameterInfo[]> loadedCommands = new();
        MethodInfo[] CommandClassMethods = typeof(CommandHandler).GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

        foreach (MethodInfo methodInfo in CommandClassMethods)
        {
            if (Attribute.IsDefined(methodInfo, typeof(CommandAttribute)))
            {
                ParameterInfo[] parameters = methodInfo.GetParameters();

                if (parameters.Length == 0) loadedCommands.Add(methodInfo, parameters);
                else foreach (ParameterInfo parameterInfo in parameters)
                        if (!syntaxParameterTypes.Contains(parameterInfo.ParameterType)) 
                            Logger.Log($"Invalid Parameter Types! Wow! Whoever defined {methodInfo.Name} did a really bad job! The command was not loaded!");
                        else loadedCommands.Add(methodInfo, parameters);
            }
        }
        return loadedCommands;
    }

    // * Command Handling 
    
    // public static void EvaluateCommand(string commandMessage)
    // {
        
    // }
    public static void HandleCommand(string[] commandMessage, string username, object platform, Type port)
    {
        CommandHandler commandHandler = CommandHandler.GetCommandHandler(username);
        foreach (KeyValuePair<MethodBase, ParameterInfo[]> pair in LoadedCommands)
            if (pair.Key.Name.Equals(Format.StripMarkDown(commandMessage[0]).ToString(), StringComparison.OrdinalIgnoreCase))
            {
                commandHandler.currentCommandMessage = commandMessage;

                object[] parameters = new object[pair.Value.Length];


                //* This is relevant for parameters, so that the right overload of the method you're trying to invoke can be selected
                //* Also, parsing happens here
                for (int i = 0; i < pair.Value.Length; ++i)
                {
                    if (commandMessage.Length < i + 2) continue;
                    if (pair.Value[i].ParameterType.Equals(typeof(string)))
                    {
                        parameters[i] = commandMessage[i + 1];
                    }
                    else if (pair.Value[i].ParameterType.Equals(typeof(bool)))
                    {
                        if (bool.TryParse(commandMessage[i + 1], out bool boolean)) parameters[i] = boolean;
                    }
                    else if (pair.Value[i].ParameterType.Equals(typeof(ulong)))
                    {
                        if (ulong.TryParse(commandMessage[i + 1], out ulong number)) parameters[i] = number;
                    }
                    else if (pair.Value[i].ParameterType.Equals(typeof(Item)))
                    {
                        if (GameElementLoader.TryGet(commandMessage[i + 1], out Item item)) parameters[i] = item;
                    }
                    else if (pair.Value[i].ParameterType.Equals(typeof(Playerdata)))
                    {
                        parameters[i] = Playerdata.GetPlayerdata(commandMessage[i + 1]); // ! temporary solution, playerdata fetching neads to be reviewed
                    }
                    else throw new Exception("What da heeeeellll");
                }

                //* Here it is assured that the cooldown is over and that it is allowed for the command to be invoked by the user (planned)
                CommandAttribute attribute = pair.Key.GetCustomAttribute<CommandAttribute>()!;
                if (attribute.Cooldown > 0)
                {
                    string timestampName = "lastCalled" + pair.Key.Name;
                    if (!commandHandler.player.Timestamps.TryGetValue(timestampName, out DateTime lastCalled)) commandHandler.player.Timestamps.Add(timestampName, DateTime.MinValue);
                    TimeSpan cooldown = TimeSpan.FromSeconds(attribute.Cooldown);
                    if (DateTime.Now - lastCalled <= cooldown)
                    {
                        TimeSpan nextAvailableCall = lastCalled + cooldown - DateTime.Now;
                        commandHandler.message.Clear();
                        commandHandler.message.Append($"You have to wait **{((nextAvailableCall.Hours < 0) ? (nextAvailableCall.Hours + "h ") : "")}{nextAvailableCall.Minutes}min and {nextAvailableCall.Seconds}s**");
                        commandHandler.Send(platform, port);
                        return;
                    }
                }
                try
                {
                    pair.Key.Invoke(commandHandler, parameters);
                }
                catch (ArgumentException)
                {
                    commandHandler.message.Clear();
                    commandHandler.message.Append($"Well, seems like your System.Reflection based command invocation was not as robust as you thought, Markus. {MentionUser(500953493918449674)}");
                    commandHandler.Send(platform, port);
                    return;
                }
                if (commandHandler.success && attribute.Cooldown > 0)
                {
                    commandHandler.player.Timestamps["lastCalled" + pair.Key.Name] = DateTime.Now;
                }
                commandHandler.Send(platform, port);
                return;
            }
        commandHandler.Unknown(platform, port);
    }

    public async void Send(object context, Type port)
    {
        MethodInfo methodInfo = port.GetMethod("SendAsync") ?? throw new NotImplementedException($"Someone didn't implemenet the SendAsync method for the {port.Name} port");
        
        methodInfo.Invoke(null, new object[]{this.message.ToString(), context});

        if (Config.logAllCommands) await Logger.Log(this);
        
        this.lastReferenced = DateTime.Now;
        this.message.Clear();
    }

    public void Unknown(object platform, Type port)
    {
        message.Clear();
        message.Append(Format.BlockQuote($"Unknown Command, please use {Config.prefix}help for a list of available commands!"));
        Send(platform, port);
    } 
    
    // * Instance relevant things being here

    public CommandHandler(string username)
    {
        player = Playerdata.GetPlayerdata(username);
        lastReferenced = DateTime.Now;
        currentCommandMessage = null!;
        Load();
    }
    public static CommandHandler GetCommandHandler(string username)
    {
        if (!LoadedCommandHandlers.TryGetValue(username, out CommandHandler? command)) return new CommandHandler(username);
        if (command is null) throw new NullReferenceException("Something went wrong");
        command.lastReferenced = DateTime.Now;
        return command;
    }

    void Load()
    {
        LoadedCommandHandlers.Add(player.Username, this);
    }

    void Unload()
    {
        player.Save();
        LoadedCommandHandlers.Remove(this.player.Username);
    }

    // * Field declarations
    public string[] currentCommandMessage;
    public bool success = true;
    public readonly StringBuilder message = new();
    public readonly Playerdata player;
    protected DateTime lastReferenced;


    private static class AutoUnloader
    {
        static AutoUnloader()
        {
            AppDomain.CurrentDomain.ProcessExit += (sender, e) => UnloadAllCommands();
        }
        public static async void Start()
        {
            while (await new PeriodicTimer(TimeSpan.FromSeconds(Config.autoUnloadInterval)).WaitForNextTickAsync())
            {
                await UnloadAllCommands();
            }
        }
        public static Task UnloadAllCommands()
        {
            foreach (CommandHandler command in LoadedCommandHandlers.Values)
            {
                if (Config.forceUnload || DateTime.Now - command.lastReferenced >= TimeSpan.FromSeconds(Config.idleUnloadTime)) command.Unload();
            }
            return Task.CompletedTask;
        }
    }

    public static string MentionUser(ulong Id)
    {
        return $"<@{Id}>";
    }
}