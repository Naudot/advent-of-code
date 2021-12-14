using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2021
{
	public class DayFourteen : Day2021
	{
		protected override object ResolveFirstPart(string[] input)
		{
			List<int> chain = new List<int>();
			Dictionary<(int, int), int> pairs = new Dictionary<(int, int), int>();
			Dictionary<int, int> charCount = new Dictionary<int, int>();

			for (int i = 0; i < input[0].Length; i++)
			{
				chain.Add(input[0][i]);
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

			int step = 10;
			for (int i = 0; i < step; i++)
			{
				//Console.WriteLine();
				//for (int j = 0; j < chain.Count; j++)
				//{
				//	Console.Write((char)chain[j]);
				//}
				int count = chain.Count;
				for (int j = 0; j < count - 1; j++)
				{
					(int, int) pair = (chain[j], chain[j + 1]);
					int valueToInsert = pairs[pair];
					chain.Insert(j + 1, valueToInsert);
					if (charCount.ContainsKey(valueToInsert))
					{
						charCount[valueToInsert]++;
					}
					else
					{
						charCount.Add(valueToInsert, 1);
					}
					j++;
					count++;
				}
			}

			int min = int.MaxValue;
			int max = int.MinValue;

			foreach (KeyValuePair<int, int> item in charCount)
			{
				if (item.Value < min)
				{
					min = item.Value;
				}
				if (item.Value > max)
				{
					max = item.Value;
				}
			}

			return max - min;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			Dictionary<(int, int), int> chain = new Dictionary<(int, int), int>();
			Dictionary<(int, int), int> pairs = new Dictionary<(int, int), int>();
			Dictionary<int, int> charCount = new Dictionary<int, int>();

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

			int step = 3;
			for (int i = 0; i < step; i++)
			{
				Dictionary<(int, int), int> clone = chain.ToDictionary(entry => entry.Key, entry => entry.Value);
				foreach (KeyValuePair<(int, int), int> clonedItem in clone)
				{
					if (clonedItem.Value == 0)
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
						chain[newFirstPair] += clonedItem.Value;
					}
					else
					{
						chain.Add(newFirstPair, clonedItem.Value);
					}
					if (chain.ContainsKey(newSecondPair))
					{
						chain[newSecondPair] += clonedItem.Value;
					}
					else
					{
						chain.Add(newSecondPair, clonedItem.Value);
					}
					chain[oldPair]--;

					if (charCount.ContainsKey(valueToInsert))
					{
						charCount[valueToInsert] += clonedItem.Value;
					}
					else
					{
						charCount.Add(valueToInsert, clonedItem.Value);
					}
				}
			}

			int min = int.MaxValue;
			int max = int.MinValue;

			foreach (KeyValuePair<int, int> item in charCount)
			{
				if (item.Value < min)
				{
					min = item.Value;
				}
				if (item.Value > max)
				{
					max = item.Value;
				}
			}

			return max - min;
		}
	}
}
