namespace PfannenkuchenBot;
abstract class GameObject
{
    public static string name;
    public static string description;
    public static string Describe()
    {
        return $"Name: {name}\nDescription: {description}";
    }
    
}