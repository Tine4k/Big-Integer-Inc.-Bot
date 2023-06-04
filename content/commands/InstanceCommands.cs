using Discord;
using Discord.WebSocket;
namespace PfannenkuchenBot.Commands;
partial class Command
{
    public void Give(SocketMessage socketmsg, string[] commandMessage)
    {
        uint amount;
        if (
            commandMessage.Length >= 2 &&
            uint.TryParse(commandMessage[2], out amount)
            ) playerdata.Gain(commandMessage[1], amount = 1);
        else Unknown(socketmsg.Channel);
    }
    public void Balance(SocketMessage socketmsg, string[] commandMessage)
    {
        message.Append($"Your current balance is {playerdata.Balance}{Config.currency}!");
        Send(message, socketmsg, commandMessage);
    }
    public void Inventory(SocketMessage socketmsg, string[] commandMessage)
    {
        message.Append(Format.Bold($"Inventory of {socketmsg.Author.Username}:"));
        message.Append(playerdata.PrintInventory());
        Send(message, socketmsg, commandMessage);
    }
    public void Daily(SocketMessage socketmsg, string[] commandMessage)
    {
        playerdata.Gain(1000);
        message.Append($"Added 1000{Config.currency} to your balance, which now contains {playerdata.Balance}{Config.currency}");
        Send(message, socketmsg, commandMessage);
    }
    public void Mine(SocketMessage socketmsg, string[] commandMessage)
    {
        Inventory items = new Inventory();
        items.Add("Gunpowder", (uint)Random.Shared.Next(0,2));
        items.Add("Stone", (uint)Random.Shared.Next(1,6));
        playerdata.Gain(items);
        message.Append($"You found:{items.PrintContent()}");
        Send(message, socketmsg, commandMessage);
    }    
    public void Mane(SocketMessage socketmsg, string[] commandMessage)
    {
        playerdata.Gain("Jone");
        message.Append("@Klagenfurt Busbahnhof");
        Send(message, socketmsg, commandMessage);
    }
}