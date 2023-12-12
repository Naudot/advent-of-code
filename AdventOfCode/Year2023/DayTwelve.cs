using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2023
{
	public class DayTwelve : Day2023
	{
		protected override object ResolveFirstPart(string[] input)
		{
			int sum = 0;

			for (int i = 0; i < input.Length; i++)
			{
				Console.WriteLine("Begin " + input[i]);
				string state = input[i].Split(' ')[0];
				int[] values = input[i].Split(' ')[1].Split(',').Select(val => int.Parse(val)).ToArray();

				int valSum = values.Sum();
				int knownSum = state.Where(c => c == '#').Count();

				// Only one arrangement is known
				if (knownSum == valSum)
				{
					sum++;
					continue;
				}

				int questionSum = state.Where(c => c == '?').Count();
				int toProcess = valSum - knownSum; // We need to replace this amount of ? and check if conditions are met

				// Regex pour valider si on a les bons groupes pour plus tard : ([#]+)

				//int iterationsCount = 1;
				//if (toProcess != questionSum)
				//{
				//	iterationsCount = Fact(questionSum) / (Fact(toProcess) * Fact(questionSum - toProcess));
				//}

				List<List<int>> combinaisons = GetCombinaisons(questionSum, toProcess);

				for (int j = 0; j < combinaisons.Count; j++)
				{
					List<int> indexes = combinaisons[j].Select(combiValue => GetNthIndex(state, '?', combiValue + 1)).ToList();
					string newState = state;
					StringBuilder sb = new StringBuilder(newState);
					for (int k = 0; k < newState.Length; k++)
					{
						if (sb[k] == '?')
						{
							sb[k] = indexes.Contains(k) ? '#' : '.';
						}
					}
					newState = sb.ToString();

					MatchCollection matchCollection = Regex.Matches(newState, @"([#]+)");

					if (values.Length != matchCollection.Count)
					{
						continue;
					}

					bool matchValues = true;
					for (int k = 0; k < matchCollection.Count; k++)
					{
						Match match = matchCollection[k];
						string val = match.Groups[0].Value;

						if (val.Where(c => c == '#').Count() != values[k])
						{
							matchValues = false;
							break;
						}
					}

					sum += (matchValues ? 1 : 0);
				}

			}

			return sum;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return string.Empty;
		}

		//private int Fact(int number)
		//{
		//	int result = number;

		//	for (int i = 1; i < number; i++)
		//	{
		//		result *= i;
		//	}

		//	return result;
		//}

		private List<List<int>> GetCombinaisons(int elementsCount, int processedElements)
		{
			List<List<int>> combinaisons = new List<List<int>>();
			List<int> indices = Enumerable.Range(0, processedElements).ToList();
			combinaisons.Add(new List<int>(indices));

			if (elementsCount == processedElements)
			{
				return combinaisons;
			}

			int i = processedElements - 1;

			while (i != -1)
			{
				indices[i]++;

				for (int j = i + 1; j < processedElements; j++)
				{
					indices[j] = indices[j - 1] + 1;
				}

				if (indices[i] == (elementsCount - processedElements + i))
				{
					i--;
				}
				else
				{
					i = processedElements - 1;
				}

				combinaisons.Add(new List<int>(indices));
			}

			return combinaisons;
		}

		public int GetNthIndex(string str, char c, int nth)
		{
			int count = 0;
			for (int i = 0; i < str.Length; i++)
			{
				if (str[i] == c)
				{
					count++;
					if (count == nth)
					{
						return i;
					}
				}
			}
			return -1;
		}
	}
}
