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

    /* public void Daily(SocketMessage socketmsg, string[] commandMessage)
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
    } */

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

    public void Buy(SocketMessage socketmsg, string[] commandMessage)
    {
        uint Price;
        if(commandMessage.Length == 2 && Item.Get(commandMessage[1], out Item? item) && item.Price != 0 && playerdata.Lose(item.Price))
        {
            Price = item.Price;
            playerdata.Lose(Price);
            playerdata.Gain(item);
            message.Append($"You've bought 1 {item} for {Price}$.");
        } 
        else if(commandMessage.Length == 3 && Item.Get(commandMessage[1], out item) && item.Price != 0 && playerdata.Lose(item.Price))
        {
            Price = item.Price * Convert.ToUInt32(commandMessage[2]);
            playerdata.Lose(Price);
            playerdata.Gain(item, Convert.ToUInt32(commandMessage[2]));
            message.Append($"You've bought {commandMessage[2]} {item} for {Price}$.");
        }
        else message.Append("This item isn't buyable or doesn't exist.");
        Send(message, socketmsg, commandMessage);
    }

    public void Sell(SocketMessage socketmsg, string[] commandMessage)
    {
        uint sellPrice;
        if(commandMessage.Length == 2 && Item.Get(commandMessage[1], out Item? item) && item.Price != 0 && playerdata.Lose(item.Name, 1))
        {
            sellPrice = item.Price / 2;
            playerdata.Gain(sellPrice);            
            message.Append($"You've sold 1 {item} for {sellPrice}$.");
        } 
        else if (commandMessage.Length == 3 && Item.Get(commandMessage[1], out item) && item.Price != 0 && playerdata.Lose(item.Name, Convert.ToUInt32(commandMessage[2])))
        {
            sellPrice = (item.Price / 2) * Convert.ToUInt32(commandMessage[2]);
            playerdata.Gain(sellPrice);            
            message.Append($"You've sold {commandMessage[2]} {item} for {sellPrice}$.");
        }
        else message.Append("This item isn't sellable or doesn't exist.");
        Send(message, socketmsg, commandMessage);
    }
}