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

	public static Direction GetDirectionOfArrow(char arrow)
	{
		return arrow switch
		{
			'^' => Direction.NORTH,
			'>' => Direction.EAST,
			'v' => Direction.SOUTH,
			'<' => Direction.WEST,
			_ => Direction.NORTH,
		};
	}

	public static char GetSymbolOfDirection((int x, int y) direction)
	{
		return direction switch
		{
			(0, -1) => '^',
			(1, 0) => '>',
			(0, 1) => 'v',
			_ => '<'
		};
	}

	public static (int x, int y) GetValueOfDirection(Direction direction)
	{
		return direction switch
		{
			Direction.NORTH => Directions[0],
			Direction.EAST => Directions[1],
			Direction.SOUTH => Directions[2],
			Direction.WEST => Directions[3],
			_ => (0, 0),
		};
	}

	public static bool IsInBoundaries((int x, int y) coordinates, string[] map)
	{
		return coordinates.x >= 0 && coordinates.y >= 0 && coordinates.y < map.Length && coordinates.x < map[coordinates.y].Length;
	}
}
