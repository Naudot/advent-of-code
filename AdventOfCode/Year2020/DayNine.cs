using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Year2020
{
	public class DayNine : Day2020
	{
		protected override object ResolveFirstPart()
		{
			long[] input = File.ReadAllLines(GetResourcesPath()).Select(long.Parse).ToArray();

			for (int i = 25; i < input.Length; i++)
			{
				if (!IsSumOfNumbers(input, i - 25, i, input[i]))
				{
					return input[i];
				}
			}

			return long.MinValue;
		}

		protected override object ResolveSecondPart()
		{
			long[] input = File.ReadAllLines(GetResourcesPath()).Select(long.Parse).ToArray();
			long foundNumber = 0;
			for (int i = 25; i < input.Length; i++)
			{
				if (!IsSumOfNumbers(input, i - 25, i, input[i]))
				{
					foundNumber = input[i];
				}
			}

			for (int i = 2; i < input.Length; i++)
			{
				int contiguousCount = i; // Better name

				for (int j = 0; j < input.Length - contiguousCount; j++)
				{
					long addedValues = 0;
					List<long> values = new List<long>();

					for (int k = j; k < j + contiguousCount; k++)
					{
						addedValues += input[k];
						values.Add(input[k]);
					}

					if (addedValues == foundNumber)
					{
						return values.Min() + values.Max();
					}
				}
			}

			return long.MinValue;
		}

		private bool IsSumOfNumbers(long[] input, int startIndex, int endIndex, long value)
		{
			for (int i = startIndex; i < endIndex; i++)
			{
				long firstNumber = input[i];

				for (int j = i + 1; j < endIndex; j++)
				{
					long secondNumber = input[j];

					if (firstNumber + secondNumber == value)
					{
						return true;
					}
				}
			}

			return false;
		}
	}
}
