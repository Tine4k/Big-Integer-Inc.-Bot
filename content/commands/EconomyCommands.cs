using Discord.WebSocket;
using Pfannenkuchenbot.Item;

namespace PfannenkuchenBot.Commands;
partial class Command
{
    public void Balance()
    {
        message.Append($"Your current balance is {player.Balance}{Config.currency}!");
        
    }

    [Command(CommandCategory.Economy)]
    public void Buy(Item item, uint amount)
    {
        if (currentCommandMessage.Length < 2 || !Item.Get(currentCommandMessage[1], out item)) message.Append("Wasn't able to find the item you want.");
        else if (item.BuyPrice == 0) message.Append($"That item ain't up for sale. To see all items you can buy, try {Config.prefix}shop");
        else if (currentCommandMessage.Length == 2 && player.TryLose(item.BuyPrice))
        {
            player.Gain(item);
            message.Append($"You've bought 1x {item} for {item.BuyPrice}{Config.currency}.");
        }
        else if (currentCommandMessage.Length == 3 || !uint.TryParse(currentCommandMessage[2], out amount) || amount == 0)
        {
            message.Append("Bro you can't buy this amount of items. That's not a valid number.");
        }
        else if (player.TryLose(item, amount))
        {
            player.Gain(item.BuyPrice * amount);
            message.Append($"You've successfulyl bought {amount}x {item} for {item.BuyPrice * amount}{Config.currency}.");
        }
        else message.Append("You ain't got enough money to buy that item. Don't try to scam me!");
        
    }

    public void Sell()
    {
        Item item;
        if (currentCommandMessage.Length < 2 || !Item.Get(currentCommandMessage[1], out item)) message.Append("Wasn't able to find the item you wanted to sell.");
        else if (item.SellPrice == 0) message.Append("This item can not be sold.");
        else if (currentCommandMessage.Length == 2 && player.TryLose(item.Name))
        {
            player.Gain(item.SellPrice);
            message.Append($"You've sold 1x {item} for {item.SellPrice}{Config.currency}.");
        }
        else if (currentCommandMessage.Length == 3 && currentCommandMessage[2].Equals("all", StringComparison.OrdinalIgnoreCase) && player.Inventory.Contents.ContainsKey(item))
        {
            uint amount = (uint)player.Inventory.Contents[item];
            player.Gain(item.SellPrice*amount);
            player.ForceLose(item.Id, amount);
            message.Append($"You've sold {amount}x {item} for {item.SellPrice * amount}{Config.currency}.");
        }
        else if (currentCommandMessage.Length > 3 || !uint.TryParse(currentCommandMessage[2], out uint amount) || amount == 0)
        {
            message.Append("Bro that ain't a valid number.");
        }
        else if (player.TryLose(item.Id, amount))
        {
            player.Gain(item.SellPrice * amount);
            message.Append($"You've sold {amount}x {item} for {item.SellPrice * amount}{Config.currency}.");
        }
        else message.Append("You don't actually have this many items. Don't try to scam me!");
        
    }
    
    public void Shop()
    {
        foreach (Item item in GameElementLoader.loadedInstances[typeof(Item)])
        {
            if(item.BuyPrice != 0 && !(item.Tags.Contains("Illegal")))
            {
                message.Append($"{item.Name}: {item.BuyPrice}{Config.currency}\n\n");
            }            
        }
        
    }
    
    public void BlackMarket()
    {
        message.Append("**Welcome to the Black Market!\n**");
        foreach(Item item in GameElementLoader.loadedInstances[typeof(Item)])
        {
            if(item.BuyPrice != 0 && item.Tags.Contains("Illegal"))
            {
                message.Append($"{item.Name}: {item.BuyPrice}{Config.currency}\n\n");
            }
        }
        
    }
}