using Newtonsoft.Json.Linq;
using System;
using System.Collections;
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
				}
			}

			return invalidIDsSum;
		}

		private bool IsInvalid(long number)
		{
			int numberOfDigits = number.CountDigits();

			if (numberOfDigits % 2 == 1)
				return false;

			for (int j = 0; j < numberOfDigits / 2; j++)
			{
				long firstDigitFromTheCenter = (number / (int)Math.Pow(10, numberOfDigits / 2 + j)) % 10;
				long firstDigitFromTheRight = (number / (int)Math.Pow(10, j)) % 10;

				if (firstDigitFromTheCenter != firstDigitFromTheRight)
					return false;
			}

			return true;
		}
	}
}
