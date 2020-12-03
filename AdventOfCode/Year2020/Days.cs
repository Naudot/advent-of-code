using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2020
{
	public class DayOne : Day2020
	{
		#region Fields

		private HashSet<int> mHashset = new HashSet<int>();
		private Dictionary<int, int> mHashMap = new Dictionary<int, int>();

		#endregion

		#region Methods

		protected override object ResolveFirstPart()
		{
			mHashset.Clear();

			int[] input = File.ReadAllLines(GetResourcesPath()).Select(int.Parse).ToArray();

			for (int i = 0; i < input.Length; i++)
			{
				mHashset.Add(input[i]);
			}

			for (int i = 0; i < input.Length; i++)
			{
				if (mHashset.Contains(2020 - input[i]))
				{
					return input[i] * (2020 - input[i]);
				}
			}

			return 0;
		}

		protected override object ResolveSecondPart()
		{
			mHashMap.Clear();

			int[] input = File.ReadAllLines(GetResourcesPath()).Select(int.Parse).ToArray();

			for (int i = 0; i < input.Length; i++)
			{
				for (int j = i; j < input.Length; j++)
				{
					if (!mHashMap.ContainsKey(input[i] + input[j]))
					{
						mHashMap.Add(input[i] + input[j], input[i] * input[j]);
					}
				}
			}

			for (int i = 0; i < input.Length; i++)
			{
				if (mHashMap.ContainsKey(2020 - input[i]))
				{
					return mHashMap[2020 - input[i]] * input[i];
				}
			}

			return 0;
		}

		#endregion
	}

	public class DayTwo : Day2020
	{
		protected override object ResolveFirstPart()
		{
			string[] input = File.ReadAllLines(GetResourcesPath());

			int result = 0;

			for (int i = 0; i < input.Length; i++)
			{
				Match match = Regex.Match(input[i], @"([\d]*)-([\d]*) ([a-z]): ([a-z]*)");
				int min = int.Parse(match.Groups[1].Value);
				int max = int.Parse(match.Groups[2].Value);
				char letter = char.Parse(match.Groups[3].Value);
				int count = match.Groups[4].Value.Count((arg) => arg == letter);
				if (min <= count && count <= max)
				{
					result++;
				}
			}

			return result;
		}

		protected override object ResolveSecondPart()
		{
			string[] input = File.ReadAllLines(GetResourcesPath());

			int result = 0;

			for (int i = 0; i < input.Length; i++)
			{
				Match match = Regex.Match(input[i], @"([\d]*)-([\d]*) ([a-z]): ([a-z]*)");
				int firstIndex = int.Parse(match.Groups[1].Value);
				int secondIndex = int.Parse(match.Groups[2].Value);
				char letter = char.Parse(match.Groups[3].Value);
				string password = match.Groups[4].Value;
				bool isFirstMatching = password[firstIndex - 1] == letter;
				bool isSecondMatching = password[secondIndex - 1] == letter;
				result += (isFirstMatching ^ isSecondMatching) ? 1 : 0;
			}

			return result;
		}
	}

	public class DayThree : Day2020
	{
		protected override object ResolveFirstPart()
		{
			string[] input = File.ReadAllLines(GetResourcesPath());

			return CalculateTrees(input, 3, 1);
		}

		protected override object ResolveSecondPart()
		{
			string[] input = File.ReadAllLines(GetResourcesPath());

			return 
				CalculateTrees(input, 1, 1) *
				CalculateTrees(input, 3, 1) *
				CalculateTrees(input, 5, 1) *
				CalculateTrees(input, 7, 1) *
				CalculateTrees(input, 1, 2);
		}

		private long CalculateTrees(string[] input, int right, int down)
		{
			int maxWidth = input[0].Length - 1; // We remove the back space
			int maxHeight = input.Length - 1; // We remove the back space
			int currentWidth = 0;
			int currentHeight = 0;

			long result = 0;

			while (currentHeight < maxHeight)
			{
				currentWidth += right;
				currentHeight += down;

				if (currentWidth > maxWidth)
				{
					currentWidth -= (maxWidth + 1); // Backspace consideration
				}

				if (currentHeight <= maxHeight) // We can do an == because the backspace is handled
				{
					result += input[currentHeight][currentWidth] == '#' ? 1 : 0;
				}
			}

			return result;
		}
	}
}
