namespace AdventOfCode.Year2025
{
	public class DayThree : Day2025
	{
		protected override object ResolveFirstPart(string[] input)
		{
			int sum = 0;

			for (int i = 0; i < input.Length; i++)
			{
				string bank = input[i];

				int biggestLeftValueIndex = -1;
				int biggestLeftValue = 0;

				for (int j = 0; j < bank.Length - 1; j++)
				{
					int val = bank[j] - '0';

					if (biggestLeftValue < val)
					{
						biggestLeftValue = val;
						biggestLeftValueIndex = j;
					}
				}

				int biggestRightValue = 0;

				for (int j = biggestLeftValueIndex + 1; j < bank.Length; j++)
				{
					int val = bank[j] - '0';

					if (biggestRightValue < val)
						biggestRightValue = val;
				}

				sum += biggestLeftValue * 10 + biggestRightValue;
			}

			return sum;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return 0;
		}
	}
}
