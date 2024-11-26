using POS.Shared.Domain.Events;
using POS.Shared.Testing;

namespace POS.Shared.Domain.UnitTests;

[TestFixture]
[Category(TestCategories.Unit)]
public class AggregateReootTests
{
    [Test]
    public void Ctor_Should_Set_Correct_Values()
    {
        // act
        var agg = new TestAggregate(42, "Foo");

        // assert
        Assert.That(agg.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(agg.NumberItem, Is.EqualTo(42));
        Assert.That(agg.StringItem, Is.EqualTo("Foo"));
    }

    [Test]
    public void Ctor_With_State_Should_Set_Correct_Values()
    {
        // act
        var state = new TestState
        {
            Id = Guid.NewGuid(),
            NumberItem = 42,
            StringItem = "Foo"
        };
        var agg = new TestAggregate(state);

        // assert
        Assert.That(agg.Id, Is.EqualTo(state.Id));
        Assert.That(agg.NumberItem, Is.EqualTo(42));
        Assert.That(agg.StringItem, Is.EqualTo("Foo"));
    }

    [Test]
    public void GetUncommittedChanges_Should_Return_Correct_Values()
    {
        // arrange
        var agg = new TestAggregate(42, "Foo");
        agg.ChangeNumberItem(43);
        agg.ChangeStringItem("Bar");

        // act
        var result = agg.GetUncommittedChanges();

        // assert
        Assert.That(result, Is.EqualTo(new IDomainEvent[] {
            new NumberItemChangedEvent(agg.Id, 43),
            new StringItemChangedEvent(agg.Id, "Bar")
        }));

        Assert.That(agg.GetUncommittedChanges(), Has.Exactly(2).Items);
        Assert.That(agg.NumberItem, Is.EqualTo(43));
        Assert.That(agg.StringItem, Is.EqualTo("Bar"));
    }

    [Test]
    public void GetUncommittedChanges_Should_Return_Correct_Values_When_No_Changes_Applied()
    {
        // arrange
        var agg = new TestAggregate(42, "Foo");

        // act
        var result = agg.GetUncommittedChanges();

        // assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GetCurrentState_Should_Return_Correct_Values()
    {
        // arrange
        var agg = new TestAggregate(42, "Foo");
        agg.ChangeNumberItem(43);
        agg.ChangeStringItem("Bar");

        // act
        var result = agg.GetCurrentState<TestState>();

        // assert
        Assert.That(result, Is.EqualTo(new TestState
        {
            Id = agg.Id,
            NumberItem = 43,
            StringItem = "Bar"
        }));
    }

    [Test]
    public void FlushUncommittedChanges_Should_Return_Correct_Values()
    {
        // arrange
        var agg = new TestAggregate(42, "Foo");
        agg.ChangeNumberItem(43);
        agg.ChangeStringItem("Bar");

        // act
        var result = agg.FlushUncommittedChanges();

        // assert
        Assert.That(result, Is.EqualTo(new IDomainEvent[] {
            new NumberItemChangedEvent(agg.Id, 43),
            new StringItemChangedEvent(agg.Id, "Bar")
        }));

        Assert.That(agg.GetUncommittedChanges(), Is.Empty);
        Assert.That(agg.NumberItem, Is.EqualTo(43));
        Assert.That(agg.StringItem, Is.EqualTo("Bar"));
    }

    [Test]
    public void FlushUncommittedChanges_Should_Return_Correct_Values_When_No_Changes_Applied()
    {
        // arrange
        var agg = new TestAggregate(42, "Foo");

        // act
        var result = agg.FlushUncommittedChanges();

        // assert
        Assert.That(result, Is.Empty);
        Assert.That(agg.NumberItem, Is.EqualTo(42));
        Assert.That(agg.StringItem, Is.EqualTo("Foo"));
    }

    public class TestAggregate : AggregateRoot
    {
        private TestState _state;

        public override Guid Id => _state.Id;

        public decimal NumberItem
        {
            get => _state.NumberItem;
            private set => _state.NumberItem = value;
        }

        public string StringItem
        {
            get => _state.StringItem;
            private set => _state.StringItem = value;
        }

        public override TState GetCurrentState<TState>()
        => (dynamic)_state;

        public TestAggregate(TestState state)
        {
            _state = state ?? throw new ArgumentNullException(nameof(state));
        }

        public TestAggregate(decimal numberItem, string stringItem)
        : this(new TestState() { Id = Guid.NewGuid(), NumberItem = numberItem, StringItem = stringItem })
        {

        }

        public void ChangeNumberItem(decimal newValue)
        {
            NumberItem = newValue;

            Apply(new NumberItemChangedEvent(Id, newValue));
        }

        public void ChangeStringItem(string newValue)
        {
            StringItem = newValue;

            Apply(new StringItemChangedEvent(Id, newValue));
        }
    }

    public record TestState
    {
        public Guid Id { get; set; }
        public decimal NumberItem { get; set; }
        public string StringItem { get; set; }
    }

    public record NumberItemChangedEvent(
        Guid Id,
        decimal NewNumberItem
    ) : IDomainEvent;

    public record StringItemChangedEvent(
        Guid Id,
        string NewNumberItem
    ) : IDomainEvent;
}
