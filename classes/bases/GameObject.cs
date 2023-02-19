namespace PfannenkuchenBot;
abstract class GameObject
{
    public readonly string name; 
    public readonly string description;
    public virtual string Describe()
    {
        return $"Name: {name}\nDescription: {description}";
    }
    
}