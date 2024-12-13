public static class ExtensionsDouble
{
	public static bool HasDecimal(this double d)
	{
		return Math.Abs(d % 1) >= double.Epsilon;
	}
}
