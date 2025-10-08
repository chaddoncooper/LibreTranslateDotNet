using System.Text.Json;
using System.Text.Json.Serialization;

namespace LibreTranslate.DotNet.Models;

public class DetectedLanguage
{
    [JsonPropertyName("confidence")]
    public float Confidence { get; set; }

    [JsonPropertyName("language")]
    public required string Language { get; set; }
}

public class SingleOrArrayConverter<T> : JsonConverter<T[]>
{
    public override T[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.StartArray)
        {
            return JsonSerializer.Deserialize<T[]>(ref reader, options)!;
        }
        else
        {
            var single = JsonSerializer.Deserialize<T>(ref reader, options)!;
            return [single];
        }
    }

    public override void Write(Utf8JsonWriter writer, T[] value, JsonSerializerOptions options)
    {
        if (value.Length == 1)
        {
            JsonSerializer.Serialize(writer, value[0], options);
        }
        else
        {
            JsonSerializer.Serialize(writer, value, options);
        }
    }
}

public class SingleOrArrayOfArrayConverter : JsonConverter<string[][]>
{
    public override string[][] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.StartArray)
        {
            var clone = reader;
            clone.Read();
            if (clone.TokenType == JsonTokenType.String || clone.TokenType == JsonTokenType.EndArray)
            {
                var flat = JsonSerializer.Deserialize<string[]>(ref reader, options)!;
                return [flat];
            }
            else
            {
                return JsonSerializer.Deserialize<string[][]>(ref reader, options)!;
            }
        }
        else
        {
            var single = JsonSerializer.Deserialize<string>(ref reader, options)!;
            return [[single]];
        }
    }

    public override void Write(Utf8JsonWriter writer, string[][] value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}

public class TranslateResponse
{
    [JsonPropertyName("alternatives")]
    [JsonConverter(typeof(SingleOrArrayOfArrayConverter))]
    public string[][]? Alternatives { get; set; }

    [JsonPropertyName("detectedLanguage")]
    [JsonConverter(typeof(SingleOrArrayConverter<DetectedLanguage>))]
    public DetectedLanguage[]? DetectedLanguage { get; set; }

    [JsonPropertyName("translatedText")]
    [JsonConverter(typeof(SingleOrArrayConverter<string>))]
    public required string[] TranslatedText { get; set; }
}
