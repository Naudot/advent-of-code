using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Year2023
{
	public class DayThirteen : Day2023
	{
		protected override object ResolveFirstPart(string[] input)
		{
			return GetPatterns(input).Select(pattern => ProcessPattern(pattern)).Sum();
		}

		protected override object ResolveSecondPart(string[] input)
		{
			List<List<string>> patterns = GetPatterns(input);

			return string.Empty;
		}

		private List<List<string>> GetPatterns(string[] input)
		{
			List<List<string>> patterns = new List<List<string>>();
			List<string> pattern = new List<string>();

			for (int i = 0; i < input.Length; i++)
			{
				if (input[i] != string.Empty)
				{
					pattern.Add(input[i]);
				}

				if (input[i] == string.Empty || i == input.Length - 1)
				{
					patterns.Add(new List<string>(pattern));
					pattern.Clear();
				}
			}

			return patterns;
		}

		private int ProcessPattern(List<string> pattern)
		{
			// Row checking
			int rowReflectionsCount = 0;
			int rowIndex = 0;
			for (int i = 0; i < pattern.Count - 1; i++)
			{
				if (pattern[i] == pattern[i + 1])
				{
					int newReflectionsCount = CalculateRowValue(pattern, i);
					if (newReflectionsCount > 0)
					{
						rowReflectionsCount = newReflectionsCount;
						rowIndex = i;
						break;
					}
				}
			}

			// Column checking
			StringBuilder sb = new StringBuilder();
			int columnReflectionsCount = 0;
			int columnIndex = 0;
			for (int i = 0; i < pattern[0].Length - 1; i++)
			{
				string firstColumn = string.Concat(pattern.Select(line => line[i]));
				string secondColumn = string.Concat(pattern.Select(line => line[i + 1]));

				if (firstColumn == secondColumn)
				{
					int newReflectionsCount = CalculateColumnValue(pattern, i);
					if (newReflectionsCount > 0)
					{
						columnReflectionsCount = newReflectionsCount;
						columnIndex = i;
						break;
					}
				}
			}

			if (rowReflectionsCount > columnReflectionsCount)
			{
				return (rowIndex + 1) * 100;
			}

			return columnIndex + 1;
		}

		private int CalculateRowValue(List<string> pattern, int startIndex)
		{
			int numberOfReflections = 1;
			int topIndex = startIndex;
			int bottomIndex = startIndex + 1;

			while (topIndex != -1 && bottomIndex != pattern.Count)
			{
				if (pattern[topIndex] != pattern[bottomIndex])
				{
					return 0;
				}

				topIndex--;
				bottomIndex++;
				numberOfReflections++;
			}

			return numberOfReflections;
		}

		private int CalculateColumnValue(List<string> pattern, int startIndex)
		{
			int numberOfReflections = 1;
			int leftIndex = startIndex;
			int rightIndex = startIndex + 1;

			while (leftIndex != -1 && rightIndex != pattern[0].Length)
			{
				if (string.Concat(pattern.Select(line => line[leftIndex])) 
					!= string.Concat(pattern.Select(line => line[rightIndex])))
				{
					return 0;
				}

				leftIndex--;
				rightIndex++;
				numberOfReflections++;
			}

			return numberOfReflections;
		}
	}
}
