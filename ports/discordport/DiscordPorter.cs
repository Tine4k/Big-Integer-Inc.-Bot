namespace PfannenkuchenBot.DiscordPort;
using PfannenkuchenBot.Commands;
using Discord;
using Discord.WebSocket;
public class DiscordPorter : IPorter
{
    static readonly string tokenpath = @"config/discordtoken.txt";
    public static readonly DiscordSocketClient client = new(new DiscordSocketConfig()
    {
        GatewayIntents =
        GatewayIntents.Guilds |
        GatewayIntents.GuildEmojis |
        GatewayIntents.GuildMessages |
        GatewayIntents.GuildMessageReactions |
        GatewayIntents.MessageContent
    });
    public static void StartUp()
    {
        client.Log += Logger.Log;
        client.MessageReceived += EvaluateCommand;
        if (!File.Exists(tokenpath)) throw new FileNotFoundException($"File {tokenpath} not found. Please provice a text file with a valid token.");
        client.LoginAsync(TokenType.Bot, File.ReadAllText(tokenpath));
        client.StartAsync();
    }

    static Task EvaluateCommand(SocketMessage _socketMessage)
    {
        if (_socketMessage is not SocketUserMessage socketMessage) return Task.CompletedTask;

        if (!(
        socketMessage.Content.StartsWith(Config.prefix)) ||
        socketMessage.Author.IsBot ||
        socketMessage.Content.Length <= Config.prefix.Length
        ) return Task.CompletedTask;

        string[] command_message = socketMessage.Content.Remove(0, Config.prefix.Length).Split(' ');

        CommandHandler.HandleCommand<DiscordPorter>(command_message, socketMessage.Author.Username, socketMessage.Channel);
        
        return Task.CompletedTask;
    }

    public static async Task Send(string message, object context)
    {
        if (context is not SocketTextChannel channel) throw new ArgumentException("Someone messed up with coding");
        await channel.SendMessageAsync(message);
    }
}