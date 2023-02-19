namespace PfannenkuchenBot;
using Discord;
using Discord.WebSocket;
partial class Command
{
    public static void Unknown(ISocketMessageChannel channel)
    {
        channel.SendMessageAsync($"Unknown Command, please use {Config.prefix}help for a list of available commands!");
    }
    public static void Help(ISocketMessageChannel channel)
    {
        string msg = Format.Bold("List of all available commands: (Not available yet)");
        channel.SendMessageAsync(msg);
    }
    public static void HelloWorld(ISocketMessageChannel channel)
    {
        channel.SendMessageAsync("Hello there");
    }
    public static void Jone(ISocketMessageChannel channel)
    {
        channel.SendMessageAsync("Rauchst du an Jone, bist du da Mane");
    }
}