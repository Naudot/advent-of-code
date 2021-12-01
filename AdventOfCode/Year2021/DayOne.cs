﻿using System.Linq;

namespace AdventOfCode.Year2021
{
	public class DayOne : Day2021
	{
		protected override object ResolveFirstPart(string[] input)
		{
			int increasedMeasurement = 0;
			int previousDepth = int.Parse(input[0]);

			for (int i = 1; i < input.Length; i++)
			{
				int depth = int.Parse(input[i]);
				if (depth > previousDepth)
				{
					increasedMeasurement++;
				}
				previousDepth = depth;
			}

			return increasedMeasurement;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			int[] betterInput = input.Select(int.Parse).ToArray();

			int increasedMeasurement = 0;
			int previousDepth = betterInput[0] + betterInput[1] + betterInput[2];

			for (int i = 1; i < betterInput.Length - 2; i++)
			{
				int depth = betterInput[i] + betterInput[i + 1] + betterInput[i + 2];
				if (depth > previousDepth)
				{
					increasedMeasurement++;
				}
				previousDepth = depth;
			}

			return increasedMeasurement;
		}
	}
}
