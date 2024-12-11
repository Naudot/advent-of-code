namespace AdventOfCode.Year2021
{
	public class DaySeven : Day2021
	{
		protected override object ResolveFirstPart(string[] input)
		{
			return CalculateFuel(input, false);
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return CalculateFuel(input, true);
		}

		private int CalculateFuel(string[] input, bool accumulate)
		{
			List<int> crabes = input[0].Split(',').Select(int.Parse).ToList();

			int leastFuel = int.MaxValue;
			int max = crabes.Max();

			for (int i = 0; i < max; i++)
			{
				int fuel = 0;

				for (int j = 0; j < crabes.Count; j++)
				{
					if (accumulate)
					{
						fuel += Sum(Math.Abs(i - crabes[j]));
					}
					else
					{
						fuel += Math.Abs(i - crabes[j]);
					}
				}

				if (fuel < leastFuel)
				{
					leastFuel = fuel;
				}
			}

			return leastFuel;
		}

		private int Sum(int n)
		{
			return (n * (1 + n)) / 2;
		}
	}
}
