using Discord.WebSocket;
using Pfannenkuchenbot.Item;

namespace PfannenkuchenBot.Commands;
partial class Command
{
    public void Balance(SocketMessage socketmsg, string[] commandMessage)
    {
        message.Append($"Your current balance is {playerdata.Balance}{Config.currency}!");
        Send(message, socketmsg, commandMessage);
    }

    public void Buy(SocketMessage socketmsg, string[] commandMessage)
    {
        Item item;
        if (commandMessage.Length < 2 || !Item.Get(commandMessage[1], out item)) message.Append("Wasn't able to find the item you want.");
        else if (item.BuyPrice == 0) message.Append($"That item ain't up for sale. To see all items you can buy, try {Config.prefix}shop");
        else if (commandMessage.Length == 2 && playerdata.TryLose(item.BuyPrice))
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
        else if (commandMessage.Length == 2 && playerdata.TryLose(item.Name))
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
    
    public void Shop(SocketMessage socketmsg, string[] commandMessage)
    {
        foreach(Item item in GameElement.ItemLoader.loadedInstances.Values)
        {
            if(item.BuyPrice != 0 && !(item.Tags.Contains("Illegal")))
            {
                message.Append($"{item.Name}: {item.BuyPrice}{Config.currency}\n\n");
            }            
        }
        Send(message, socketmsg, commandMessage);
    }
    
    public void BlackMarket(SocketMessage socketmsg, string[] commandMessage)
    {
        message.Append("**Welcome to the Black Market!\n**");
        foreach(Item item in GameElement.ItemLoader.loadedInstances.Values)
        {
            if(item.BuyPrice != 0 && item.Tags.Contains("Illegal"))
            {
                message.Append($"{item.Name}: {item.BuyPrice}{Config.currency}\n\n");
            }
        }
        Send(message, socketmsg, commandMessage);
    }
}