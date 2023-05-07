namespace PfannenkuchenBot;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
[JsonConverter(typeof(InventoryConverter))]
class Inventory
{
    public Inventory() : this(new Dictionary<Item, ulong>())
    { }
    [JsonConstructor]
    Inventory(Dictionary<Item, ulong> data)
    {
        this.contents = data;
    }
    public void Add(Item item, uint amount = 1)
    {
        if (contents.ContainsKey(item)) contents[item] += amount;
        else contents.Add(item, amount);
    }
    public void Add(string itemName, uint amount = 1)
    {
        if (!Item.Get(itemName, out Item? item)) throw new KeyNotFoundException();
        if (item is null) throw new NullReferenceException();
        this.Add(item, amount);
    }
    public void Add(Inventory items)
    {
        foreach (KeyValuePair<Item, ulong> pair in items) Add(pair.Key, (uint)pair.Value);
    }
    public bool Remove(Item item, uint amount = 1) // Returns true if player has enough Items;
    {
        if (!contents.ContainsKey(item)) return false;
        if (amount < contents[item])
        {
            contents[item] -= amount;
            return true;
        }
        else if (contents[item] == amount)
        {
            contents.Remove(item);
            return true;
        }
        else return false;
    }
    public bool Remove(Inventory items)
    {
        foreach (KeyValuePair<Item, ulong> pair in items)
        {
            if (!this.Remove(pair.Key, (uint)pair.Value)) return false;
        }
        return true;
    }
    public bool Remove(string itemName, uint amount = 1) // Returns true if player has enough Items;
    {
        if (!Item.Get(itemName, out Item? item)) throw new KeyNotFoundException();
        if (item is null) throw new NullReferenceException();
        return Remove(item, amount);
    }

    public bool Contains(Inventory items)
    {
        foreach (Item item in items.Keys) if (!contents.ContainsKey(item)) return false;
        return contents.Count() > 0;
    }
    public bool Transfer(Inventory targetInventory, Inventory items)
    {
        if (!this.Contains(items)) return false;
        targetInventory.Add(items);
        this.Remove(items);
        return true;
    }
    public void Clear() => contents.Clear();
    public string PrintContent()
    {
        StringBuilder message = new StringBuilder();
        foreach (KeyValuePair<Item, ulong> pair in contents) message.Append($"\n{pair.Value}x {pair.Key.Name}");
        return message.ToString();
    }
    public static readonly Inventory Empty = new Inventory(new Dictionary<Item, ulong>());
    [JsonPropertyName("Contents")]
    public Dictionary<Item, ulong> Contents => contents;
    readonly Dictionary<Item, ulong> contents = new Dictionary<Item, ulong>();
    // * Do not change, not relevant for game design
    public IEnumerator<KeyValuePair<Item, ulong>> GetEnumerator() => contents.GetEnumerator();
    Dictionary<Item, ulong>.KeyCollection Keys => contents.Keys;
    Dictionary<Item, ulong>.ValueCollection Values => contents.Values;

    class InventoryConverter : JsonConverter<Inventory>
    {
        public override Inventory Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject) throw new JsonException();
        

        var data = new Dictionary<Item, ulong>();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject) return new Inventory(data);
            
            if (reader.TokenType != JsonTokenType.PropertyName) throw new JsonException();

            string itemName = reader.GetString() ?? throw new NullReferenceException();

            if (!Item.Get(itemName, out Item? item)) throw new KeyNotFoundException($"Item with name '{itemName}' not found.");
            if (item is null) throw new NullReferenceException();
            
            if (!reader.Read()) throw new JsonException();
            if (reader.TokenType != JsonTokenType.Number) throw new JsonException();

            ulong itemQuantity = reader.GetUInt64();
            data[item] = itemQuantity;
        }

        throw new JsonException();
    }
        public override void Write(Utf8JsonWriter writer, Inventory inventory, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            foreach (KeyValuePair<Item, ulong> pair in inventory)
            {
                writer.WriteNumber(pair.Key.Name, pair.Value);
            }
            writer.WriteEndObject();
        }
    }
}