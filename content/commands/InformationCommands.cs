using Discord.WebSocket;
namespace PfannenkuchenBot.Commands;
public partial class CommandHandler
{
    [Command(CommandCategory.Information)]
    public void Info()
    {
        if (currentCommandMessage.Length == 2 && Item.Get(currentCommandMessage[1], out Item? item)) {
            if (item is null) throw new InvalidGameObjectException();
            message.Append(item.Describe());
        }
        else message.Append("Couldn't find the item with the id you specified, please check the spelling!");
        
    }
    
}