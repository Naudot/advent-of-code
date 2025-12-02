namespace AdventOfCode.Year2025
{
	public class DayTwo : Day2025
	{
		protected override object ResolveFirstPart(string[] input)
		{
			string[] ranges = input[0].Split(',');

			long invalidIDsSum = 0;

			for (int i = 0; i < ranges.Length; i++)
			{
				string[] minMax = ranges[i].Split('-');

				long min = long.Parse(minMax[0]);
				long max = long.Parse(minMax[1]);

				for (long j = min; j <= max; j++)
				{
					if (IsInvalid(j))
						invalidIDsSum += j;
				}
			}

			return invalidIDsSum;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			string[] ranges = input[0].Split(',');

			long invalidIDsSum = 0;

			for (int i = 0; i < ranges.Length; i++)
			{
				string[] minMax = ranges[i].Split('-');

				long min = long.Parse(minMax[0]);
				long max = long.Parse(minMax[1]);

				for (long j = min; j <= max; j++)
				{
					if (IsInvalidCut(j, 2))
						invalidIDsSum += j;
					else if (IsInvalidCut(j, 3))
						invalidIDsSum += j;
					else if (IsInvalidCut(j, 4))
						invalidIDsSum += j;
					else if (IsInvalidCut(j, 5))
						invalidIDsSum += j;
					else if (IsInvalidCut(j, 6))
						invalidIDsSum += j;
					else if (IsInvalidCut(j, 7))
						invalidIDsSum += j;
				}
			}

			return invalidIDsSum;
		}

		private bool IsInvalid(long number)
		{
			int numberOfDigits = number.CountDigits();

			if (numberOfDigits % 2 != 0)
				return false;

			for (int i = 0; i < numberOfDigits / 2; i++)
			{
				long firstDigitFromTheCenter = (number / (int)Math.Pow(10, numberOfDigits / 2 + i)) % 10;
				long firstDigitFromTheRight = (number / (int)Math.Pow(10, i)) % 10;

				if (firstDigitFromTheCenter != firstDigitFromTheRight)
					return false;
			}

			return true;
		}

		private bool IsInvalidCut(long number, int cut)
		{
			int numberOfDigits = number.CountDigits();

			if (numberOfDigits % cut != 0)
				return false;

			for (int i = 0; i < numberOfDigits / cut; i++)
			{
				long[] numbers = new long[cut];

				for (int j = 0; j < cut; j++)
					numbers[j] = (number / (int)Math.Pow(10, (numberOfDigits / cut) * j + i)) % 10;

				for (int j = 0; j < cut - 1; j++)
					if (numbers[j] != numbers[j + 1])
						return false;

			}

			return true;
		}
	}
}
