using Newtonsoft.Json;

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
        return ToJson(value, Formatting.Indented);
    }

    /// <summary>
    /// Serializes the object to a JSON string.
    /// </summary>
    public static string ToJson(this object value, Formatting formatting)
    {
        return JsonConvert.SerializeObject(value, Formatting.Indented);
    }
}
