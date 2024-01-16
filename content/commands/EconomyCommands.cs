using Discord.WebSocket;
using Pfannenkuchenbot.Item;

namespace PfannenkuchenBot.Commands;
public partial class CommandHandler
{
    [Command(CommandCategory.Economy)]
    public void Balance()
    {
        message.Append($"Your current balance is {player.Balance}{Config.currency}!");
        
    }

    [Command(CommandCategory.Economy)]
    public void Buy(Item item, uint amount)
    {
        if (item is null) message.Append("Wasn't able to find the item you want.");
        else if (item.BuyPrice == 0) message.Append($"That item ain't up for sale. To see all items you can buy, try {Config.prefix}shop");
        else if (amount == 0 && player.TryLose(item.BuyPrice))
        {
            player.Gain(item);
            message.Append($"You've bought 1x {item} for {item.BuyPrice}{Config.currency}.");
        }
        else if (player.TryLose(amount))
        {
            player.Gain(item, amount);
            message.Append($"You've successfulyl bought {amount}x {item} for {item.BuyPrice * amount}{Config.currency}.");
        }
        else message.Append("You ain't got enough money to buy that item. Don't try to scam me!");
    }

    [Command(CommandCategory.Economy)]
    public void Sell(Item item, uint amount)
    {
        if (item is null) message.Append("Wasn't able to find the item you wanted to sell.");
        else if (item.SellPrice == 0) message.Append("This item can not be sold.");
        else if (amount == 0 || player.TryLose(item))
        {
            player.Gain(item.SellPrice);
            message.Append($"You've sold 1x {item} for {item.SellPrice}{Config.currency}.");
        }
        else if (player.TryLose(item, amount))
        {
            player.Gain(item.SellPrice*amount);
            message.Append($"You've sold {amount}x {item} for {item.SellPrice * amount}{Config.currency}.");
        }
        else message.Append("You don't actually have this many items. Don't try to scam me!");
    }
    [Command(CommandCategory.Economy)]
    public void Sell(Item item)
    {
        Sell(item, 0);
    }
    public void Sell(Item item, string text)
    {
        if (!text.Equals("all", StringComparison.OrdinalIgnoreCase)) message.Append("This is just invalid and does'nt work this way!");
        else if (player.Has(item)) 
        {
            ulong amount = player.Inventory.Contents[item];
            Sell(item, (uint)amount);
        }
    }
    
    [Command(CommandCategory.Economy)]
    public void Shop()
    {
        if (!GameElementLoader.loadedInstances.TryGetValue(typeof(Item), out List<GameElement>? items))
        {
            message.Append($"The shop can't be accessed right now, try again in a minute");
            return;
        }
        foreach (Item item in items.Cast<Item>().ToArray())
        {
            if(item.BuyPrice != 0 && !item.Tags.Contains("Illegal"))
            {
                message.AppendLine($"{item.Name}: {item.BuyPrice}{Config.currency}\n");
            }            
        }
    }
    
    [Command(CommandCategory.Economy)]
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
    [Command(CommandCategory.Economy)]
    public void Catalogue()
    {
        foreach (Item item in GameElementLoader.loadedInstances[typeof(Item)])
        {
            if(item.SellPrice != 0 && !item.Tags.Contains("Illegal"))
            {
                message.Append($"{item.Name}: {item.BuyPrice}{Config.currency}\n\n");
            }            
        }
    }
}