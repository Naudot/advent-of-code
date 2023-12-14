using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2023
{
	public class DayTwelve : Day2023
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
			return input.Select(line => GetArrangementCount(line)).Sum();
		}

		protected override object ResolveSecondPart(string[] input)
		{
			long sum = 0;

			for (int i = 0; i < input.Length; i++)
			{
				if (i == 6)
				{
					continue;
				}

				long val = GetAdvancedArrangementCount(input[i]);
				Console.WriteLine("Input " + i + " is " + val);
				sum += val;
			}

			return sum;
		}

		private int GetArrangementCount(string line)
		{
			string state = line.Split(' ')[0];
			int[] values = line.Split(' ')[1].Split(',').Select(val => int.Parse(val)).ToArray();

			int valSum = values.Sum();
			int knownCount = state.Where(c => c == '#').Count();

			// Only one arrangement is known
			if (knownCount == valSum)
			{
				return 1;
			}

			int unknownCount = state.Where(c => c == '?').Count();
			int toProcess = valSum - knownCount; // We need to replace this amount of ? and check if conditions are met

			int result = 0;
			List<List<int>> combinaisons = GetCombinaisons(unknownCount, toProcess);

			for (int j = 0; j < combinaisons.Count; j++)
			{
				List<int> indexes = combinaisons[j].Select(combiValue => GetNthIndex(state, '?', combiValue + 1)).ToList();
				StringBuilder stringBuilder = new StringBuilder(state);
				for (int k = 0; k < stringBuilder.Length; k++)
				{
					if (stringBuilder[k] == '?')
					{
						stringBuilder[k] = indexes.Contains(k) ? '#' : '.';
					}
				}

				MatchCollection matchCollection = Regex.Matches(stringBuilder.ToString(), @"([#]+)");

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

				result += (matchValues ? 1 : 0);
			}

			return result;
		}

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

		private long GetAdvancedArrangementCount(string line)
		{
			long baseValue = GetArrangementCount(line);

			string state = line.Split(' ')[0];
			state = state + "?" + state;

			//string state2 = line.Split(' ')[0];
			//state2 = "?" + state2/* + state*/;

			string valuesLine = line.Split(' ')[1];
			valuesLine += "," + valuesLine;

			int[] values = valuesLine.Split(',').Select(val => int.Parse(val)).ToArray();

			int valSum = values.Sum();
			int knownCount = state.Where(c => c == '#').Count();

			// Only one arrangement is known
			if (knownCount == valSum)
			{
				return 1;
			}

			int unknownCount = state.Where(c => c == '?').Count();
			int toProcess = valSum - knownCount; // We need to replace this amount of ? and check if conditions are met
			
			int result = GetCombinaisonsCount(unknownCount, toProcess, values, state);
			//int result2 = GetCombinaisonsCount(unknownCount, toProcess, values, state2);

			//result = Math.Max(result, result2);

			//if (result != result2)
			//	Console.WriteLine("State 1 " + result + " State 2 " + result2);

			// 828404064846 too low
			// 1050168883107
			// Les deux lignes à utiliser en dessous quand on a doublé le nombre de values uniquement
			long ratio = result / baseValue;
			return result * ratio * ratio * ratio;

			//return baseValue * result * result * result * result;
		}

		private int GetCombinaisonsCount(int elementsCount, int processedElements, int[] values, string state)
		{
			List<List<int>> combinaisons = new List<List<int>>();
			List<int> indices = Enumerable.Range(0, processedElements).ToList();
			combinaisons.Add(new List<int>(indices));

			int count = GetCountFor(indices, state, values);

			if (elementsCount == processedElements)
			{
				return count;
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

				count += GetCountFor(indices, state, values);
			}

			return count;
		}

		public int GetCountFor(List<int> indices, string state, int[] values)
		{
			List<int> indexes = indices.Select(combiValue => GetNthIndex(state, '?', combiValue + 1)).ToList();
			StringBuilder stringBuilder = new StringBuilder(state);

			for (int k = 0; k < stringBuilder.Length; k++)
			{
				if (stringBuilder[k] == '?')
				{
					stringBuilder[k] = indexes.Contains(k) ? '#' : '.';
				}
			}

			MatchCollection matchCollection = Regex.Matches(stringBuilder.ToString(), @"([#]+)");

			if (values.Length != matchCollection.Count)
			{
				return 0;
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

			return (matchValues ? 1 : 0);
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
