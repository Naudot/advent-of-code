public static class StaticBank
{
	// x, y
	public static readonly List<(int x, int y)> Directions = new()
	{
		// Up
		(0, -1),
		// Right
		(1, 0),
		// Down
		(0, 1),
		// Left
		(-1, 0)
	};

	public static bool IsInBoundaries((int x, int y) coordinates, string[] map)
	{
		return coordinates.x >= 0 && coordinates.y >= 0 && coordinates.y < map.Length && coordinates.x < map[coordinates.y].Length;
	}
}
