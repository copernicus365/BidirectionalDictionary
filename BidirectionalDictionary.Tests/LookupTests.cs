namespace Tests;

public class LookupTests
{
	[Fact]
	public void GetValue_ExistingKey_ReturnsValue()
	{
		// Arrange
		BidirectionalDictionary<int, string> map = [];
		map.Add(1, "one");

		// Act
		string value = map.GetValue(1);
		string valueFromIndexer = map[1]; // Indexer should also work for forward lookup

		// Assert
		Equal("one", value);
		Equal("one", valueFromIndexer);
	}

	[Fact]
	public void GetValue_NonExistingKey_ThrowsKeyNotFoundException()
	{
		// Arrange
		BidirectionalDictionary<int, string> map = [];

		// Act & Assert
		Throws<KeyNotFoundException>(() => map.GetValue(1));
	}

	[Fact]
	public void GetKey_ExistingValue_ReturnsKey()
	{
		// Arrange
		BidirectionalDictionary<int, string> map = [];
		map.Add(1, "one");

		// Act
		int key = map.GetKey("one");

		// Assert
		Equal(1, key);
	}

	[Fact]
	public void GetKey_NonExistingValue_ThrowsKeyNotFoundException()
	{
		// Arrange
		BidirectionalDictionary<int, string> map = [];

		// Act & Assert
		Throws<KeyNotFoundException>(() => map.GetKey("one"));
	}

	[Fact]
	public void TryGetValue_ExistingKey_ReturnsTrueAndValue()
	{
		// Arrange
		BidirectionalDictionary<int, string> map = [];
		map.Add(1, "one");

		// Act
		bool result = map.TryGetValue(1, out string? value);

		// Assert
		True(result);
		Equal("one", value);
	}

	[Fact]
	public void TryGetValue_NonExistingKey_ReturnsFalse()
	{
		// Arrange
		BidirectionalDictionary<int, string> map = [];

		// Act
		bool result = map.TryGetValue(1, out string? value);

		// Assert
		False(result);
		Null(value);
	}

	[Fact]
	public void TryGetKey_ExistingValue_ReturnsTrueAndKey()
	{
		// Arrange
		BidirectionalDictionary<int, string> map = [];
		map.Add(1, "one");

		// Act
		bool result = map.TryGetKey("one", out int key);

		// Assert
		True(result);
		Equal(1, key);
	}

	[Fact]
	public void TryGetKey_NonExistingValue_ReturnsFalse()
	{
		// Arrange
		BidirectionalDictionary<int, string> map = [];

		// Act
		bool result = map.TryGetKey("one", out int key);

		// Assert
		False(result);
		Equal(0, key);
	}

	[Fact]
	public void Indexer_Get_ExistingKey_ReturnsValue()
	{
		// Arrange
		BidirectionalDictionary<int, string> map = [];
		map.Add(1, "one");

		// Act
		string value = map[1];

		// Assert
		Equal("one", value);
	}

	[Fact]
	public void Indexer_Get_NonExistingKey_ThrowsKeyNotFoundException()
	{
		// Arrange
		BidirectionalDictionary<int, string> map = [];

		// Act & Assert
		Throws<KeyNotFoundException>(() => map[1]);
	}

	[Fact]
	public void Indexer_Set_UpdatesValue()
	{
		// Arrange
		BidirectionalDictionary<int, string> map = [];
		map.Add(1, "one");

		// Act
		map[1] = "uno";

		// Assert
		Equal("uno", map[1]);
	}
}
