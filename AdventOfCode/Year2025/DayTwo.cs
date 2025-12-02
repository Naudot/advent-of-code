namespace AdventOfCode.Year2025
{
	public class DayTwo : Day2025
	{
		long[] pows =
		{
			0,
			1,
			10,
			100,
			1000,
			10000,
			100000,
			1000000,
			10000000,
			100000000,
			1000000000,
			10000000000,
			100000000000,
			1000000000000,
			10000000000000
		};

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
					if (IsInvalidCut(j, 2))
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

				int cutCount = 7;

				for (long j = min; j <= max; j++)
				{
					for (int k = 0; k < cutCount; k++)
					{
						if (IsInvalidCut(j, cutCount))
						{
							invalidIDsSum += j;
							break;
						}
					}
				}
			}

			return invalidIDsSum;
		}

		private bool IsInvalidCut(long number, int cut)
		{
			int numberOfDigits = number.CountDigits();

			if (numberOfDigits % cut != 0)
				return false;

			int partsCount = numberOfDigits / cut;

			for (int i = 0; i < partsCount; i++)
				for (int j = 0; j < cut - 1; j++)
					if ((number / (int)Math.Pow(10, (partsCount) * j + i)) % 10	!= // This line take the ((partsCount) * j + i)nth digit
						(number / (int)Math.Pow(10, (partsCount) * (j + 1) + i)) % 10) // Take the next left digit
						return false;

			return true;
		}
	}
}
