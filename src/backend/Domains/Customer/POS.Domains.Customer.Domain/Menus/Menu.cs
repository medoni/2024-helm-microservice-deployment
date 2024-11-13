using POS.Domains.Customer.Domain.Menus.Entities;
using POS.Domains.Customer.Domain.Menus.Entities.Mapper;
using POS.Shared.Domain;

namespace POS.Domains.Customer.Domain.Menus;

/// <summary>
/// Aggregate root for Menu
/// </summary>
public class Menu : AggregateRoot<Guid>
{
    private readonly MenuEntity _state;

    /// <inheritdoc/>
    public override TState GetCurrentState<TState>() => (dynamic)_state;

    /// <summary>
    /// The Id of the Menu.
    /// </summary>
    public override Guid Id => _state.MenuId;

    /// <summary>
    /// Date and time when the Menu was created.
    /// </summary>
    public DateTimeOffset CreatedAt
    {
        get => _state.CreatedAt;
    }

    /// <summary>
    /// Date and time when the Menu was last changed at.
    /// </summary>
    public DateTimeOffset LastChangedAt
    {
        get => _state.LastChangedAt;
        private set => _state.LastChangedAt = value;
    }

    /// <summary>
    /// True, when Menu is currently active. To get active, the Menu must be published.
    /// </summary>
    public bool IsActive
    {
        get => _state.IsActive ?? false;
        private set => _state.IsActive = value ? true : null;
    }

    /// <summary>
    /// Date and time when the menu was getting active.
    /// </summary>
    public DateTimeOffset? ActivatedAt
    {
        get => _state.ActivatedAt;
        private set => _state.ActivatedAt = value;
    }

    /// <summary>
    /// Menu sections.
    /// </summary>
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

    /// <summary>
    /// Creates a new Menu.
    /// </summary>
    public Menu(MenuEntity state)
    {
        _state = state;
    }

    /// <summary>
    /// Creates a new Menu.
    /// </summary>
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

    /// <summary>
    /// Updates the sections of this Menu.
    /// </summary>
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

    /// <summary>
    /// Activates this menu.
    /// </summary>
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

    /// <summary>
    /// Deactivates the menu.
    /// </summary>
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
