namespace PfannenkuchenBot;
abstract class GameObject
{
    public string Name
    { get; private set; }
    public string Description
    { get; private set; }
    public virtual string Describe()
    {
        return $"Name: {Name}\nDescription: {Description}";
    }
}