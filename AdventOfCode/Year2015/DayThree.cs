namespace AdventOfCode.Year2015
{
	public class DayThree : Day2015
	{
		protected override object ResolveFirstPart()
		{
			char[] input = File.ReadAllText(GetResourcesPath()).ToCharArray();

			int north = 0;
			int east = 0;

			HashSet<Tuple<int, int>> cases = new HashSet<Tuple<int, int>>();

			cases.Add(new Tuple<int, int>(0, 0));

			for (int i = 0; i < input.Length; i++)
			{
				if (input[i] == '<')
				{
					east -= 1;
				}
				if (input[i] == '>')
				{
					east += 1;
				}
				if (input[i] == '^')
				{
					north += 1;
				}
				if (input[i] == 'v')
				{
					north -= 1;
				}

				if (cases.FirstOrDefault(t => t.Item1 == north && t.Item2 == east) == null)
				{
					cases.Add(new Tuple<int, int>(north, east));
				}
			}

			return cases.Count;
		}

		protected override object ResolveSecondPart()
		{
			char[] input = File.ReadAllText(GetResourcesPath()).ToCharArray();

			int northSanta = 0;
			int eastSanta = 0;

			int northRobotSanta = 0;
			int eastRobotSanta = 0;

			HashSet<Tuple<int, int>> cases = new HashSet<Tuple<int, int>>();

			cases.Add(new Tuple<int, int>(0, 0));

			for (int i = 0; i < input.Length; i++)
			{
				if (input[i] == '<')
				{
					if (i % 2 == 0)
					{
						eastSanta -= 1;
					}
					else
					{
						eastRobotSanta -= 1;
					}
				}
				if (input[i] == '>')
				{
					if (i % 2 == 0)
					{
						eastSanta += 1;
					}
					else
					{
						eastRobotSanta += 1;
					}
				}
				if (input[i] == '^')
				{
					if (i % 2 == 0)
					{
						northSanta += 1;
					}
					else
					{
						northRobotSanta += 1;
					}
				}
				if (input[i] == 'v')
				{
					if (i % 2 == 0)
					{
						northSanta -= 1;
					}
					else
					{
						northRobotSanta -= 1;
					}
				}

				if (i % 2 == 0)
				{
					if (cases.FirstOrDefault(t => t.Item1 == northSanta && t.Item2 == eastSanta) == null)
					{
						cases.Add(new Tuple<int, int>(northSanta, eastSanta));
					}
				}
				else
				{
					if (cases.FirstOrDefault(t => t.Item1 == northRobotSanta && t.Item2 == eastRobotSanta) == null)
					{
						cases.Add(new Tuple<int, int>(northRobotSanta, eastRobotSanta));
					}
				}
			}

			return cases.Count;
		}
	}
}
