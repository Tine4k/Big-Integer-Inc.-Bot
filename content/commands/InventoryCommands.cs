using Pfannenkuchenbot.Item;

namespace PfannenkuchenBot.Commands;
partial class Command
{
    public void Inventory()
    {
        message.Append((player.Inventory.Count > 0) ? $"**Inventory of {currentSocketMessage.Author.Username}:**" : "It appears as if you inventory was empty...");
        message.Append(player.PrintContent());
        
    }

    public void Craft()
    {
        if (currentCommandMessage.Length == 2 && Recipe.GetRecipe(currentCommandMessage[1], out Recipe recipe) && Recipe.Craft(recipe, player.Inventory))
        {
            message.Append($"You successfully crafted {recipe.Output.PrintContent()}");
        }
        else message.Append("Either this recipe doesn't exist or you didn't have all the materials required for crafting this item.");
        
    }
}