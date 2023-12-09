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
				int[] diff;
				int[] current = input[i].Split(' ').Select(term => int.Parse(term)).ToArray();

				List<int[]> arrays = new List<int[]>();
				arrays.Add(current);

				bool isFound = false;
				while (!isFound)
				{
					diff = new int[current.Length - 1];
					for (int j = 0; j < current.Length - 1; j++)
					{
						int first = current[j];
						int second = current[j + 1];
						diff[j] = second - first;
					}

					current = diff;
					arrays.Add(current);
					isFound = !diff.Where(value => value != 0).Any();
				}

				sum += arrays.Select(array => array[array.Length - 1]).Sum();
			}

			return sum;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return string.Empty;
		}
	}
}
