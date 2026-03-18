namespace Tests;

public class RemoveTests
{
	[Fact]
	public void RemoveByKey_ExistingKey_RemovesAndReturnsTrue()
	{
		// Arrange
		BidirectionalDictionary<int, string> map = [];
		map.Add(1, "one");

		// Act
		bool result = map.RemoveByKey(1);

		// Assert
		True(result);
		Empty(map);
		False(map.ContainsKey(1));
		False(map.ContainsValue("one"));
	}

	[Fact]
	public void RemoveByKey_NonExistingKey_ReturnsFalse()
	{
		// Arrange
		BidirectionalDictionary<int, string> map = [];

		// Act
		bool result = map.RemoveByKey(1);

		// Assert
		False(result);
	}

	[Fact]
	public void RemoveByValue_ExistingValue_RemovesAndReturnsTrue()
	{
		// Arrange
		BidirectionalDictionary<int, string> map = [];
		map.Add(1, "one");

		// Act
		bool result = map.RemoveByValue("one");

		// Assert
		True(result);
		Empty(map);
		False(map.ContainsKey(1));
		False(map.ContainsValue("one"));
	}

	[Fact]
	public void RemoveByValue_NonExistingValue_ReturnsFalse()
	{
		// Arrange
		BidirectionalDictionary<int, string> map = [];

		// Act
		bool result = map.RemoveByValue("one");

		// Assert
		False(result);
	}

	[Fact]
	public void Remove_ExistingKey_RemovesAndReturnsTrue()
	{
		// Arrange
		BidirectionalDictionary<int, string> map = [];
		map.Add(1, "one");

		// Act
		bool result = map.Remove(1);

		// Assert
		True(result);
		Empty(map);
	}
}
