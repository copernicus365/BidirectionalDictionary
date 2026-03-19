namespace Tests;

public class ReadmeExampleTests
{
	[Fact]
	public void FirstReadme_Example()
	{
		BidirectionalDictionary<int, string> map = [];

		// Add. Like with normal Dictionary, checks for conflicts. Throws if key OR value already exists
		map.Add(101, "Alice");
		map.Add(102, "Bob");

		// Indexer (=Set()). Add is a bit lighter when you know you are adding new entries
		map[103] = "Charlie";

		// Indexer call above is an indirection call to, and thus is identical, to:
		map.Set(103, "Charlie", force: map.Force);

		// Set. Can be an addition (new key/value), but also can be an Update
		map.Set(120, "Sophie");

		// Forward lookup (key -> value)
		string name = map[101];
		Equal("Alice", name);

		// Reverse lookup (value -> key)
		int id = map["Bob"];         // 102 (reverse indexer)
		int id2 = map.GetKey("Bob"); // 102 (explicit method)

		Equal(102, id);
		Equal(102, id2);

		// Check existence
		bool hasUser = map.ContainsKey(101);      // true
		bool hasName = map.ContainsValue("Bob");  // true

		True(hasUser);
		True(hasName);

		// Remove mappings
		True(map.RemoveByKey(102));
		True(map.RemoveByValue("Charlie"));
	}

	[Fact]
	public void TrySet_Sophie()
	{
		BidirectionalDictionary<int, string> map = [];
		map.Add(110, "Rudolph");
		map[115] = "Zog";
		map.Set(120, "Sophie");

		Equal(3, map.Count); // verify 3 items exist

		True(map.TrySet(120, "Sophia", out int conflictKey));
		// success, conflictKey = 0 (default)

		False(map.TrySet(220, "Sophia", out conflictKey));
		// fail! conflictKey = 120 ... key 120 is already paired with value "Sophia"

		Equal(3, map.Count); // verify still only 3 items, the failed TrySet didn't add a new pair

		False(map.ContainsValue("Sophie"));
		// original value "Sophie" is gone

		True(map.TryGetKey("Sophia", out int ownerKey) && ownerKey == 120);
	}
}
