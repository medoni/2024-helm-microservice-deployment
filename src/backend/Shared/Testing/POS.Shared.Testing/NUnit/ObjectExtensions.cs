using System.Text.Json;

namespace POS.Shared.Testing.NUnit;
/// <summary>
/// Extension methods for <see cref="Object"/> for testing purposes.
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    /// Serializes the object to a JSON string.
    /// </summary>
    public static string ToJson(this object value)
    {
        return ToJson(value, new JsonSerializerOptions { WriteIndented = true });
    }

    /// <summary>
    /// Serializes the object to a JSON string.
    /// </summary>
    public static string ToJson(this object value, JsonSerializerOptions options)
    {
        return JsonSerializer.Serialize(value, options);
    }
}
