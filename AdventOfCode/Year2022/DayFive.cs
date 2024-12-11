using System.Text.RegularExpressions;

namespace AdventOfCode.Year2022
{
	public class DayFive : Day2022
	{
		protected override object ResolveFirstPart(string[] input)
		{
			List<List<char>> stacks = new List<List<char>>();

			int stackCount = 9;
			int height = 8;

			for (int i = 0; i < stackCount; i++)
			{
				stacks.Add(new List<char>());
			}

			for (int i = 0; i < height; i++)
			{
				string line = input[i] + " ";

				for (int j = 0; j < stackCount * 4; j += 4)
				{
					char[] part = line.Skip(j).Take(4).ToArray();
					if (part[1] != ' ')
					{
						stacks[j / 4].Add(part[1]);
					}
				}
			}

			List<Stack<char>> properStacks = new List<Stack<char>>();

			for (int i = 0; i < stacks.Count; i++)
			{
				Stack<char> properStack = new Stack<char>();

				for (int j = 0; j < stacks[i].Count; j++)
				{
					properStack.Push(stacks[i][stacks[i].Count - j - 1]);
				}

				properStacks.Add(properStack);
			}

			for (int i = 10; i < input.Length; i++)
			{
				string instruction = input[i];

				Match match = Regex.Match(instruction, @"move (\d*) from (\d*) to (\d*)");

				int count = int.Parse(match.Groups[1].Value);
				int from = int.Parse(match.Groups[2].Value);
				int to = int.Parse(match.Groups[3].Value);

				for (int j = 0; j < count; j++)
				{
					properStacks[to - 1].Push(properStacks[from - 1].Pop());
				}
			}

			string result = string.Empty;

			for (int i = 0; i < properStacks.Count; i++)
			{
				result += properStacks[i].Peek();
			}

			return result;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			List<List<char>> stacks = new List<List<char>>();

			int stackCount = 9;
			int height = 8;

			for (int i = 0; i < stackCount; i++)
			{
				stacks.Add(new List<char>());
			}

			for (int i = 0; i < height; i++)
			{
				string line = input[i] + " ";

				for (int j = 0; j < stackCount * 4; j += 4)
				{
					char[] part = line.Skip(j).Take(4).ToArray();
					if (part[1] != ' ')
					{
						stacks[j / 4].Add(part[1]);
					}
				}
			}

			List<Stack<char>> properStacks = new List<Stack<char>>();

			for (int i = 0; i < stacks.Count; i++)
			{
				Stack<char> properStack = new Stack<char>();

				for (int j = 0; j < stacks[i].Count; j++)
				{
					properStack.Push(stacks[i][stacks[i].Count - j - 1]);
				}

				properStacks.Add(properStack);
			}

			for (int i = 10; i < input.Length; i++)
			{
				string instruction = input[i];

				Match match = Regex.Match(instruction, @"move (\d*) from (\d*) to (\d*)");

				int count = int.Parse(match.Groups[1].Value);
				int from = int.Parse(match.Groups[2].Value);
				int to = int.Parse(match.Groups[3].Value);

				List<char> toMove = new List<char>();

				for (int j = 0; j < count; j++)
				{
					toMove.Add(properStacks[from - 1].Pop());
				}

				for (int j = 0; j < toMove.Count; j++)
				{
					properStacks[to - 1].Push(toMove[toMove.Count - j - 1]);
				}
			}

			string result = string.Empty;

			for (int i = 0; i < properStacks.Count; i++)
			{
				result += properStacks[i].Peek();
			}

			return result;
		}
	}
}
