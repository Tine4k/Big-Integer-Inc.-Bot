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
        this.content = data;
    }
    public void Add(Item item, uint amount = 1)
    {
        if (content.ContainsKey(item)) content[item] += amount;
        else content.Add(item, amount);
    }
    public void Add(string id, uint amount = 1)
    {
        if (!Item.Get(id, out Item? item)) throw new InvalidGameObjectException();
        if (item is null) throw new NullReferenceException();
        this.Add(item, amount);
    }
    public void Add(Inventory items)
    {
        foreach (KeyValuePair<Item, ulong> pair in items) this.Add(pair.Key, (uint)pair.Value);
    }

    public bool TryRemove(Item item, uint amount = 1) // Returns true if player has enough Items and removes amount
    {
        if (!content.ContainsKey(item)) return false;
        if (amount < content[item])
        {
            content[item] -= amount;
            return true;
        }
        else if (amount == content[item])
        {
            content.Remove(item);
            return true;
        }
        else return false;
    }
    public bool TryRemove(string id, uint amount = 1)
    {
        if (!Item.Get(id, out Item? item)) throw new InvalidGameObjectException();
        if (item is null) throw new NullReferenceException();
        return TryRemove(item, amount);
    }
    public bool TryRemove(Inventory items)
    {
        if (!this.Contains(items)) return false;
        this.Remove(items);
        return true;
    }

    public void Remove(Item item, uint amount = 1) // Removes amount even if player doesn't have enough Items 
    {
        if (!content.ContainsKey(item)) return;
        if (amount < content[item])
        {
            content[item] -= amount;
        }
        else if (amount >= content[item])
        {
            content.Remove(item);
        }
    }
    public void Remove(string id, uint amount = 1)
    {
        if (!Item.Get(id, out Item? item)) throw new InvalidGameObjectException();
        if (item is null) throw new NullReferenceException();
        this.Remove(item, amount);
    }
    public void Remove(Inventory items)
    {
        foreach (KeyValuePair<Item, ulong> pair in items) this.Remove(pair.Key, (uint)pair.Value);
    }

    public bool Contains(Item item, ulong amount = 1)
    {
        if (!content.ContainsKey(item)) return false;
        if (content[item] < amount) return false;
        return true;
    }
    public bool Contains(Inventory items)
    {
        foreach (KeyValuePair<Item, ulong> pair in items)
        {
            if (!this.Contains(pair.Key, pair.Value)) return false;
        }
        return true;
    }

    public bool Transfer(Inventory targetInventory, Item item)
    {
        if (!this.Contains(item)) return false;
        targetInventory.Add(item);
        this.Remove(item);
        return true;
    }
    public bool Transfer(Inventory targetInventory, string id)
    {
        if (!Item.Get(id, out Item? item)) throw new InvalidGameObjectException();
        if (item is null) throw new NullReferenceException();
        return this.Transfer(targetInventory, item);
    }
    public bool Transfer(Inventory targetInventory, Inventory items)
    {
        if (!this.Contains(items)) return false;
        targetInventory.Add(items);
        this.Remove(items);
        return true;
    }
    public bool Transfer(Playerdata player, Item item)
    {
        if (!this.Contains(item)) return false;
        player.Gain(item);
        this.Remove(item);
        return true;
    }
    public bool Transfer(Playerdata player, string id)
    {
        if (!Item.Get(id, out Item? item)) throw new InvalidGameObjectException();
        if (item is null) throw new NullReferenceException();
        return this.Transfer(player, item);
    }
    public bool Transfer(Playerdata player, Inventory items)
    {
        if (!this.Contains(items)) return false;
        player.Gain(items);
        this.Remove(items);
        return true;
    }

    public void Clear() => content.Clear();

    public string PrintContent()
    {
        StringBuilder message = new StringBuilder();
        foreach (KeyValuePair<Item, ulong> pair in content) if (pair.Value > 0) message.Append($"\n{pair.Value}x {pair.Key.Name}");
        return message.ToString();
    }

    public static readonly Inventory Empty = new Inventory(new Dictionary<Item, ulong>());


    [JsonPropertyName("Contents")]
    public Dictionary<Item, ulong> Contents => content;
    readonly Dictionary<Item, ulong> content = new Dictionary<Item, ulong>();

    // * Do not change, not relevant for game design
    public IEnumerator<KeyValuePair<Item, ulong>> GetEnumerator() => content.GetEnumerator();
    Dictionary<Item, ulong>.KeyCollection Keys => content.Keys;
    Dictionary<Item, ulong>.ValueCollection Values => content.Values;

    class InventoryConverter : JsonConverter<Inventory>
    {
        public override Inventory? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject) throw new JsonException();


            var data = new Dictionary<Item, ulong>();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject) return new Inventory(data);

                if (reader.TokenType != JsonTokenType.PropertyName) throw new JsonException();

                string id = reader.GetString() ?? throw new NullReferenceException();

                if (!Item.Get(id, out Item? item)) throw new KeyNotFoundException($"Item with name '{id}' not found.");
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