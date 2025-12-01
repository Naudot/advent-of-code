namespace AdventOfCode.Year2025
{
	public class DayOne : Day2025
	{
		protected override object ResolveFirstPart(string[] input)
		{
			int dialToZeroCount = 0;
			int dial = 50;

			for (int i = 0; i < input.Length; i++)
			{
				dial = (dial + (input[i][0] == 'L' ? -1 : 1) * int.Parse(input[i][1..])) % 100;
				dialToZeroCount += (dial == 0 ? 1 : 0);
			}

			return dialToZeroCount;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			int dialToZeroCount = 0;
			int dial = 50;

			for (int i = 0; i < input.Length; i++)
			{
				bool isDialStartingAtZero = dial == 0;
				dial += (input[i][0] == 'L' ? -1 : 1) * int.Parse(input[i][1..]);

				while (dial > 99)
				{
					dial -= 100;
					if (dial != 0)
						dialToZeroCount++;
				}

				while (dial < 0)
				{
					if (isDialStartingAtZero)
						isDialStartingAtZero = false;
					else if (dial != 0)
						dialToZeroCount++;

					dial += 100;
				}

				dialToZeroCount += (dial == 0 ? 1 : 0);
			}

			return dialToZeroCount;
		}
	}
}
