namespace Tests;

public class AddRangeTests
{
	// --- AddRange(source) — uses this.Force ---

	[Fact]
	public void AddRange_NoForceArg_UsesForceProperty_True_Evicts()
	{
		BidirectionalDictionary<int, string> map = new() { Force = true };
		map.Add(1, "one");

		// "one" is owned by 1; force=true should evict it and give it to 2
		map.AddRange(new Dictionary<int, string> { [2] = "one" });

		HasExactly(map, (2, "one"));
		False(map.ContainsKey(1));
	}

	[Fact]
	public void AddRange_NoForceArg_UsesForceProperty_False_Throws()
	{
		BidirectionalDictionary<int, string> map = new() { Force = false };
		map.Add(1, "one");

		Throws<ArgumentException>(() =>
			map.AddRange(new Dictionary<int, string> { [2] = "one" }));

		HasExactly(map, (1, "one")); // untouched
	}

	// --- AddRange(source, bool force) ---

	[Fact]
	public void AddRange_Force_False_CleanSource_AddsAll()
	{
		BidirectionalDictionary<int, string> map = [];

		map.AddRange(new Dictionary<int, string> { [1] = "one", [2] = "two", [3] = "three" }, force: false);

		HasExactly(map, (1, "one"), (2, "two"), (3, "three"));
	}

	[Fact]
	public void AddRange_Force_True_CleanSource_AddsAll()
	{
		BidirectionalDictionary<int, string> map = [];

		map.AddRange(new Dictionary<int, string> { [1] = "one", [2] = "two" }, force: true);

		HasExactly(map, (1, "one"), (2, "two"));
	}

	[Fact]
	public void AddRange_Force_False_DuplicateValue_Throws()
	{
		BidirectionalDictionary<int, string> map = [];
		map.Add(1, "one");

		Throws<ArgumentException>(() =>
			map.AddRange(new Dictionary<int, string> { [2] = "one" }, force: false));

		HasExactly(map, (1, "one")); // untouched
	}

	[Fact]
	public void AddRange_Force_False_DuplicateKey_Throws()
	{
		BidirectionalDictionary<int, string> map = [];
		map.Add(1, "one");

		Throws<ArgumentException>(() =>
			map.AddRange(new Dictionary<int, string> { [1] = "uno" }, force: false));

		HasExactly(map, (1, "one")); // untouched
	}

	[Fact]
	public void AddRange_Force_True_ValueConflict_Evicts_LastInWins()
	{
		BidirectionalDictionary<int, string> map = [];
		map.Add(1, "one");

		// key 2 claims "one" — should evict key 1
		map.AddRange(new Dictionary<int, string> { [2] = "one" }, force: true);

		HasExactly(map, (2, "one"));
		False(map.ContainsKey(1));
	}

	[Fact]
	public void AddRange_Force_True_ExistingKey_NewValue_Updates()
	{
		BidirectionalDictionary<int, string> map = [];
		map.Add(1, "one");

		map.AddRange(new Dictionary<int, string> { [1] = "uno" }, force: true);

		HasExactly(map, (1, "uno")); // also implicitly verifies "one" is gone via count
	}

	[Fact]
	public void AddRange_NullSource_Throws()
	{
		BidirectionalDictionary<int, string> map = [];
		Throws<ArgumentNullException>(() => map.AddRange(null!, force: false));
		Throws<ArgumentNullException>(() => map.AddRange(null!, force: true));
	}

	// --- AddRange(source, out conflicts) ---

	[Fact]
	public void AddRange_Conflicts_CleanSource_AddsAll_ConflictsNull()
	{
		BidirectionalDictionary<int, string> map = [];

		map.AddRange(new Dictionary<int, string> { [1] = "one", [2] = "two" }, out var conflicts);

		HasExactly(map, (1, "one"), (2, "two"));
		Null(conflicts);
	}

	[Fact]
	public void AddRange_Conflicts_ValueConflict_Skipped_AddsAfterConflict()
	{
		BidirectionalDictionary<int, string> map = [];
		map.Add(1, "one");

		// key 2 tries to claim "one" which belongs to key 1
		map.AddRange(new Dictionary<int, string> { [2] = "one", [3] = "three" }, out List<KVConflict<int, string>>? conflicts);

		// key 2 skipped, key 3 added
		HasExactly(map, (1, "one"), (3, "three"));
		False(map.ContainsKey(2));

		NotNull(conflicts);
		Single(conflicts);
		Equal(2, conflicts[0].Key);
		Equal("one", conflicts[0].Value);
		Equal(1, conflicts[0].ConflictKey);
	}

	[Fact]
	public void AddRange_Conflicts_FirstInWins()
	{
		// Both key 1 and key 2 want "shared" — key 1 is added first, key 2 is a conflict
		BidirectionalDictionary<int, string> map = [];
		map.Add(1, "shared");

		map.AddRange(new Dictionary<int, string> { [2] = "shared" }, out var conflicts);

		HasExactly(map, (1, "shared"));
		False(map.ContainsKey(2));

		NotNull(conflicts);
		Single(conflicts);
		Equal(1, conflicts[0].ConflictKey);
	}

	[Fact]
	public void AddRange_Conflicts_MultipleConflicts_AllCollected()
	{
		BidirectionalDictionary<int, string> map = [];
		map.Add(1, "one");
		map.Add(2, "two");

		map.AddRange(new Dictionary<int, string> { [10] = "one", [20] = "two", [3] = "three" }, out var conflicts);

		// only key 3 added; original 1 and 2 untouched
		HasExactly(map, (1, "one"), (2, "two"), (3, "three"));

		NotNull(conflicts);
		Equal(2, conflicts.Count);
	}

	[Fact]
	public void AddRange_Conflicts_ExistingKey_NewValue_IsUpdate_NotConflict()
	{
		// TrySet handles key updates — same key with a new unclaimed value is NOT a conflict
		BidirectionalDictionary<int, string> map = [];
		map.Add(1, "one");

		map.AddRange(new Dictionary<int, string> { [1] = "uno" }, out var conflicts);

		HasExactly(map, (1, "uno"));
		Null(conflicts); // update, not a conflict
	}

	[Fact]
	public void AddRange_Conflicts_NullSource_Throws()
	{
		BidirectionalDictionary<int, string> map = [];
		Throws<ArgumentNullException>(() => map.AddRange(null!, out _));
	}
}
