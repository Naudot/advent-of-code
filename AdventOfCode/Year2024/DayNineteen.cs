namespace AdventOfCode.Year2024
{
	public class DayNineteen : Day2024
	{
		protected override bool DeactivateJIT => true;

		private Dictionary<int, long> memoise = new();

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
			HashSet<string> towels = input[0].Split(", ").ToHashSet();
			int maxTowelSize = towels.Select(towel => towel.Length).Max();
			long count = 0;

			for (int i = 2; i < input.Length; i++)
			{
				memoise.Clear();
				count += GetPatternCount(towels, input[i], 0, 0);
			}

			return count;
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

		private long GetPatternCount(HashSet<string> towels, string pattern, int pointer, int depth)
		{
			// Si là où on est positionné dans le pattern, il y a déjà une solution, on l'utilise
			if (memoise.ContainsKey(pointer))
				return memoise[pointer];

			long finalCount = 0;

			// Pour la taille du pattern
			for (int j = pointer; j < pattern.Length; j++)
			{
				Console.WriteLine(j + " " + depth);

				int size = j - pointer + 1;
				string sub = pattern.Substring(pointer, size);

				// Dès qu'on trouve un sous-pattern correspondant à une serviette
				if (towels.Contains(sub))
				{
					// On compte toutes les possibilités des sous patterns
					finalCount += GetPatternCount(towels, pattern, pointer + size, depth + 1);

					// Si on atteint la fin du pattern, on peut dire qu'on a trouvé une possibilité
					// Et on la mémoise.
					if (pointer + size == pattern.Length)
					{
						if (!memoise.ContainsKey(pointer))
							memoise.Add(pointer, finalCount + 1);

						return finalCount + 1;
					}
				}
			}

			if (!memoise.ContainsKey(pointer))
				memoise.Add(pointer, finalCount);

			return finalCount;
		}
	}
}
