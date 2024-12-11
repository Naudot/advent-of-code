namespace AdventOfCode.Year2015
{
	public class DayThree : Day2015
	{
		protected override object ResolveFirstPart(string[] input)
		{
			int north = 0;
			int east = 0;

			HashSet<(int y, int x)> cases = new();

			cases.Add((0, 0));

			for (int i = 0; i < input[0].Length; i++)
			{
				if (input[0][i] == '<')
					east -= 1;
				if (input[0][i] == '>')
					east += 1;
				if (input[0][i] == '^')
					north += 1;
				if (input[0][i] == 'v')
					north -= 1;

				if (!cases.Contains((north, east)))
					cases.Add((north, east));
			}

			return cases.Count;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			int northSanta = 0;
			int eastSanta = 0;

			int northRobotSanta = 0;
			int eastRobotSanta = 0;

			HashSet<(int, int)> cases = new HashSet<(int, int)>();

			cases.Add((0, 0));

			for (int i = 0; i < input[0].Length; i++)
			{
				if (input[0][i] == '<')
				{
					if (i % 2 == 0)
						eastSanta -= 1;
					else
						eastRobotSanta -= 1;
				}
				if (input[0][i] == '>')
				{
					if (i % 2 == 0)
						eastSanta += 1;
					else
						eastRobotSanta += 1;
				}
				if (input[0][i] == '^')
				{
					if (i % 2 == 0)
						northSanta += 1;
					else
						northRobotSanta += 1;
				}
				if (input[0][i] == 'v')
				{
					if (i % 2 == 0)
						northSanta -= 1;
					else
						northRobotSanta -= 1;
				}

				if (i % 2 == 0)
				{
					if (!cases.Contains((northSanta, eastSanta)))
						cases.Add((northSanta, eastSanta));
				}
				else
				{
					if (!cases.Contains((northRobotSanta, eastRobotSanta)))
						cases.Add((northRobotSanta, eastRobotSanta));
				}
			}

			return cases.Count;
		}
	}
}
