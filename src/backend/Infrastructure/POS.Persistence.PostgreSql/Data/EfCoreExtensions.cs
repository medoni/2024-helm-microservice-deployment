using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace POS.Persistence.PostgreSql.Data;
internal static class EfCoreExtensions
{
    public static PropertyBuilder<T> HasJsonConversion<T>(this PropertyBuilder<T> propertyBuilder)
    {
        var converter = new ValueConverter<T, string>(
            v => Serialize(v),
            v => Deserialize<T>(v));

        var comparer = new ValueComparer<T>(
            (l, r) => Serialize(l) == Serialize(r),
            v => v == null ? 0 : Serialize(v).GetHashCode(),
            v => Deserialize<T>(Serialize(v)));

        propertyBuilder.HasConversion(converter);
        propertyBuilder.Metadata.SetValueConverter(converter);
        propertyBuilder.Metadata.SetValueComparer(comparer);

        return propertyBuilder;
    }

    private static string Serialize<T>(T value)
    {
        return JsonSerializer.Serialize(value);
    }

    private static T Deserialize<T>(string value)
    {
        return JsonSerializer.Deserialize<T>(value)!;
    }
}
