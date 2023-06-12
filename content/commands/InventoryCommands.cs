using Discord.WebSocket;
using Pfannenkuchenbot.Item;

namespace PfannenkuchenBot.Commands;
partial class Command
{
    public void Inventory(SocketMessage socketmsg, string[] commandMessage)
    {
        message.Append($"**Inventory of {socketmsg.Author.Username}:**");
        message.Append(playerdata.PrintInventory());
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