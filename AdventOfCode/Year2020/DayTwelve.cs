using System.Text.RegularExpressions;

namespace AdventOfCode.Year2020
{
	public class DayTwelve : Day2020
	{
		protected override object ResolveFirstPart()
		{
			MatchCollection matchCollection = Regex.Matches(File.ReadAllText(GetResourcesPath()), @"([A-Z])(\d*)");

			Direction currentDirection = Direction.EAST;
			int east = 0;
			int north = 0;

			for (int i = 0; i < matchCollection.Count; i++)
			{
				Match match = matchCollection[i];

				string letter = match.Groups[1].Value;
				int value = int.Parse(match.Groups[2].Value);

				if (letter == "N")
				{
					north += value;
				}
				else if (letter == "S")
				{
					north -= value;
				}
				else if (letter == "E")
				{
					east += value;
				}
				else if (letter == "W")
				{
					east -= value;
				}
				else if (letter == "F")
				{
					if (currentDirection == Direction.EAST)
					{
						east += value;
					}
					if (currentDirection == Direction.WEST)
					{
						east -= value;
					}
					if (currentDirection == Direction.NORTH)
					{
						north += value;
					}
					if (currentDirection == Direction.SOUTH)
					{
						north -= value;
					}
				}
				else
				{
					int enumSize = Enum.GetNames(typeof(Direction)).Length;
					int direction = (int)currentDirection;
					int clockwise = letter == "L" ? -1 : 1;
					while (value != 0)
					{
						direction += clockwise;
						value -= 90;
					}
					while (direction < 0)
					{
						direction += enumSize;
					}
					while (direction >= enumSize)
					{
						direction -= enumSize;
					}
					currentDirection = (Direction)direction;
				}
			}

			return Math.Abs(east) + Math.Abs(north);
		}

		protected override object ResolveSecondPart()
		{
			MatchCollection matchCollection = Regex.Matches(File.ReadAllText(GetResourcesPath()), @"([A-Z])(\d*)");

			int waypointEast = 10;
			int waypointNorth = 1;
			int waypointWest = -waypointEast;
			int waypointSouth = -waypointNorth;
			int shipEast = 0;
			int shipNorth = 0;

			for (int i = 0; i < matchCollection.Count; i++)
			{
				Match match = matchCollection[i];

				string letter = match.Groups[1].Value;
				int value = int.Parse(match.Groups[2].Value);

				if (letter == "N")
				{
					waypointNorth += value;
					waypointSouth = -waypointNorth;
				}
				else if (letter == "S")
				{
					waypointNorth -= value;
					waypointSouth = -waypointNorth;
				}
				else if (letter == "E")
				{
					waypointEast += value;
					waypointWest = -waypointEast;
				}
				else if (letter == "W")
				{
					waypointEast -= value;
					waypointWest = -waypointEast;
				}
				else if (letter == "F")
				{
					shipEast += value * waypointEast;
					shipNorth += value * waypointNorth;
				}
				else
				{
					value %= 360;
					while (value != 0)
					{
						if (letter == "L")
						{
							int tmp = waypointNorth;
							waypointNorth = waypointEast;
							waypointEast = waypointSouth;
							waypointSouth = waypointWest;
							waypointWest = tmp;
						}
						else
						{
							int tmp = waypointNorth;
							waypointNorth = waypointWest;
							waypointWest = waypointSouth;
							waypointSouth = waypointEast;
							waypointEast = tmp;
						}

						value -= 90;
					}
				}
			}

			return Math.Abs(shipEast) + Math.Abs(shipNorth);
		}
	}
}
