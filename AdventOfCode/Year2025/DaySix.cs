using System.Text.RegularExpressions;

namespace AdventOfCode.Year2025
{
	public class DaySix : Day2025
	{
		protected override object ResolveFirstPart(string[] input)
		{
			ulong grandTotal = 0;

			List<ulong> first = TruncateSpace(input[0]).TrimStart().TrimEnd().Split(' ').Select(val => ulong.Parse(val)).ToList();
			List<ulong> second = TruncateSpace(input[1]).TrimStart().TrimEnd().Split(' ').Select(val => ulong.Parse(val)).ToList();
			List<ulong> third = TruncateSpace(input[2]).TrimStart().TrimEnd().Split(' ').Select(val => ulong.Parse(val)).ToList();
			List<ulong> fourth = TruncateSpace(input[3]).TrimStart().TrimEnd().Split(' ').Select(val => ulong.Parse(val)).ToList();
			List<char> operations = TruncateSpace(input[4]).TrimStart().TrimEnd().Split(' ').Select(l_val => l_val[0]).ToList();

			for (int i = 0; i < first.Count; i++)
			{
				if (operations[i] == '*')
					grandTotal += first[i] * second[i] * third[i] * fourth[i];
				else
					grandTotal += first[i] + second[i] + third[i] + fourth[i];
			}

			return grandTotal;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			List<int> blockWidths = GetBlockWidths(input);

			long grandTotal = 0;
			int totalWidth = 0;
			int numberHeight = input.Length - 1;
			List<long> blockNumbers = new();

			for (int i = 0; i < blockWidths.Count; i++)
			{
				char op = input[^1][totalWidth];

				int width = blockWidths[i];
				for (int j = 0; j < width; j++)
				{
					string number = string.Empty;
					for (int k = 0; k < numberHeight; k++)
						if (input[k][totalWidth + j] != ' ')
							number += input[k][totalWidth + j];
					blockNumbers.Add(long.Parse(number));
				}

				if (op == '*')
					grandTotal += blockNumbers.Aggregate((x, y) => (x * y));
				else
					grandTotal += blockNumbers.Sum();

				totalWidth += width + 1;
				blockNumbers.Clear();
			}

			return grandTotal;
		}

		private List<int> GetBlockWidths(string[] input)
		{
			List<int> lengths = new();

			int length = 0;
			for (int i = 0; i < input[0].Length; i++)
			{
				bool isSeparation = true;

				for (int j = 0; j < input.Length - 1; j++)
				{
					if (input[j][i] != ' ')
					{
						length++;
						isSeparation = false;
						break;
					}
				}

				if (isSeparation)
				{
					lengths.Add(length);
					length = 0;
				}
			}

			lengths.Add(length);

			return lengths;
		}

		private string TruncateSpace(string input)
		{
			return Regex.Replace(input, @" +", " ");
		}
	}
}
