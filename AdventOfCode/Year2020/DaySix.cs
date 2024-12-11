namespace AdventOfCode.Year2020
{
	public class DaySix : Day2020
	{
		protected override object ResolveFirstPart()
		{
			string[] input = File.ReadAllLines(GetResourcesPath());

			int result = 0;
			bool[] cache = new bool[26];

			for (int i = 0; i < input.Length; i++)
			{
				if (input[i].Length == 0)
				{
					Array.Clear(cache, 0, cache.Length);
					continue;
				}

				for (int j = 0; j < input[i].Length; j++)
				{
					int charValue = input[i][j] - 97;
					if (!cache[charValue])
					{
						result++;
						cache[charValue] = true;
					}
				}
			}

			return result;
		}

		protected override object ResolveSecondPart()
		{
			string[] input = File.ReadAllLines(GetResourcesPath());

			int result = 0;
			int[] cache = new int[26];

			int lineNumber = 0;

			for (int i = 0; i < input.Length; i++)
			{
				bool isEndOfBlock = input[i].Length == 0;
				lineNumber += isEndOfBlock ? 0 : 1;

				for (int j = 0; j < input[i].Length; j++)
				{
					cache[input[i][j] - 97] += 1;
				}

				bool isEndOfFile = i + 1 == input.Length;
				if (isEndOfBlock || isEndOfFile)
				{
					for (int j = 0; j < cache.Length; j++)
					{
						if (cache[j] == lineNumber)
						{
							result++;
						}
					}

					Array.Clear(cache, 0, cache.Length);
					lineNumber = 0;
				}
			}

			return result;
		}
	}
}
