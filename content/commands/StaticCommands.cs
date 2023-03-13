namespace PfannenkuchenBot;
using Discord;
using Discord.WebSocket;
partial class Command
{
    // * Any commands that don't require the reference of playerdata belong here
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
        channel.SendMessageAsync(
        Format.BlockQuote(Format.Bold(
        "Rauchst du an Jone, bist du da Mane!\nRauchst du kan, brauchst du an!\n") +
        "Kontaktieren Sie die Jone GmbH gerne unter " +
        Format.Italics(Format.Underline("+43 67689807553"))));
    }
}