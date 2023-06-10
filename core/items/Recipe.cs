using System.Text.Json;
using System.Text.Json.Serialization;
using PfannenkuchenBot;

namespace Pfannenkuchenbot.Item;
class Recipe
{
    [JsonConstructor]
    public Recipe(Inventory materials, Inventory output)
    {
        Materials = materials;
        Output = output;
    }

    [JsonPropertyName("Materials")]
    public Inventory Materials
    { get; protected set; }

    [JsonPropertyName("Output")]
    public Inventory Output
    { get; protected set; }


    public static bool Craft(string id, Inventory inventory)
    {
        if (!GetRecipe(id, out Recipe recipe)) return false;
        return Craft(recipe, inventory);
    }

    public static bool Craft(Recipe recipe, Inventory inventory)
    {
        if (inventory.TryRemove(recipe.Materials))
        {
            inventory.Add(recipe.Output);
            return true;
        }
        else return false;
    }

    public static bool GetRecipe(string id, out Recipe recipe)
    {
        string recipeJson = File.ReadAllText($@"content\recipes\{id.ToLower()}.json");
        recipe = JsonSerializer.Deserialize<Recipe>(recipeJson)!;
        if (recipe is null) return false;
        return true;
    }
}