using System.Diagnostics.Contracts;
using PfannenkuchenBot;

namespace Pfannenkuchenbot.Item;
class Recipe
{
    public Recipe(Inventory _materials, Inventory _output)
    {
        materials = _materials;
        output = _output;
    }
    public readonly Inventory materials;
    public readonly Inventory output; 

}