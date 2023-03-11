﻿namespace PfannenkuchenBot.Core;
using Discord;
using Discord.WebSocket;
static class Program
{
    public static void Main(string[] args)
    {
        if (args.Length > 0)
        {
            if (args[0] == "debug")
            {
                Console.WriteLine("Debugging");
                Program.DebugStartup().GetAwaiter().GetResult();
            }
            else if (args[0] == "test")
            {
                Test();
            }
        }
        else Program.Startup().GetAwaiter().GetResult();
    }
    public static DiscordSocketClient client = new DiscordSocketClient(new DiscordSocketConfig()
    {
        GatewayIntents =
        GatewayIntents.Guilds |
        GatewayIntents.GuildEmojis |
        GatewayIntents.GuildMessages |
        GatewayIntents.GuildMessageReactions |
        GatewayIntents.MessageContent
    });
    static async Task Startup()
    {
        client.Log += Log;
        Program.client.MessageReceived += CommandHandler.HandleCommand;
        await client.LoginAsync(TokenType.Bot, File.ReadAllText("config/token.txt"));
        await client.StartAsync();
        await Task.Delay(-1);
    }
    static Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
    static async Task DebugStartup()
    {
        client.Log += Log;
        Program.client.MessageReceived += CommandHandler.HandleCommand;
        await client.LoginAsync(TokenType.Bot, File.ReadAllText("config/token.txt"));
        await client.StartAsync();
        // debug here
        await Task.Delay(-1);
    }
    static void Test()
    {
        
    }
}