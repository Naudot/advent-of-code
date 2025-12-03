namespace AdventOfCode.Year2025
{
	public class DayThree : Day2025
	{
		protected override object ResolveFirstPart(string[] input)
		{
			double sum = 0;

			for (int i = 0; i < input.Length; i++)
			{
				string bank = input[i];
				sum += GetBiggestValue(bank, 2, 0, 0);
			}

			return sum;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			double sum = 0;

			for (int i = 0; i < input.Length; i++)
			{
				string bank = input[i];
				sum += GetBiggestValue(bank, 12, 0, 0);
			}

			return sum;
		}

		private double GetBiggestValue(string bank, int numberOfDigits, int startIndex, double currentResult)
		{
			double result = currentResult;

			int biggestValueIndex = startIndex;
			int biggestValue = 0;

			for (int j = startIndex; j < bank.Length - (numberOfDigits - 1); j++)
			{
				int val = bank[j] - '0';

				if (biggestValue < val)
				{
					biggestValue = val;
					biggestValueIndex = j;
				}
			}

			result += Math.Pow(10, numberOfDigits - 1) * biggestValue;

			if (numberOfDigits > 1)
				return GetBiggestValue(bank, (numberOfDigits - 1), biggestValueIndex + 1, result);

			return result;
		}
	}
}
