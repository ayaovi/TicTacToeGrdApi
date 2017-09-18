using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace tttGrd.Test
{
  [TestFixture]
  internal class UtilitiesTests
  {
    [Test]
    public void Copy_GivenEmptyEnumerable_ShouldReturnEmptyEnumerable()
    {
      //Arrange
      var enumerable = Enumerable.Empty<int>().ToList();

      //Act
      var copy = enumerable.Copy(x => x);

      //Assert
      copy.ShouldAllBeEquivalentTo(enumerable);
    }

    [Test]
    public void Copy_GivenEnumerableWithItems_ShouldReturnCopyOfEnumerable()
    {
      //Arrange
      var enumerable = Enumerable.Range(0, 3).ToList();

      //Act
      var copy = enumerable.Copy(x => x);

      //Assert
      copy.ShouldAllBeEquivalentTo(enumerable);
    }

    [Test]
    public void Maxs_GivenEmptyEnumerable_ShouldReturnEmptyEnumerable()
    {
      //Arrange
      var enumerable = Enumerable.Empty<int>().ToList();

      //Act
      var maxs = enumerable.Maxs((x, y) => x.Equals(y));

      //Assert
      Assert.IsFalse(maxs.Any());
    }

    [Test]
    public void Maxs_GivenEnumerableWithOneItem_ShouldReturnEnumerableWithOneItem()
    {
      //Arrange
      var enumerable = new[] { 0 };

      //Act
      var maxs = enumerable.Maxs((x, y) => x.Equals(y));

      //Assert
      maxs.ShouldAllBeEquivalentTo(enumerable);
    }

    [Test]
    public void Maxs_GivenEnumerableWithItems_ShouldReturnMaxs()
    {
      //Arrange
      var enumerable = new[] { 1, 4, 3, 4, 0 };
      var expected = new[] { 4, 4 };

      //Act
      var maxs = enumerable.Maxs((x, y) => x.Equals(y));

      //Assert
      maxs.ShouldAllBeEquivalentTo(expected);
    }

    [Test]
    public void Random_GivenEmptyEnumerable_ShouldReturnDefaultOfType()
    {
      //Arrange
      var enumerable = Enumerable.Empty<int>().ToList();

      //Act
      var random = enumerable.Random();

      //Assert
      Assert.AreEqual(0, random); /* default(int) is 0. */
    }

    [Test]
    public void Random_GivenEnumerableWithOneItem_ShouldReturnTheOneItem()
    {
      //Arrange
      var enumerable = new[] { 1 };

      //Act
      var random = enumerable.Random();

      //Assert
      Assert.AreEqual(1, random);
    }

    [Test]
    public void Random_GivenEnumerableWithItems_ShouldReturnRandomItem()
    {
      //Arrange
      var enumerable = new[] { 1, 2, 3 };

      //Act
      var random = enumerable.Random();

      //Assert
      Assert.IsTrue(enumerable.Contains(random));
    }
  }
}