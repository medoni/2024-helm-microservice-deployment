using NSubstitute;
using POS.Domains.Customer.Domain.Menus;
using POS.Domains.Customer.Persistence.Menus;
using POS.Domains.Customer.UseCases.Menus.CRUDMenuUseCase;
using POS.Domains.Customer.UseCases.Menus.CRUDMenuUseCase.Dtos;
using POS.Shared.Persistence.UOW;
using POS.Shared.Testing;

namespace POS.Domains.Customer.UseCases.Tests.Menus.CRUDMenuUseCase;

[TestFixture]
[Category(TestCategories.Unit)]
public class DefaultCRUDMenuUseCaseTests
{
    private DefaultCRUDMenuUseCase Sut { get; set; }
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

        Sut = new DefaultCRUDMenuUseCase(
            _uowFactoryMock,
            _menuRepositoryMock
        );
    }

    [Test]
    public async Task CreateMenuAsync_ShouldAddMenuToUnitOfWorkAndCommit()
    {
        // arrange
        var dto = new CreateMenuDto(
            Guid.NewGuid(),
            "EUR",
            new List<MenuSectionDto>()
        );

        // act
        await Sut.CreateMenuAsync(dto);

        // assert
        _uowMock.Received(1).Add(Arg.Is<Menu>(m =>
            m.Id == dto.Id &&
            m.Currency == dto.Currency
        ));
        await _uowMock.Received(1).CommitAsync();
    }

    [Test]
    public async Task UpdateMenuAsync_ShouldUpdateMenuSectionsAndCommit()
    {
        // arrange
        var id = Guid.NewGuid();
        var dto = new UpdateMenuDto(
            id,
            new List<MenuSectionDto>()
        );

        var menu = TestDataGenerator.CreateMenu(id);
        _uowMock.GetAsync<Menu>(id).Returns(menu);

        // act
        await Sut.UpdateMenuAsync(dto);

        // assert
        Assert.That(menu.Sections, Is.Empty);
        await _uowMock.Received(1).CommitAsync();
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnMenuDto()
    {
        // arrange
        var id = Guid.NewGuid();
        var menu = TestDataGenerator.CreateMenu(id);
        _uowMock.GetAsync<Menu>(id).Returns(menu);

        // act
        var result = await Sut.GetByIdAsync(id);

        // assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(id));
    }

    [Test]
    public async Task GetAllAsync_ShouldReturnAllMenus()
    {
        // arrange
        var menu1 = TestDataGenerator.CreateMenu();
        var menu2 = TestDataGenerator.CreateMenu();

        _menuRepositoryMock.IterateAsync().Returns(new[] { menu1, menu2 }.ToAsyncEnumerable());

        // act
        var result = Sut.GetAllAsync();
        var menus = await result.ToListAsync();

        // assert
        Assert.That(menus, Is.Not.Null);
        Assert.That(menus.Count, Is.EqualTo(2));
        Assert.That(menus[0].Id, Is.EqualTo(menu1.Id));
        Assert.That(menus[1].Id, Is.EqualTo(menu2.Id));
    }
}
