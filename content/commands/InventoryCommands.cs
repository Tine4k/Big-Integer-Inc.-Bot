using Pfannenkuchenbot.Item;

namespace PfannenkuchenBot.Commands;
public partial class CommandHandler
{
    [Command(CommandCategory.Inventory)]
    public void Inventory()
    {
        message.Append((player.Inventory.Count > 0) ? $"**Inventory of {player.Username}:**" : "It appears as if your inventory was empty...");
        message.Append(player.PrintContent());
        
    }
    
    [Command(CommandCategory.Inventory)]
    public void Craft()
    {
        if (currentCommandMessage.Length == 2 && Recipe.GetRecipe(currentCommandMessage[1], out Recipe recipe) && Recipe.Craft(recipe, player.Inventory))
        {
            message.Append($"You successfully crafted {recipe.Output.PrintContent()}");
        }
        else message.Append("Either this recipe doesn't exist or you didn't have all the materials required for crafting this item.");
        
    }
}