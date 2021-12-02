namespace AdventOfCode.Year2021
{
	public class DayTwo : Day2021
	{
		protected override object ResolveFirstPart(string[] input)
		{
			return Calculate(input, false);
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return Calculate(input, true);
		}

		private int Calculate(string[] input, bool withAim)
		{
			int horizontal = 0;
			int depth = 0;
			int aim = 0;

			for (int i = 0; i < input.Length; i++)
			{
				string[] fullCommand = input[i].Split(' ');
				string command = fullCommand[0];
				int value = int.Parse(fullCommand[1]);

				if (command == "down")
				{
					if (withAim)
					{
						aim += value;
					}
					else
					{
						depth += value;
					}
				}
				else if (command == "up")
				{
					if (withAim)
					{
						aim -= value;
					}
					else
					{
						depth -= value;
					}
				}
				else
				{
					horizontal += value;
					if (withAim)
					{
						depth += aim * value;
					}
				}
			}

			return horizontal * depth;
		}
	}
}
