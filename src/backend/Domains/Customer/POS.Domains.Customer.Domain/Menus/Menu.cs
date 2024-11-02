using POS.Domains.Customer.Domain.Menus.Dtos;
using POS.Domains.Customer.Domain.Menus.Dtos.Mapper;
using POS.Shared.Domain;

namespace POS.Domains.Customer.Domain.Menus;

public class Menu : AggregateRoot<Guid>
{
    private readonly MenuEntity _state;
    public override TState GetCurrentState<TState>() => (dynamic)_state;

    public override Guid Id => _state.MenuId;

    public DateTimeOffset CreatedAt
    {
        get => _state.CreatedAt;
    }

    public DateTimeOffset LastChangedAt
    {
        get => _state.LastChangedAt;
        private set => _state.LastChangedAt = value;
    }

    public bool IsActive
    {
        get => _state.IsActive ?? false;
        private set => _state.IsActive = value ? true : null;
    }

    public DateTimeOffset? ActivatedAt
    {
        get => _state.ActivatedAt;
        private set => _state.ActivatedAt = value;
    }

    public IReadOnlyList<MenuSection> Sections
    {
        get => _state.Sections.ToDomain();
        private set
        {
            _state.Sections.Clear();
            foreach (var item in value)
            {
                _state.Sections.Add(item.ToEntity());
            }
        }
    }

    #region ctor

    public Menu(MenuEntity state)
    {
        _state = state;
    }

    public Menu(
        Guid id,
        DateTimeOffset createdAt,
        IReadOnlyList<MenuSection> sections
    )
    : this(
        new MenuEntity(
            id,
            createdAt,
            sections.ToEntity()
        )
    )
    {
    }

    #endregion

    #region UpdateSections

    public void UpdateSections(
        DateTimeOffset updatedAt,
        IReadOnlyList<MenuSection> sections
    )
    {
        EnsureIsInactive();
        EnsureUpdatedAtIsValid(updatedAt, nameof(updatedAt));

        Sections = sections;
        LastChangedAt = updatedAt;
    }

    #endregion

    #region Activate / Deactivate

    public void Activate(
        DateTimeOffset activateAt
    )
    {
        if (IsActive) return;
        EnsureUpdatedAtIsValid(activateAt, nameof(activateAt));

        IsActive = true;
        ActivatedAt = activateAt;
        LastChangedAt = activateAt;
    }

    public void Deactivate(
        DateTimeOffset deactivateAt
    )
    {
        if (!IsActive) return;
        EnsureUpdatedAtIsValid(deactivateAt, nameof(deactivateAt));

        IsActive = false;
        ActivatedAt = null;
        LastChangedAt = deactivateAt;
    }

    #endregion

    #region Generic Ensure ...

    private void EnsureUpdatedAtIsValid(DateTimeOffset newValue, string argumentName)
    {
        if (newValue < this.LastChangedAt) throw new ArgumentException($"{argumentName} cannot be in the past. NewValue: '{newValue}', actual: {this.LastChangedAt}", argumentName);
    }

    private void EnsureIsInactive()
    {
        if (IsActive) throw new InvalidOperationException("Menu is active. Cannot perform action.");
    }

    #endregion
}
