public static class ExtensionsDictionary
{
	public static void AddUp<TKey>(this IDictionary<TKey, long> map, TKey key, long value)
	{
		if (map.ContainsKey(key))
			map[key] += value;
		else
			map.Add(key, value);
	}
}
