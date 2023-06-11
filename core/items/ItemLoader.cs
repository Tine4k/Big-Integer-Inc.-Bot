using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PfannenkuchenBot;

abstract partial class GameElement
{
    public static class ItemLoader
    {
        static ItemLoader()
        {
            loadedInstances = new Dictionary<string, Item>();
            directory = $@"content\{nameof(Item)}";
            LoadAll();
        }
        public static bool Get(string id, out Item item)
        {
            bool successfull = loadedInstances.TryGetValue(id, out item!);
            if (item is null) throw new KeyNotFoundException($"Invalid {nameof(GameElement)} detected at {id}");
            return successfull;
        }
        public static void Reload()
        {
            Reset();
            LoadAll();
        }
        static ItemConverter converter = new ItemConverter();
        static void Reset() => loadedInstances = new Dictionary<string, Item>();
        static void LoadAll()
        {
            foreach (string path in Directory.GetFiles(directory, "*json", SearchOption.AllDirectories)) Load(path);
        }
        static void Load(string path)
        {
            Item? item = JsonSerializer.Deserialize<Item>(File.ReadAllText(path), new JsonSerializerOptions { Converters = { converter } });
            if (item == null) throw new JsonException($"Tried to load invalid {nameof(PfannenkuchenBot.Item)} from file {path}");
            loadedInstances.Add(item.Name, item);
        }
        public static Dictionary<string, Item> loadedInstances;
        static readonly string directory;

        public class ItemConverter : JsonConverter<Item>
        {
            readonly PropertyInfo[] properties = typeof(Item).GetProperties();
            public override Item Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType != JsonTokenType.StartObject) throw new JsonException();
                Item item = new Item();
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject) return item;
                    string propertyName = reader.GetString() ?? throw new NullReferenceException();
                    if (!reader.Read()) throw new JsonException("Unexpected end of JSON object");
                    foreach (PropertyInfo property in properties)
                    {
                        if (property.Name == propertyName)
                        {
                            if (property.PropertyType == typeof(string)) property.SetValue(item, reader.GetString());
                            else
                            {
                                object? value = JsonSerializer.Deserialize(ref reader, property.PropertyType, options);
                                if (value == null && Nullable.GetUnderlyingType(property.PropertyType) == null)
                                {
                                    throw new InvalidOperationException($"Cannot convert null value to non-nullable type {property.PropertyType.Name}.");
                                }
                                property.SetValue(item, value);
                            }
                        }
                    }                        
                }
                throw new JsonException("Unexpected end of JSON object");
            }
            public override void Write(Utf8JsonWriter writer, Item value, JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }
        }
    }
}
