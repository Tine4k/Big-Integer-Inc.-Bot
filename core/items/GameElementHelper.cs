using System.Text.Json;
using System.Text.Json.Serialization;

namespace PfannenkuchenBot;
class GameElementHelper<T> : GameElementHelperBase where T : GameElement
{
    static GameElementHelper()
    {
        helpers = new Dictionary<Type, GameElementHelperBase>();
    }
    public GameElementHelper() : base()
    {
        Dictionary<int, string> mydic=  new Dictionary<int, string>();
        mydic.TryGetValue()
        helpers.Add(GetType(), this);
        loadedInstances = new Dictionary<string, T>();
        directory = $"content/{typeof(T).Name}";
        LoadAll();
    }
    public bool Get(string name, out T? gameElement)
    {
        bool successfull = loadedInstances.TryGetValue(name, out gameElement);
        if (gameElement is null) throw new KeyNotFoundException($"Invalid {typeof(T).Name} detected at {name}");
        return successfull;
    }
    public void Reload()
    {
        Reset();
        LoadAll();
    }
    void Reset() => loadedInstances = new Dictionary<string, T>();
    void LoadAll()
    {
        foreach (string Path in Directory.GetFiles(directory, "*json", SearchOption.AllDirectories)) Load(Path);
    }
    void Load(string path)
    {
        T? gameElement = JsonSerializer.Deserialize<T>(File.ReadAllText(path));
        if (gameElement == null) throw new JsonException($"Tried to load invalid GameElement from file {path}");
        loadedInstances.Add(gameElement.Name, gameElement);
    }
    Dictionary<string, T> loadedInstances;
    readonly string directory;
    static public Dictionary<Type, GameElementHelperBase> helpers;

    public class GameElementConverter<T2> : JsonConverter<T> where T2 : GameElement
    {
        public GameElementConverter()
        {
            this.helper = GameElementHelper<T2>.helpers[typeof(T)] as GameElementHelper<T>;
            if (helper is null) throw new Exception("Ich hab keine Ahnung was hier passiert");
        }
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string name = reader.GetString() ?? throw new JsonException("Someone messed up creating items...");
            return GetGameElement(name);

            T GetGameElement(string name)
            {
                if (!helper.Get(name, out T? gameElement)) throw new InvalidGameObjectException(name);
                return gameElement ?? throw new InvalidGameObjectException(name);
            }
        }


        public override void Write(Utf8JsonWriter utf8JsonWriter, T t, JsonSerializerOptions jsonSerializerOptions)
        {
            throw new NotImplementedException("I have absolutely no idea how this method got called, best wishes, Markus");
        }
        GameElementHelper<T> helper;
    }
}
abstract class GameElementHelperBase
{
    public GameElementHelperBase()
    {
        jsonConverter = GetType();
    }
    public Type jsonConverter;
}