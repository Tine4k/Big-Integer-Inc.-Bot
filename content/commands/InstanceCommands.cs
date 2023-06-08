using Discord;
using Discord.WebSocket;
using Pfannenkuchenbot.Item;

namespace PfannenkuchenBot.Commands;
partial class Command
{
    public void Give(SocketMessage socketmsg, string[] commandMessage)
    {
        if (commandMessage.Length == 2) playerdata.Gain(commandMessage[1]);
        else if (
            commandMessage.Length >= 3 &&
            uint.TryParse(commandMessage[2], out uint amount)
            ) playerdata.Gain(commandMessage[1], amount);
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
        if (!playerdata.Timestamps.Contains ("lastDaily" )Playerdata.Timestamps.Add("lastDaily" DateTime.default);
        if (DateTime.Now - Playerdata.Timestamps["lastDaily"] < TiemSpan.FromHours(24))
        {
            message.Append($"Wait *{DateTime.Now - Playerdata.Timestamps["lastDaily"]}* longer to be get your next Daily reward!");
                Send(message, socketmsg, commandMessage);
                return

        }
        playerdata.Gain(1000);
        message.Append($"Added 1000{Config.currency} to your balance, which now contains {playerdata.Balance}{Config.currency}");
        Send(message, socketmsg, commandMessage);
    }
    public void Mine(SocketMessage socketmsg, string[] commandMessage)
    {
        Inventory items = new Inventory();
        items.Add("Gunpowder", (uint)Random.Shared.Next(0, 2));
        items.Add("Stone", (uint)Random.Shared.Next(1, 6));
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
            items.Add("Wood", (uint)Random.Shared.Next(1, 2));
            items.Add("Stick", (uint)Random.Shared.Next(2, 4));
            playerdata.Gain(items);
            message.Append($"The tree fell. From the distance, you hear the scream of Markus: *\"DU MÖRDER! BÄUME SIND AUCH MENSCHEN\"*.\nYou found:{items.PrintContent()}");
        }
        Send(message, socketmsg, commandMessage);
    }
    public void Mane(SocketMessage socketmsg, string[] commandMessage)
    {
        playerdata.Gain("Jone");
        message.Append("@Klagenfurt Busbahnhof");
        Send(message, socketmsg, commandMessage);
    }

    public void Craft(SocketMessage socketmsg, string[] commandMessage)
    {
        if (commandMessage.Length == 2 && Recipe.GetRecipe(commandMessage[1], out Recipe recipe) && Recipe.Craft(recipe, playerdata.Inventory))
        {
            message.Append($"You successfully crafted {recipe.Output.PrintContent()}");
        }
        else message.Append("Either this recipe doesn't exist or you didn't have all the materials required for crafting this item.");
        Send(message, socketmsg, commandMessage);
    }
}