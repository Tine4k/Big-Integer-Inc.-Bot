using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PfannenkuchenBot;
static class GameElementLoader<T> where T : GameElement, new()
{
    static GameElementLoader()
    {
        loadedInstances = new Dictionary<string, T>();
        directory = $@"content\{typeof(T).Name}";
        LoadAll();
    }
    public static bool Get(string id, out T gameElement)
    {
        if (!loadedInstances.TryGetValue(id, out gameElement!)) return false;
        if (gameElement is null) throw new KeyNotFoundException($"Invalid {nameof(GameElement)} detected at {id}");
        return true;
    }
    public static void Reload()
    {
        Reset();
        LoadAll();
    }
    static void Reset() => loadedInstances.Clear();
    static void LoadAll()
    {
        foreach (string path in Directory.GetFiles(directory, "*json", SearchOption.AllDirectories)) Load(path);
    }
    static void Load(string path)
    {
        T? item = JsonSerializer.Deserialize<T>(File.ReadAllText(path), jsonSerializerOptions);
        if (item == null) throw new JsonException($"Tried to load invalid {nameof(PfannenkuchenBot.Item)} from file {path}");
        loadedInstances.Add(item.Id, item);
    }
    public static readonly Dictionary<string, T> loadedInstances;
    static readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions() { Converters = { new GameElementConverter<T>() } };
    static readonly string directory;

    public class GameElementConverter<T2> : JsonConverter<T2> where T2 : GameElement, new()
    {
        readonly PropertyInfo[] properties = typeof(T2).GetProperties();
        public override T2 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject) throw new JsonException();
            T2 gameElement = new T2();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject) return gameElement;
                string propertyName = reader.GetString() ?? throw new NullReferenceException();
                if (!reader.Read()) throw new JsonException("Unexpected end of JSON object");
                foreach (PropertyInfo property in properties)
                {
                    if (property.Name == propertyName)
                    {
                        if (property.PropertyType == typeof(string)) property.SetValue(gameElement, reader.GetString());
                        else
                        {
                            object? value = JsonSerializer.Deserialize(ref reader, property.PropertyType, options);
                            if (value == null && Nullable.GetUnderlyingType(property.PropertyType) == null)
                            {
                                throw new InvalidOperationException($"Cannot convert null value to non-nullable type {property.PropertyType.Name}.");
                            }
                            property.SetValue(gameElement, value);
                        }
                    }
                }
            }
            throw new JsonException("Unexpected end of JSON object");
        }

        public override void Write(Utf8JsonWriter writer, T2 value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
