namespace Tests;

public static class XunitX
{
	public static void Single<T>(ICollection<T> coll)
		=> Xunit.Assert.Equal(1, coll.Count); // `Single(coll)` is incorrect, uses enumerator not Count
}
