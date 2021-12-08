using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2021
{
	public class DayEight : Day2021
	{
		protected override object ResolveFirstPart(string[] input)
		{
			int simpleNumbersCount = 0;
			Dictionary<int, List<char>> numbers = GetNumbers();

			for (int i = 0; i < input.Length; i++)
			{
				string[] line =  input[i].Split('|');
				string[] displayed = line[1].Split(' ');

				for (int j = 0; j < displayed.Length; j++)
				{
					if (displayed[j].Length == numbers[1].Count 
						|| displayed[j].Length == numbers[4].Count 
						|| displayed[j].Length == numbers[7].Count 
						|| displayed[j].Length == numbers[8].Count)
					{
						simpleNumbersCount++;
					}
				}
			}

			return simpleNumbersCount;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			double accumulator = 0;
			for (int i = 0; i < input.Length; i++)
			{
				double lineResult = 0;
				string[] line =  input[i].Split('|');
				string[] rawEntries = line[0].TrimEnd(' ').Split(' ');
				string[] displayed = line[1].TrimStart(' ').Split(' ');

				Dictionary<int, string> entriesAndTheirNumber = new Dictionary<int, string>();
				while (entriesAndTheirNumber.Keys.Count != rawEntries.Length)
				{
					for (int j = 0; j < rawEntries.Length; j++)
					{
						string entry = rawEntries[j];
						if (entriesAndTheirNumber.ContainsValue(entry))
						{
							continue;
						}

						if (entry.Length == 2)
						{
							entriesAndTheirNumber[1] = entry;
						}
						else if (entry.Length == 3)
						{
							entriesAndTheirNumber[7] = entry;
						}
						else if (entry.Length == 4)
						{
							entriesAndTheirNumber[4] = entry;
						}
						else if (entry.Length == 7)
						{
							entriesAndTheirNumber[8] = entry;
						}

						if (!entriesAndTheirNumber.ContainsKey(1) || !entriesAndTheirNumber.ContainsKey(4))
						{
							// I will process it when I'll get enough data
							continue;
						}

						// Je regarde chaque entry

						if (entry.Length == 5) // Either a 2, a 3 or a 5
						{
							// Si elle a 5 branchements soit un 2 un 3 ou un 5
							// donc si elle a les deux mêmes char que le 1 alors c'est un 3
							// sinon elle a les deux mêmes char que le 4 exceptés les char du 1 c'est un 5
							// sinon c'est un 2
							if (HasAllChar(entry, entriesAndTheirNumber[1].ToList()))
							{
								entriesAndTheirNumber[3] = entry;
							}
							else if (HasAllChar(entry, entriesAndTheirNumber[4].ToList().Except(entriesAndTheirNumber[1].ToList()).ToList()))
							{
								entriesAndTheirNumber[5] = entry;
							}
							else
							{
								entriesAndTheirNumber[2] = entry;
							}
						}
						else if (entry.Length == 6)
						{
							// pour 0 6 et 9
							// si elle a deux mêmes char que 1 alors c'est un 0 ou 9 
							// si il a deux mêmes char que 4 alors 9 sinon 0
							// sinon 6
							if (HasAllChar(entry, entriesAndTheirNumber[1].ToList()))
							{
								if (HasAllChar(entry, entriesAndTheirNumber[4].ToList().Except(entriesAndTheirNumber[1].ToList()).ToList()))
								{
									entriesAndTheirNumber[9] = entry;
								}
								else
								{
									entriesAndTheirNumber[0] = entry;
								}
							}
							else
							{
								entriesAndTheirNumber[6] = entry;
							}
						}
					}
				}

				for (int j = 0; j < displayed.Length; j++)
				{
					foreach (KeyValuePair<int, string> item in entriesAndTheirNumber)
					{
						if (displayed[j].Length == item.Value.Length && HasAllChar(displayed[j], item.Value.ToList()))
						{
							lineResult += Math.Pow(10, displayed.Length - j - 1) * item.Key;
						}
					}
				}

				accumulator += lineResult;
			}

			return accumulator;
		}

		public Dictionary<int, List<char>> GetNumbers()
		{
			Dictionary<int, List<char>> numbers = new Dictionary<int, List<char>>();
			numbers[0] = new List<char>() { 'a', 'b', 'c', 'e', 'f', 'g' };
			numbers[1] = new List<char>() { 'c', 'f' };
			numbers[2] = new List<char>() { 'a', 'c', 'd', 'e', 'g' };
			numbers[3] = new List<char>() { 'a', 'c', 'd', 'f', 'g' };
			numbers[4] = new List<char>() { 'b', 'c', 'd', 'f' };
			numbers[5] = new List<char>() { 'a', 'b', 'd', 'f', 'g' };
			numbers[6] = new List<char>() { 'a', 'b', 'd', 'e', 'f', 'g' };
			numbers[7] = new List<char>() { 'a', 'c', 'f' };
			numbers[8] = new List<char>() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' };
			numbers[9] = new List<char>() { 'a', 'b', 'c', 'd', 'f', 'g' };
			return numbers;
		}

		public bool HasAllChar(string str, List<char> chars)
		{
			for (int i = 0; i < chars.Count; i++)
			{
				if (!str.Contains(chars[i]))
				{
					return false;
				}
			}

			return true;
		}	
	}
}
