#pragma warning disable IDE0130 // Namespace does not match folder structure
using NUnit.Framework.Constraints;
using System.Text.Json;

namespace NUnit.Framework;

/// <summary>
/// Extension methods for <see cref="Constraint"/>.
/// </summary>
public static class ContstrainExtensions
{
    /// <summary>
    /// Compares two values using Json-Serializer
    /// </summary>
    public static EqualConstraint UsingJson(this EqualConstraint constraint)
    {
        return constraint.Using(new UsingJsonComparer(JsonSerializerOptions.Default));
    }

    private class UsingJsonComparer(
        JsonSerializerOptions SerializerOptions
    ) : IComparer<object>
    {
        public int Compare(object? x, object? y)
        {
            if (ReferenceEquals(x, null) && ReferenceEquals(y, null)) return 0;
            if (ReferenceEquals(x, null)) return -1;
            if (ReferenceEquals(y, null)) return 1;

            var xt = x.GetType();
            var yt = y.GetType();

            if (xt == typeof(string) && yt == typeof(string)) return StringComparer.InvariantCulture.Compare(x, y);

            if (xt != typeof(string)) x = JsonSerializer.Serialize(x, xt, SerializerOptions);
            if (yt != typeof(string)) y = JsonSerializer.Serialize(y, yt, SerializerOptions);

            return StringComparer.InvariantCulture.Compare(x, y);
        }
    }
}
