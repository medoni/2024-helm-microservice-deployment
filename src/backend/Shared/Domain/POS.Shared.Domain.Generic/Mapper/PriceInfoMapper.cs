using POS.Shared.Domain.Generic.Dtos;

namespace POS.Shared.Domain.Generic.Mapper;
/// <summary>
/// Extension methods that converts <see cref="PriceInfo"/> to <see cref="PriceInfoDto"/>.
/// </summary>
public static class PriceInfoMapper
{
    /// <summary>
    /// Converts a <see cref="PriceInfo"/> to <see cref="PriceInfoDto"/>.
    /// </summary>
    public static PriceInfoDto ToDto(this PriceInfo value)
    => new()
    {
        Price = value.Price.ToDto(),
        RegularyVatInPercent = value.RegularyVatInPercent
    };

    /// <summary>
    /// Converts a <see cref="PriceInfoDto"/> to <see cref="PriceInfo"/>.
    /// </summary>
    public static PriceInfo ToDomain(this PriceInfoDto value)
    => new PriceInfo(
        value.Price.ToDomain(),
        value.RegularyVatInPercent
    );
}
