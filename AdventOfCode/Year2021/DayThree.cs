using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Year2021
{
	public class DayThree : Day2021
	{
		protected override object ResolveFirstPart(string[] input)
		{
			int length = input[0].Length;

			StringBuilder gammaRateString = new StringBuilder(input[0]);
			StringBuilder epsilonRateString = new StringBuilder(input[0]);

			for (int i = 0; i < length; i++)
			{
				int zeroCount = 0;
				int oneCount = 0;

				for (int j = 0; j < input.Length; j++)
				{
					char bit = input[j][i];

					if (bit == '0')
					{
						zeroCount++;
					}
					else
					{
						oneCount++;
					}
				}

				gammaRateString[i] = zeroCount > oneCount ? '0' : '1';
				epsilonRateString[i] = zeroCount > oneCount ? '1' : '0';
			}

			long epsilonRate = Convert.ToInt64(gammaRateString.ToString(), 2);
			long gammaRate = Convert.ToInt64(epsilonRateString.ToString(), 2);

			return epsilonRate * gammaRate;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			List<string> zeros = new List<string>();
			List<string> ones = new List<string>();

			int zeroCount = 0;
			int oneCount = 0;

			for (int i = 0; i < input.Length; i++)
			{
				char bit = input[i][0];

				if (bit == '0')
				{
					zeroCount++;
					zeros.Add(input[i]);
				}
				else
				{
					oneCount++;
					ones.Add(input[i]);
				}
			}

			List<string> mostUsed = zeroCount > oneCount ? zeros : ones;
			List<string> leastUsed = zeroCount > oneCount ? ones : zeros;

			int length = mostUsed[0].Length;

			int mostUsedIndex = 1;
			// Keep 1
			while (mostUsed.Count != 1)
			{
				zeroCount = mostUsed.Where(str => str[mostUsedIndex] == '0').Count();
				oneCount = mostUsed.Count - zeroCount;
				char notUsedChar = oneCount >= zeroCount ? '0' : '1';
				mostUsed.RemoveAll(str => str[mostUsedIndex] == notUsedChar);
				mostUsedIndex++;
			}

			int leastUsedIndex = 1;
			// Keep 0
			while (leastUsed.Count != 1)
			{
				zeroCount = leastUsed.Where(str => str[leastUsedIndex] == '0').Count();
				oneCount = leastUsed.Count - zeroCount;
				char notUsedChar = oneCount < zeroCount ? '0' : '1';
				leastUsed.RemoveAll(str => str[leastUsedIndex] == notUsedChar);
				leastUsedIndex++;
			}

			return Convert.ToInt64(mostUsed[0], 2) * Convert.ToInt64(leastUsed[0], 2);
		}
	}
}
