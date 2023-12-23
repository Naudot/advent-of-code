using System.Text;

namespace AdventOfCode.Year2023
{
	public class DayThirteen : Day2023
	{
		protected override object ResolveFirstPart(string[] input)
		{
			int sum = 0;
			List<List<string>> patterns = GetPatterns(input);
			for (int i = 0; i < patterns.Count; i++)
			{
				(bool, int) result = ProcessPattern(patterns[i], false, (false, -1));
				sum += (result.Item2 + 1) * (result.Item1 ? 100 : 1);
			}

			return sum;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			int sum = 0;
			List<List<string>> patterns = GetPatterns(input);

			for (int i = 0; i < patterns.Count; i++)
			{
				List<string> pattern = patterns[i];
				(bool, int) forbiddenValue = ProcessPattern(pattern, false, (false, -1));

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
			// Row check
			for (int i = 0; i < pattern.Count - 1; i++)
			{
				if (pattern[i] == pattern[i + 1] && IsProperRowReflection(pattern, i))
				{
					if (useForbidden && forbiddenRefl.Item1 && forbiddenRefl.Item2 == i)
					{
						continue;
					}

					return (true, i);
				}
			}

			// Column check
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < pattern[0].Length - 1; i++)
			{
				string firstColumn = string.Concat(pattern.Select(line => line[i]));
				string secondColumn = string.Concat(pattern.Select(line => line[i + 1]));

				if (firstColumn == secondColumn && IsProperColumnReflection(pattern, i))
				{
					if (useForbidden && !forbiddenRefl.Item1 && forbiddenRefl.Item2 == i)
					{
						continue;
					}

					return (false, i);
				}
			}

			return (false, -1);
		}

		private bool IsProperRowReflection(List<string> pattern, int startIndex)
		{
			int topIndex = startIndex;
			int bottomIndex = startIndex + 1;

			while (topIndex != -1 && bottomIndex != pattern.Count)
			{
				if (pattern[topIndex] != pattern[bottomIndex])
				{
					return false;
				}

				topIndex--;
				bottomIndex++;
			}

			return true;
		}

		private bool IsProperColumnReflection(List<string> pattern, int startIndex)
		{
			int leftIndex = startIndex;
			int rightIndex = startIndex + 1;

			while (leftIndex != -1 && rightIndex != pattern[0].Length)
			{
				if (string.Concat(pattern.Select(line => line[leftIndex])) 
					!= string.Concat(pattern.Select(line => line[rightIndex])))
				{
					return false;
				}

				leftIndex--;
				rightIndex++;
			}

			return true;
		}
	}
}
