namespace AdventOfCode.Year2024
{
	public class DayNineteen : Day2024
	{
		protected override bool DeactivateJIT => true;

		protected override object ResolveFirstPart(string[] input)
		{
			HashSet<string> towels = input[0].Split(", ").ToHashSet();
			int maxTowelSize = towels.Select(towel => towel.Length).Max();
			int count = 0;

			for (int i = 2; i < input.Length; i++)
			{
				string pattern = input[i];
				if (GetMaxPointer(towels, pattern, 0, maxTowelSize) == pattern.Length)
					count++;
			}

			return count;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return 0;
		}

		private int GetMaxPointer(HashSet<string> towels, string pattern, int pointer, int maxTowelSize)
		{
			//Console.WriteLine("Checking " + pattern.Substring(0, pointer) + " with pointer " + pointer);

			int maxPointer = pointer;

			for (int j = pointer; j < pattern.Length; j++)
			{
				int size = j - pointer + 1;
				string sub = pattern.Substring(pointer, size);

				if (size > maxTowelSize)
					break;

				if (towels.Contains(sub))
				{
					int recursivePointer = GetMaxPointer(towels, pattern, pointer + size, maxTowelSize);

					if (maxPointer < recursivePointer)
						maxPointer = recursivePointer;
					if (maxPointer == pattern.Length)
						return maxPointer;
				}
			}

			return maxPointer;
		}
	}
}
