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
    public static async Task StartUp()
    {
        client.Log += Logger.Log;
        client.MessageReceived += EvaluateCommand;
        if (!File.Exists(tokenpath)) throw new FileNotFoundException($"File {tokenpath} not found. Please provice a text file with a valid token.");
        await client.LoginAsync(TokenType.Bot, File.ReadAllText(tokenpath));
        await client.StartAsync();
        await Task.Delay(-1);
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

        CommandHandler.HandleCommand(command_message, socketMessage.Author.Username, socketMessage.Channel, typeof(DiscordPorter));
        
        return Task.CompletedTask;
    }

    public static async Task SendAsync(string message, object context)
    {
        if (context is not SocketTextChannel channel) throw new ArgumentException("Someone messed up with coding");
        await channel.SendMessageAsync(message);
    }
}