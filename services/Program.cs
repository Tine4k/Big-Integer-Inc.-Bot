﻿namespace PfannenkuchenBot.Core;
using Discord;
using Discord.WebSocket;
static class Program
{
    public static void Main(string[] args)
    {
        // if (args.Length > 0)
        // {
        //     if (args[0] == "restore")
        //     {
        //         Console.WriteLine("Restoring all files!");
        //         Restorer.Restore();
        //     }
        // }
        // else 
        Program.Startup().GetAwaiter().GetResult();
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
        // new Restorer().Restore();
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
}