using POS.Shared.Domain.Generic.Dtos;

namespace POS.Shared.Domain.Generic.Mapper;
public static class MoneyDtoMapper
{
    public static Money ToEntity(this MoneyDto dto)
    => new Money(dto.Amount, dto.Currency);

    public static MoneyDto ToDto(this Money entity)
    => new MoneyDto(entity.Amount, entity.Currency);
}
