using POS.Shared.Testing;

#nullable disable

namespace POS.Shared.Domain.UnitTests;


[TestFixture]
[Category(TestCategories.Unit)]
public class ValueObjectTests
{
    [Test]
    public void Equals_Should_Return_Correct_Values()
    {
        // arrange
        Assert.That(
            new TestValueObject(12.13m, "Foo").Equals(new TestValueObject(12.13m, "Foo")),
            Is.True
        );

        Assert.That(
            new TestValueObject(12.13m, "Foo").Equals(new TestValueObject(13, "Foo")),
            Is.False
        );

        Assert.That(
            new TestValueObject(12.13m, "Foo").Equals(new TestValueObject(12.13m, "Bar")),
            Is.False
        );

        Assert.That(
            new TestValueObject(12.13m, "Foo").Equals(null),
            Is.False
        );

        Assert.That(
            new TestValueObject(12.13m, "Foo").Equals("Foo"),
            Is.False
        );
    }

    [Test]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Assertion", "NUnit2010:Use EqualConstraint for better assertion messages in case of failure", Justification = "<Pending>")]
    public void Equality_Operator_Should_Return_Correct_Values()
    {
        // arrange
        Assert.That(
            new TestValueObject(12.13m, "Foo") == new TestValueObject(12.13m, "Foo"),
            Is.True
        );

        Assert.That(
            new TestValueObject(12.13m, "Foo") == new TestValueObject(13, "Foo"),
            Is.False
        );

        Assert.That(
            new TestValueObject(12.13m, "Foo") == new TestValueObject(12.13m, "Bar"),
            Is.False
        );

        Assert.That(
            new TestValueObject(12.13m, "Foo") == null,
            Is.False
        );

        Assert.That(
            ((TestValueObject)null) == ((TestValueObject)null),
            Is.True
        );
    }

    [Test]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Assertion", "NUnit2010:Use EqualConstraint for better assertion messages in case of failure", Justification = "<Pending>")]
    public void Unequal_Operator_Should_Return_Correct_Values()
    {
        // arrange
        Assert.That(
            new TestValueObject(12.13m, "Foo") != new TestValueObject(12.13m, "Foo"),
            Is.False
        );

        Assert.That(
            new TestValueObject(12.13m, "Foo") != new TestValueObject(13, "Foo"),
            Is.True
        );

        Assert.That(
            new TestValueObject(12.13m, "Foo") != new TestValueObject(12.13m, "Bar"),
            Is.True
        );

        Assert.That(
            new TestValueObject(12.13m, "Foo") != null,
            Is.True
        );

        Assert.That(
            ((TestValueObject)null) != ((TestValueObject)null),
            Is.False
        );
    }


    [Test]
    public void GetHashCode_Should_Return_Correct_Values()
    {
        Assert.That(
            new TestValueObject(12.13m, "Foo").GetHashCode(),
            Is.EqualTo(new TestValueObject(12.13m, "Foo").GetHashCode())
        );

        Assert.That(
            new TestValueObject(12.13m, "Foo").GetHashCode(),
            Is.Not.EqualTo(new TestValueObject(12, "Foo").GetHashCode())
        );

        Assert.That(
            new TestValueObject(12.13m, "Foo").GetHashCode(),
            Is.Not.EqualTo(new TestValueObject(12.13m, "Bar").GetHashCode())
        );
    }

    public class TestValueObject : ValueObject
    {
        public decimal NumberItem { get; }
        public string StringItem { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return NumberItem;
            yield return StringItem;
        }

        public TestValueObject(decimal numberItem, string stringItem)
        {
            NumberItem = numberItem;
            StringItem = stringItem;
        }
    }
}
