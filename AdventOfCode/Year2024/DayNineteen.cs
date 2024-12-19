namespace AdventOfCode.Year2024
{
	public class DayNineteen : Day2024
	{
		protected override bool DeactivateJIT => true;

		protected override object ResolveFirstPart(string[] input)
		{
			HashSet<string> towels = input[0].Split(", ").ToHashSet();
			int count = 0;

			for (int i = 2; i < input.Length; i++)
			{
				string pattern = input[i];
				if (GetMaxPointer(towels, pattern, 0) == pattern.Length)
					count++;
			}

			return count;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return 0;
		}

		private int GetMaxPointer(HashSet<string> towels, string pattern, int pointer)
		{
			int maxPointer = pointer;

			for (int j = pointer; j < pattern.Length; j++)
			{
				int size = j - pointer + 1;
				string sub = pattern.Substring(pointer, size);
				if (towels.Contains(sub))
				{
					int recursivePointer = GetMaxPointer(towels, pattern, pointer + size);
					if (maxPointer < recursivePointer)
						maxPointer = recursivePointer;
				}
			}

			return maxPointer;
		}
	}
}
