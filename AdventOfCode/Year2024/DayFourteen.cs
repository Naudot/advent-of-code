using System.Text.RegularExpressions;

namespace AdventOfCode.Year2024
{
	public class Robot
	{
		// x represents the number of tiles the robot is from the left wall
		// and y represents the number of tiles from the top wall
		public (int x, int y) Position;
		// x and y are given in tiles per second. Positive x means the robot is moving to the right,
		// and positive y means the robot is moving down.
		public (int x, int y) Velocity;
	}

	public class DayFourteen : Day2024
	{
		protected override bool DeactivateJIT
		{
			get
			{
				return true;
			}
		}

		protected override object ResolveFirstPart(string[] input)
		{
			List<Robot> robots = GetRobots(input);
			int mapWidth = robots.Select(robot => robot.Position.x).Max() + 1;
			int mapHeight = robots.Select(robot => robot.Position.y).Max() + 1;

			int seconds = 100;

			for (int i = 0; i < robots.Count; i++)
			{
				Robot robot = robots[i];
				robot.Position = ((robot.Position.x + robot.Velocity.x * seconds) % mapWidth,
								  (robot.Position.y + robot.Velocity.y * seconds) % mapHeight);
				if (robot.Position.x < 0)
					robot.Position.x += mapWidth;
				if (robot.Position.y < 0)
					robot.Position.y += mapHeight;
			}

			for (int y = 0; y < mapHeight; y++)
			{
				for (int x = 0; x < mapWidth; x++)
				{
					int count = robots.Count(robot => robot.Position == (x, y));
					Console.Write(count == 0 ? '.' : count.ToString());
				}
				Console.WriteLine();
			}

			List<int> robotCount = new();

			int mapDivision = 2;
			int offsetY = 0;
			for (int divisionY = 0; divisionY < mapDivision; divisionY++)
			{
				int offsetX = 0;
				for (int divisionX = 0; divisionX < mapDivision; divisionX++)
				{
					int lowX = offsetX + mapWidth / mapDivision * divisionX;
					int highX = offsetX + mapWidth / mapDivision * (divisionX + 1);

					int lowY = offsetY + mapHeight / mapDivision * divisionY;
					int highY = offsetY + mapHeight / mapDivision * (divisionY + 1);

					robotCount.Add(robots.Count(robot => 
							robot.Position.x >= lowX 
						&& robot.Position.x < highX 
						&& robot.Position.y >= lowY
						&& robot.Position.y < highY));

					offsetX++;
				}
				offsetY++;
			}

			return robotCount.Aggregate((a, b) => a * b);
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return 0;
		}

		private List<Robot> GetRobots(string[] input)
		{
			List<Robot> robots = new();

			for (int i = 0; i < input.Length; i++)
			{
				Match match = Regex.Match(input[i], @"p=(-?\d+),(-?\d+) v=(-?\d+),(-?\d+)");
				robots.Add(new()
				{
					Position = (int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value)),
					Velocity = (int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value))
				});
			}

			return robots;
		}
	}
}
