namespace POS.Shared.Infrastructure.Api.Dtos;

/// <summary>
/// Dto for list of data
/// </summary>
/// <param name="Data">The collection of items in the current result set.</param>
public record ResultSetDto<TData>(
    IReadOnlyList<TData> Data
)
{
    /// <summary>
    /// Token to retrieve the next page of results.
    /// </summary>
    public string? NextPageToken { get; set; }

    /// <summary>
    /// Token to retrieve the previous page of results.
    /// </summary>
    public string? PreviousPageToken { get; set; }

    /// <summary>
    /// Additional metadata or information about the request or result.
    /// </summary>
    public Dictionary<string, string> Metadata { get; set; } = new();
}
