namespace Tests;

public class ScenarioTests
{
	[Fact]
	public void ComplexScenario_AddSetRemove_MaintainsConsistency()
	{
		// Arrange
		BidirectionalDictionary<string, int> map = [];

		// Act & Assert
		map.Add("A", 1);
		map.Add("B", 2);
		Equal(2, map.Count);

		map.Set("A", 3);  // Should keep B->2 and update A->3
		Equal(2, map.Count);
		Equal(3, map["A"]);
		True(map.ContainsValue(2));  // B->2 should still exist

		map.RemoveByKey("A");
		Single(map);
		False(map.ContainsKey("A"));
		False(map.ContainsValue(3));
		True(map.ContainsKey("B"));  // B->2 should still exist

		map.Add("C", 4);
		Equal(2, map.Count);
	}

	[Fact]
	public void BidirectionalLookup_WorksCorrectly()
	{
		// Arrange
		BidirectionalDictionary<string, int> map = [];
		map.Add("Alice", 101);
		map.Add("Bob", 102);
		map.Add("Charlie", 103);

		// Act & Assert - Forward lookup
		Equal(101, map["Alice"]);
		Equal(102, map["Bob"]);
		Equal(103, map["Charlie"]);

		// Act & Assert - Reverse lookup
		Equal("Alice", map.GetKey(101));
		Equal("Alice", map[101]);
		Equal("Bob", map.GetKey(102));
		Equal("Charlie", map.GetKey(103));
	}
}
