using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using POS.Domains.Payment.Service.Domain;
using POS.Domains.Payment.Service.Dtos;
using POS.Domains.Payment.Service.Exceptions;
using POS.Domains.Payment.Service.Mapper;
using POS.Domains.Payment.Service.Processors;
using POS.Shared.Domain.Generic.Dtos;
using POS.Shared.Testing;
using POS.Shared.Testing.NUnit;
using System.Collections.Concurrent;

namespace POS.Domains.Payment.Service.UnitTests;

[TestFixture]
[Category(TestCategories.Unit)]
public class DefaultPaymentServiceTests
{
    private DefaultPaymentService Sut { get; set; }

    private IKeyedServiceProvider ServiceProviderMock { get; set; }
    private IPaymentRepository PaymentRepository { get; set; }

    [SetUp]
    public void SetUp()
    {
        ServiceProviderMock = Substitute.For<IKeyedServiceProvider>();
        PaymentRepository = new InMemoryPaymentRepository();

        Sut = new DefaultPaymentService(
            ServiceProviderMock,
            PaymentRepository
        );

        ServiceProviderMock
            .GetRequiredKeyedService(typeof(IPaymentProcessor), PaymentProviders.Paypal)
            .Returns(new TestPaymentProcessor());
    }

    #region RequestPaymentAsync

    [Test]
    public async Task RequestPaymentAsync_Should_Return_Correct_Result()
    {
        // arrange
        var requestDto = new RequestPaymentDto()
        {
            EntityId = "abcdef1234",
            EntityType = EntityTypes.CustomerOrder,
            Provider = PaymentProviders.Paypal,
            RequestedAt = new DateTimeOffset(2024, 12, 24, 06, 25, 00, TimeSpan.Zero)
        };

        // act
        var result = await Sut.RequestPaymentAsync(requestDto);

        // assert
        Assert.That(result.PaymentId, Is.Not.EqualTo(Guid.Empty));
        Assert.That(result.EntityId, Is.EqualTo(requestDto.EntityId));
        Assert.That(result.EntityType, Is.EqualTo(requestDto.EntityType));
        Assert.That(result.RequestedAt, Is.EqualTo(requestDto.RequestedAt));
        Assert.That(result.State, Is.EqualTo(PaymentStates.Requested));
        Assert.That(result.Provider, Is.EqualTo(PaymentProviders.Paypal));
        Assert.That(result.Description, Is.EqualTo(TestPaymentProviderState.Default.Description));
        Assert.That(result.Amount, Is.EqualTo(TestPaymentProviderState.Default.Amount));
        Assert.That(result.Links, Is.EqualTo(TestPaymentProviderState.Default.Links));
        Assert.That(result.PayedAt, Is.Null);
    }

    [Test]
    public async Task RequestPaymentAsync_Should_Throw_Exception_When_Payment_Is_Still_In_Progress()
    {
        // arrange
        var requestDto = new RequestPaymentDto()
        {
            EntityId = "abcdef1234",
            EntityType = EntityTypes.CustomerOrder,
            Provider = PaymentProviders.Paypal,
            RequestedAt = new DateTimeOffset(2024, 12, 24, 06, 25, 00, TimeSpan.Zero)
        };

        await PaymentRepository.AddOrUpdateAsync(new PaymentEntity()
        {
            Id = Guid.NewGuid(),
            EntityId = requestDto.EntityId,
            EntityType = requestDto.EntityType,
            Provider = requestDto.Provider,
            RequestedAt = requestDto.RequestedAt,
            State = PaymentStates.Requested,
            ProviderState = TestPaymentProviderState.Default,
        });

        // act
        Assert.That(() =>
            Sut.RequestPaymentAsync(requestDto),
            Throws.TypeOf<PaymentStillInProgress>()
        );
    }

    [Test]
    public async Task RequestPaymentAsync_Should_Store_Correct_Result()
    {
        // arrange
        var requestDto = new RequestPaymentDto()
        {
            EntityId = "abcdef1234",
            EntityType = EntityTypes.CustomerOrder,
            Provider = PaymentProviders.Paypal,
            RequestedAt = new DateTimeOffset(2024, 12, 24, 06, 25, 00, TimeSpan.Zero)
        };

        // act
        var result = await Sut.RequestPaymentAsync(requestDto);

        // assert
        var storedState = await PaymentRepository.GetAsync(result.PaymentId);
        Assert.That(storedState.Id, Is.EqualTo(result.PaymentId));
        Assert.That(storedState.EntityId, Is.EqualTo(requestDto.EntityId));
        Assert.That(storedState.EntityType, Is.EqualTo(requestDto.EntityType));
        Assert.That(storedState.RequestedAt, Is.EqualTo(requestDto.RequestedAt));
        Assert.That(storedState.State, Is.EqualTo(PaymentStates.Requested));
        Assert.That(storedState.Provider, Is.EqualTo(requestDto.Provider));
        Assert.That(storedState.ProviderState, Is.EqualTo(TestPaymentProviderState.Default));
        Assert.That(storedState.PayedAt, Is.Null);
    }

    #endregion

    #region GetPaymentDetailsAsync

    [Test]
    public async Task GetPaymentDetailsAsync_Should_Return_Correct_Result()
    {
        // arrange
        var paymentId = Guid.Parse("eabf2aec-76e4-484c-82d2-69bcc1b62f7d");
        var paymentEntity = new PaymentEntity
        {
            Id = paymentId,
            EntityType = EntityTypes.CustomerOrder,
            EntityId = "abcdef",
            RequestedAt = new DateTimeOffset(2024, 12, 29, 07, 24, 33, TimeSpan.Zero),
            State = PaymentStates.Requested,
            Provider = PaymentProviders.Paypal,
            ProviderState = TestPaymentProviderState.Default
        };

        await PaymentRepository.AddOrUpdateAsync(paymentEntity);

        // act
        var result = await Sut.GetPaymentDetailsAsync(paymentId);

        // assert
        Assert.That(result.ToJson(), Is.EqualTo(paymentEntity.ToDetailsDto().ToJson()));
    }

    [Test]
    public void GetPaymentDetailsAsync_Should_Throw_Exception_When_Entity_Was_Not_Found()
    {
        // arrange
        var paymentId = Guid.NewGuid();

        // act
        Assert.That(
            () => Sut.GetPaymentDetailsAsync(paymentId),
            Throws.TypeOf<PaymentNotFoundException>()
        );
    }

    #endregion

    private class TestPaymentProcessor : IPaymentProcessor
    {
        public const string Description = "Lorem Ipsum";

        public async Task<PaymentProviderState> RequestPaymentAsync(RequestPaymentDto dto)
        {
            await Task.Yield();

            return TestPaymentProviderState.Default;
        }
    }

    private class TestPaymentProviderState : PaymentProviderState
    {

        public static TestPaymentProviderState Default = new()
        {
            Amount = GrossNetPriceDto.CreateByGross(MoneyDto.Create(42.23m, "EUR"), 7),
            Description = "Lorem Ipsum",
            Links = new()
                {
                    new PaymentLinkDescription("http://example.com", PaymentLinkTypes.Approve, PaymentLinkMethods.GET)
                },
            PaymentProviderId = "prov_id_123"
        };
    }

    private class InMemoryPaymentRepository : IPaymentRepository
    {
        private readonly ConcurrentDictionary<Guid, PaymentEntity> _store = new();

        public async Task AddOrUpdateAsync(PaymentEntity entity)
        {
            await Task.Yield();
            _store[entity.Id] = entity;
        }

        public async Task<PaymentEntity> GetAsync(Guid paymentId)
        {
            await Task.Yield();
            if (!_store.TryGetValue(paymentId, out var entity))
            {
                throw new PaymentNotFoundException(paymentId);
            }
            return entity;
        }

        public async Task<PaymentEntity> GetByEntityIdAsync(EntityTypes type, string entityId)
        {
            var entity = await TryGetByEntityIdAsync(type, entityId);
            if (entity is null)
            {
                throw new PaymentNotFoundException(type, entityId);
            }
            return entity;
        }

        public async Task<PaymentEntity?> TryGetByEntityIdAsync(EntityTypes type, string entityId)
        {
            await Task.Yield();
            foreach (var entity in _store.Values)
            {
                if (entity.EntityType == type && entity.EntityId == entityId)
                {
                    return entity;
                }
            }

            return null;
        }
    }
}
