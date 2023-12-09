using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2023
{
	public class DayNine : Day2023
	{
		protected override bool DeactivateJIT
		{
			get
			{
				return true;
			}
		}

		protected override object ResolveFirstPart(string[] input)
		{
			int sum = 0;

			for (int i = 0; i < input.Length; i++)
			{
				List<int[]> arrays = new List<int[]>();
				bool isFound = false;

				int[] array = input[i].Split(' ').Select(term => int.Parse(term)).ToArray();
				arrays.Add(array);

				while (!isFound)
				{
					int[] diff = new int[array.Length - 1];

					for (int j = 0; j < array.Length - 1; j++)
					{
						int first = array[j];
						int second = array[j + 1];
						diff[j] = second - first;
					}

					array = diff;
					arrays.Add(array);

					if (!diff.Where(value => value != 0).Any())
					{
						int lineSum = 0;

						for (int k = 0; k < arrays.Count; k++)
						{
							int[] calcArray = arrays[arrays.Count - 1 - k];
							lineSum += calcArray[calcArray.Length - 1];
						}

						sum += lineSum;
						isFound = true;
					}
				}

			}

			return sum;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return string.Empty;
		}
	}
}
