using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using POS.Shared.Domain;
using POS.Shared.Persistence.Repositories;
using POS.Shared.Persistence.UOW;
using POS.Shared.Testing;

namespace POS.Shared.Persistence.PostgreSql.UnitTests;

[TestFixture]
[Category(TestCategories.Integration)]
public class PostgreSqlStartupTests
{
    [Test]
    public void AddUnitOfWorkSupport_Should_Work_With_Registered_Generic_Repo()
    {
        // arrange
        var services = new ServiceCollection();
        services.AddSingleton<TestDbContext>();
        services.AddSingleton<IGenericRepository<TestAggregate1>, TestRepository1>();
        services.AddSingleton<ITestRepository2, TestRepository2>();

        // act
        services.AddUnitOfWorkSupport<TestDbContext>();

        // assert
        var svcp = services.BuildServiceProvider();
        var uow = svcp.GetRequiredService<UnitOfWorkFactory>()();

        var aggregate = new TestAggregate1();
        uow.Add(aggregate);
        var repo = (TestRepository1)svcp.GetRequiredService<IGenericRepository<TestAggregate1>>();

        Assert.That(repo.AddedAggregates, Is.EqualTo(new[] { aggregate }));
    }

    [Test]
    public void AddUnitOfWorkSupport_Should_Work_With_Registered_Specific_Repo()
    {
        // arrange
        var services = new ServiceCollection();
        services.AddSingleton<TestDbContext>();
        services.AddSingleton<IGenericRepository<TestAggregate1>, TestRepository1>();
        services.AddSingleton<ITestRepository2, TestRepository2>();

        // act
        services.AddUnitOfWorkSupport<TestDbContext>();

        // assert
        var svcp = services.BuildServiceProvider();
        var uow = svcp.GetRequiredService<UnitOfWorkFactory>()();

        var aggregate = new TestAggregate2();
        uow.Add(aggregate);
        var repo = (TestRepository2)svcp.GetRequiredService<ITestRepository2>();

        Assert.That(repo.AddedAggregates, Is.EqualTo(new[] { aggregate }));
    }

    #region TestDbContext

    private class TestDbContext : DbContext
    {

    }

    #endregion

    #region TestRepository1

    private class TestRepository1 : IGenericRepository<TestAggregate1>
    {
        public List<TestAggregate1> AddedAggregates = new();

        public Task AddAsync(TestAggregate1 aggregate)
        {
            AddedAggregates.Add(aggregate);
            return Task.CompletedTask;
        }

        public Task<TestAggregate1> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<TestAggregate1> IterateAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(TestAggregate1 aggregate)
        {
            throw new NotImplementedException();
        }
    }

    private class TestAggregate1 : AggregateRoot
    {
        private Guid _id = Guid.NewGuid();
        public override Guid Id => _id;

        public override TState GetCurrentState<TState>()
        {
            throw new NotImplementedException();
        }
    }

    #endregion

    #region TestRepository2

    private interface ITestRepository2 : IGenericRepository<TestAggregate2>
    {

    }

    private class TestRepository2 : ITestRepository2
    {
        public List<TestAggregate2> AddedAggregates = new();

        public Task AddAsync(TestAggregate2 aggregate)
        {
            AddedAggregates.Add(aggregate);
            return Task.CompletedTask;
        }

        public Task<TestAggregate2> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<TestAggregate2> IterateAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(TestAggregate2 aggregate)
        {
            throw new NotImplementedException();
        }
    }

    private class TestAggregate2 : AggregateRoot
    {
        private Guid _id = Guid.NewGuid();
        public override Guid Id => _id;

        public override TState GetCurrentState<TState>()
        {
            throw new NotImplementedException();
        }
    }

    #endregion
}
