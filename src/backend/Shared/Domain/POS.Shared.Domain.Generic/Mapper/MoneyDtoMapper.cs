﻿using POS.Shared.Domain.Generic.Dtos;

namespace POS.Shared.Domain.Generic.Mapper;

/// <summary>
/// Extension methods to map <see cref="Money"/> and <see cref="MoneyDto"/>.-
/// </summary>
public static class MoneyDtoMapper
{
    /// <summary>
    /// Maps an <see cref="MoneyDto"/> to <see cref="Money"/>.
    /// </summary>
    public static Money ToEntity(this MoneyDto dto)
    => new Money(dto.Amount, dto.Currency);

    /// <summary>
    /// Maps an <see cref="Money"/> to <see cref="MoneyDto"/>.
    /// </summary>
    public static MoneyDto ToDto(this Money entity)
    => new MoneyDto(entity.Amount, entity.Currency);
}