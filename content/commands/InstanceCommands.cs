using Discord;
using Discord.WebSocket;
using PfannenkuchenBot.Core;
namespace PfannenkuchenBot;
class InstanceCommand: Command
{
    // * All commands that reference playerdata belong here
    public InstanceCommand(SocketMessage _socketmsg, string[] _command): base(_socketmsg, _command)
    {
        this.instanceHandler = InstanceHandler.GetInstanceHandler(_socketmsg.Author.Id.ToString());
        this.player = instanceHandler.playerdata;
    }
    protected override void Send()
    {
        base.Send();
        instanceHandler.lastReferenced = DateTime.Now;
    }
    readonly Playerdata player;
    readonly InstanceHandler instanceHandler;
    public void Give()
    {
        uint amount;
        if (
            command.Length >= 2 &&
            uint.TryParse(command[2], out amount)
            ) player.Gain(command[1], amount = 1);
        else Unknown();
    }
    public void Balance()
    {
        message.Append($"Your current balance is {player.Balance}{Config.currency}!");
        Send();
    }
    public void Inventory()
    {
        message.Append(Format.Bold($"Inventory of {author.Username}:"));
        message.Append(player.PrintInventory());
        Send();
    }
    public void Daily()
    {
        player.Gain(1000);
        message.Append($"Added 1000{Config.currency} to your balance, which now contains {player.Balance}{Config.currency}");
        Send();
    }
    public void Mine()
    {
        Inventory items = new Inventory();
        items.Add("Gunpowder", (uint)Random.Shared.Next(1,4));
        player.Gain(items);
        message.Append($"You found:{items.PrintContent()}");
        Send();
    }
    public void Mane()
    {
        player.Gain("Jone");
        message.Append("@Klagenfurt Busbahnhof");
        Send();
    }
}