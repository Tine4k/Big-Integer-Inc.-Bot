using System.Text;
using Discord;
using Discord.WebSocket;
using PfannenkuchenBot.Core;
namespace PfannenkuchenBot;
partial class Command
{
    // * All commands that reference playerdata belong here
    public Command(SocketMessage _socketmsg, string[] _command)
    {
        this.channel = _socketmsg.Channel;
        this.author = _socketmsg.Author;
        this.instanceHandler = InstanceHandler.GetInstanceHandler(_socketmsg.Author.Id.ToString());
        this.player = instanceHandler.playerdata;
        this.command = _command;
    }
    readonly Playerdata player;
    readonly InstanceHandler instanceHandler;
    readonly ISocketMessageChannel channel;
    readonly SocketUser author;
    readonly string[] command;
    public void Give()
    {
        uint amount;
        if (
            command.Length >= 2 &&
            uint.TryParse(command[2], out amount)
            ) player.Gain(command[1], amount = 1);
        else Command.Unknown(channel);
    }
    public void Balance()
    {
        channel.SendMessageAsync($"Your current balance is {player.Balance}$");
    }
    public void Inventory()
    {
        channel.SendMessageAsync(Format.BlockQuote(Format.Bold($"Inventory of {author.Username}:\n") + player.PrintContent()));
    }
    public void Daily()
    {
        player.Gain(1000);
        channel.SendMessageAsync($"Added 1000$ to your balance, which now contains {player.Balance}$");
    }
    public void Mine()
    {
        Inventory items = new Inventory();
        items.Add("Gunpowder", (uint)Random.Shared.Next(4));
        channel.SendMessageAsync($"You found {items.PrintContent()}!");
    }
}