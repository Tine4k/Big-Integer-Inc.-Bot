using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PfannenkuchenBot;
static class GameElementLoader
{
    static GameElementLoader()
    {
        loadedInstances = new Dictionary<Type, List<GameElement>>();
    }
    public static bool Get<T>(string id, out T returnElement) where T : GameElement, new()
    {
        if (!loadedInstances.TryGetValue(typeof(T), out List<GameElement>? list))
        {
            Reload<T>();
            list = loadedInstances[typeof(T)];
        }
        foreach (GameElement gameElement in list) if (gameElement.Id.Equals(id, StringComparison.OrdinalIgnoreCase))
            {
                returnElement = (T)gameElement;
                return true;
            }
        returnElement = null!;
        return false;
    }
    public static void Reload<T>() where T : GameElement, new()
    {
        if (loadedInstances.ContainsKey(typeof(T))) Reset<T>();
        LoadAll<T>();
    }
    static void Reset() => loadedInstances.Clear();
    static void Reset<T>() where T : GameElement, new() => loadedInstances[typeof(T)].Clear();
    static void LoadAll<T>() where T : GameElement, new()
    {
        foreach (string path in Directory.GetFiles($@"content\{typeof(T).Name.ToLower()}", "*json", SearchOption.AllDirectories)) Load<T>(path);
    }
    static void Load<T>(string path) where T : GameElement, new()
    {
        T? gameElement = JsonSerializer.Deserialize<T>(File.ReadAllText(path), new JsonSerializerOptions() { Converters = { new GameElementConverter<T>() } });
        if (gameElement is null) throw new JsonException($"Tried to load invalid {nameof(PfannenkuchenBot.Item)} from file {path}");
        if (!loadedInstances.TryGetValue(typeof(T), out List<GameElement>? list)) loadedInstances.Add(typeof(T), new List<GameElement>() { gameElement });
        else list.Add(gameElement);
    }
    public static readonly Dictionary<Type, List<GameElement>> loadedInstances;



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
                        if (property.PropertyType == typeof(String)) property.SetValue(gameElement, reader.GetString());
                        else
                        {
                            object? value = JsonSerializer.Deserialize(ref reader, property.PropertyType, options);
                            if (value is null && Nullable.GetUnderlyingType(property.PropertyType) is null)
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
