using Discord.WebSocket;
using Pfannenkuchenbot.Item;

namespace PfannenkuchenBot.Commands;
partial class Command
{
    public void Mine(SocketMessage socketmsg, string[] commandMessage)
    {
        Inventory items = new Inventory();
        items.Add("gunpowder", (uint)Random.Shared.Next(0, 2));
        items.Add("stone", (uint)Random.Shared.Next(1, 6));
        playerdata.Gain(items);
        message.Append($"You found:{items.PrintContent()}");
        Send(message, socketmsg, commandMessage);
    }

    public void PunchTree(SocketMessage socketmsg, string[] commandMessage)
    {
        if (Random.Shared.Next(3) == 1)
        {
            message.Append("Wow, you really took that litteral. Your fist is shattered, the tree is still standing.");
        }
        else
        {
            Inventory items = new Inventory();
            items.Add("wood", (uint)Random.Shared.Next(1, 2));
            items.Add("stick", (uint)Random.Shared.Next(2, 4));
            playerdata.Gain(items);
            message.Append($"The tree fell. From the distance, you hear the scream of Markus: *\"DU MÖRDER! BÄUME SIND AUCH MENSCHEN\"*.\nYou found:{items.PrintContent()}");
        }
        Send(message, socketmsg, commandMessage);
    }
}