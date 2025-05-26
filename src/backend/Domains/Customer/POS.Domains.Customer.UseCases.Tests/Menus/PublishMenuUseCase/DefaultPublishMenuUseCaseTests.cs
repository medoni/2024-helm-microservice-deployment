using NSubstitute;
using POS.Domains.Customer.Domain.Menus;
using POS.Domains.Customer.Persistence.Menus;
using POS.Domains.Customer.UseCases.Menus.PublishMenuUseCase;
using POS.Shared.Persistence.UOW;
using POS.Shared.Testing;

namespace POS.Domains.Customer.UseCases.Tests.Menus.PublishMenuUseCase;

[TestFixture]
[Category(TestCategories.Unit)]
public class DefaultPublishMenuUseCaseTests
{
    private DefaultPublishMenuUseCase Sut { get; set; }
    private UnitOfWorkFactory _uowFactoryMock;
    private IMenuRespository _menuRepositoryMock;
    private IUnitOfWork _uowMock;

    [SetUp]
    public void SetUp()
    {
        _uowMock = Substitute.For<IUnitOfWork>();
        _uowFactoryMock = Substitute.For<UnitOfWorkFactory>();
        _menuRepositoryMock = Substitute.For<IMenuRespository>();

        _uowFactoryMock.Invoke().Returns(_uowMock);

        Sut = new DefaultPublishMenuUseCase(
            _uowFactoryMock,
            _menuRepositoryMock
        );
    }

    [Test]
    public async Task PublishAsync_WhenNoActiveMenu_ShouldActivateNewMenuAndCommit()
    {
        // arrange
        var menuId = Guid.NewGuid();
        _menuRepositoryMock.GetActiveAsync().Returns((Menu?)null);

        var menuToActivate = TestDataGenerator.CreateMenu(menuId);
        _uowMock.GetAsync<Menu>(menuId).Returns(menuToActivate);

        // act
        await Sut.PublishAsync(menuId);

        // assert
        Assert.That(menuToActivate.IsActive, Is.True);
        await _uowMock.Received(1).CommitAsync();
    }

    [Test]
    public async Task PublishAsync_WithExistingActiveMenu_ShouldDeactivateOldAndActivateNewMenu()
    {
        // arrange
        var activeMenuId = Guid.NewGuid();
        var newMenuId = Guid.NewGuid();

        var activeMenu = TestDataGenerator.CreateMenu(activeMenuId);
        _menuRepositoryMock.GetActiveAsync().Returns(activeMenu);

        _uowMock.GetAsync<Menu>(activeMenuId).Returns(activeMenu);

        var newActiveMenu = TestDataGenerator.CreateMenu(newMenuId);
        _uowMock.GetAsync<Menu>(newMenuId).Returns(newActiveMenu);

        // act
        await Sut.PublishAsync(newMenuId);

        // assert
        Assert.That(activeMenu.IsActive, Is.False);
        Assert.That(newActiveMenu.IsActive, Is.True);
        await _uowMock.Received(1).CommitAsync();
    }

    [Test]
    public async Task GetActiveAsync_WhenActiveMenuExists_ShouldReturnMenuDto()
    {
        // arrange
        var activeMenuId = Guid.NewGuid();
        var activeMenu = TestDataGenerator.CreateMenu(activeMenuId);
        _menuRepositoryMock.GetActiveAsync().Returns(activeMenu);

        // act
        var result = await Sut.GetActiveAsync();

        // assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(activeMenuId));
    }

    [Test]
    public async Task GetActiveAsync_WhenNoActiveMenuExists_ShouldReturnNull()
    {
        // arrange
        _menuRepositoryMock.GetActiveAsync().Returns((Menu?)null);

        // act
        var result = await Sut.GetActiveAsync();

        // assert
        Assert.That(result, Is.Null);
    }
}
