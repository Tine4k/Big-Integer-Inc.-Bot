﻿namespace PfannenkuchenBot.Core;
using Discord;
using Discord.WebSocket;
static class Program
{
    public static void Main(string[] args)
    {
        
        var playerdata = Playerdata.GetPlayerdata("999999999999999999");
        playerdata.Gain("Gunpowder");
        Console.WriteLine("Finished!");
        return;
        if (args.Length > 0)
        {
            if (args[0] == "test")
            {
                Console.WriteLine("Test running...");
                var pd = Playerdata.GetPlayerdata("500953493918449674");
                pd.Gain("Gunpowder", 10);
            }
        }
        else 
        StartUp().GetAwaiter().GetResult();
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
    static async Task StartUp()
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
    public static Task Log(string msg)
    {
        Console.WriteLine($"{DateTime.Now.ToLongTimeString()} Custom      {msg}");
        return Task.CompletedTask;
    }
}