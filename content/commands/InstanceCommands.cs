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

    public void Mane(SocketMessage socketmsg, string[] commandMessage)
    {
        playerdata.Gain("jone");
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
        Item item;
        if (commandMessage.Length < 2 || !Item.Get(commandMessage[1], out item)) message.Append("Wasn't able to find the item you want.");
        else if (item.BuyPrice == 0) message.Append($"That item ain't up for sale. To see all items you can buy, try {Config.prefix}shop");
        else if(commandMessage.Length == 2 && playerdata.TryLose(item.BuyPrice))
        {
            playerdata.Gain(item);
            message.Append($"You've bought 1x {item} for {item.BuyPrice}{Config.currency}.");
        }
        else if (commandMessage.Length == 3 || !uint.TryParse(commandMessage[2], out uint amount) || amount == 0)
        {
            message.Append("Bro you can't buy this amount of items. That's not a valid number.");
        }
        else if (playerdata.TryLose(item, amount))
        {
            playerdata.Gain(item.BuyPrice * amount);
            message.Append($"You've successfulyl bought {amount}x {item} for {item.BuyPrice * amount}{Config.currency}.");
        }
        else message.Append("You ain't got enough money to buy that item. Don't try to scam me!");
        Send(message, socketmsg, commandMessage);
    }

    public void Sell(SocketMessage socketmsg, string[] commandMessage)
    {
        Item item;
        if (commandMessage.Length < 2 || !Item.Get(commandMessage[1], out item)) message.Append("Wasn't able to find the item you wanted to sell.");
        else if (item.SellPrice == 0) message.Append("This item can not be sold.");
        else if(commandMessage.Length == 2 && playerdata.TryLose(item.Name))
        {
            playerdata.Gain(item.SellPrice);            
            message.Append($"You've sold 1x {item} for {item.SellPrice}{Config.currency}.");
        }
        else if (commandMessage.Length == 3 || !uint.TryParse(commandMessage[2], out uint amount) || amount == 0)
        {
            message.Append("Bro that ain't a valid number.");
        }
        else if (playerdata.TryLose(item.Name, amount))
        {
            playerdata.Gain(item.SellPrice * amount);          
            message.Append($"You've sold {amount}x {item} for {item.SellPrice * amount}{Config.currency}.");
        }
        else message.Append("You don't actually have this many items. Don't try to scam me!");
        Send(message, socketmsg, commandMessage);
    }
}