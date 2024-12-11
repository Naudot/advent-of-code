﻿using System.Text.RegularExpressions;

namespace AdventOfCode.Year2021
{
	public class DayFourteen : Day2021
	{
		protected override object ResolveFirstPart(string[] input)
		{
			return Resolve(input, 10);
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return Resolve(input, 40);
		}

		private long Resolve(string[] input, int step)
		{
			Dictionary<(int, int), long> chain = new Dictionary<(int, int), long>();
			Dictionary<(int, int), int> pairs = new Dictionary<(int, int), int>();
			Dictionary<int, long> charCount = new Dictionary<int, long>();

			for (int i = 0; i < input[0].Length; i++)
			{
				if (i < input[0].Length - 1)
				{
					(int, int) pair = (input[0][i], input[0][i + 1]);

					if (chain.ContainsKey(pair))
					{
						chain[pair]++;
					}
					else
					{
						chain.Add(pair, 1);
					}
				}

				if (charCount.ContainsKey(input[0][i]))
				{
					charCount[input[0][i]]++;
				}
				else
				{
					charCount.Add(input[0][i], 1);
				}
			}

			for (int i = 2; i < input.Length; i++)
			{
				string line = input[i];
				Match match = Regex.Match(line, @"(\w+) -> (\w)");
				(int, int) pair = (match.Groups[1].Value[0], match.Groups[1].Value[1]);
				int val = match.Groups[2].Value[0];
				pairs.Add(pair, val);
			}

			for (int i = 0; i < step; i++)
			{
				Dictionary<(int, int), long> clone = chain.ToDictionary(entry => entry.Key, entry => entry.Value);
				foreach (KeyValuePair<(int, int), long> clonedItem in clone)
				{
					long pairCount = clonedItem.Value;

					if (pairCount == 0)
					{
						continue;
					}

					// Each pair becomes two pairs
					(int, int) oldPair = clonedItem.Key;
					int valueToInsert = pairs[oldPair];
					(int, int) newFirstPair = (clonedItem.Key.Item1, valueToInsert);
					(int, int) newSecondPair = (valueToInsert, clonedItem.Key.Item2);

					if (chain.ContainsKey(newFirstPair))
					{
						chain[newFirstPair] += pairCount;
					}
					else
					{
						chain.Add(newFirstPair, pairCount);
					}
					if (chain.ContainsKey(newSecondPair))
					{
						chain[newSecondPair] += pairCount;
					}
					else
					{
						chain.Add(newSecondPair, pairCount);
					}
					chain[oldPair] -= clonedItem.Value;

					if (charCount.ContainsKey(valueToInsert))
					{
						charCount[valueToInsert] += pairCount;
					}
					else
					{
						charCount.Add(valueToInsert, pairCount);
					}
				}
			}

			IEnumerable<long> values = charCount.Select(item => item.Value);
			return values.Max() - values.Min();
		}
	}
}
