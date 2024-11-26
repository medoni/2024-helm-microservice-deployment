using POS.Shared.Infrastructure.Api.Dtos;

namespace POS.Shared.Infrastructure.Api.Extensions;

/// <summary>
/// Extension methods for ResultSet
/// </summary>
public static class ResultSetExtensionMethods
{
    /// <summary>
    /// Converts a Enumerable into paginated ResultSet in memory.
    /// </summary>
    /// <param name="items">List of items</param>
    /// <param name="token">Pagination token</param>
    public static ResultSetDto<TDto> ToInMemoryPaginatedResultSet<TDto>(
        this IEnumerable<TDto> items,
        string? token
    )
    {
        // TODO: missing implementation
        return new ResultSetDto<TDto>(items.ToArray());
    }
}
