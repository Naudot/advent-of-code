using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Year2023
{
	public class DayThirteen : Day2023
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
			List<List<string>> patterns = GetPatterns(input);
			for (int i = 0; i < patterns.Count; i++)
			{
				(bool, int) result = ProcessPattern(patterns[i], false, (false, -1));

				if (result.Item1)
				{
					sum += (result.Item2 + 1) * 100;
				}
				else
				{
					sum += (result.Item2 + 1);
				}
			}

			return sum;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			// 31059 too low
			int sum = 0;
			List<List<string>> patterns = GetPatterns(input);

			for (int i = 0; i < patterns.Count; i++)
			{
				List<string> pattern = patterns[i];
				(bool, int) forbiddenValue = ProcessPattern(pattern, false, (false, -1));
				WritePattern(pattern);
				Console.WriteLine("Base value " + i + " : Index : " + forbiddenValue.Item2 + (forbiddenValue.Item1 ? "row" : "column"));

				bool otherPatternFound = false;
				for (int y = 0; y < pattern.Count; y++)
				{
					StringBuilder sb = new StringBuilder(pattern[y]);

					for (int x = 0; x < pattern[y].Length; x++)
					{
						char tmp = sb[x];
						sb[x] = sb[x] == '.' ? '#' : '.';
						pattern[y] = sb.ToString();

						(bool, int) value = ProcessPattern(pattern, true, forbiddenValue);
						if (value.Item2 > -1)
						{
							WritePattern(pattern);
							Console.WriteLine("Pattern " + i + " : " + value.Item2);
							Console.WriteLine("Y: " + y + " X: " + x);

							sum += value.Item1 ? (value.Item2 + 1) * 100 : value.Item2 + 1;

							otherPatternFound = true;
							break;
						}

						sb[x] = tmp;
						pattern[y] = sb.ToString();
					}

					if (otherPatternFound)
					{
						break;
					}
				}
			}

			return sum;
		}

		private void WritePattern(List<string> pattern)
		{
			for (int y = 0; y < pattern.Count; y++)
			{
				Console.WriteLine();
				for (int x = 0; x < pattern[y].Length; x++)
				{
					Console.Write(pattern[y][x]);
				}
			}
			Console.WriteLine();
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

		private (bool, int) ProcessPattern(List<string> pattern, bool useForbidden, (bool, int) forbiddenRefl)
		{
			// Row checking
			int rowReflectionsCount = 0;
			int rowIndex = -1;
			for (int i = 0; i < pattern.Count - 1; i++)
			{
				if (pattern[i] == pattern[i + 1])
				{
					int newReflectionsCount = CalculateRowValue(pattern, i);
					if (newReflectionsCount > 0)
					{
						// It means we are in the forbidden case
						if (useForbidden && forbiddenRefl.Item1 && forbiddenRefl.Item2 == i)
						{
							continue;
						}

						rowReflectionsCount = newReflectionsCount;
						rowIndex = i;
						break;
					}
				}
			}

			// Column checking
			StringBuilder sb = new StringBuilder();
			int columnReflectionsCount = 0;
			int columnIndex = -1;
			for (int i = 0; i < pattern[0].Length - 1; i++)
			{
				string firstColumn = string.Concat(pattern.Select(line => line[i]));
				string secondColumn = string.Concat(pattern.Select(line => line[i + 1]));

				if (firstColumn == secondColumn)
				{
					int newReflectionsCount = CalculateColumnValue(pattern, i);
					if (newReflectionsCount > 0)
					{
						if (useForbidden && !forbiddenRefl.Item1 && forbiddenRefl.Item2 == i)
						{
							continue;
						}

						columnReflectionsCount = newReflectionsCount;
						columnIndex = i;
						break;
					}
				}
			}

			if (rowIndex == -1 && columnIndex == -1)
			{
				return (false, -1);
			}

			if (rowReflectionsCount >= columnReflectionsCount)
			{
				return (true, rowIndex);
			}

			return (false, columnIndex);
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
