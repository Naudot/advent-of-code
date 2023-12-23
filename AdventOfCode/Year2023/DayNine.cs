namespace AdventOfCode.Year2023
{
	public class DayNine : Day2023
	{
		protected override object ResolveFirstPart(string[] input)
		{
			return input.Select(line => GetInterpolationForLine(line, false)).Sum();
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return input.Select(line => GetInterpolationForLine(line, true)).Sum();
		}

		private int GetInterpolationForLine(string line, bool isBackward)
		{
			int[] diff;
			int[] current = line.Split(' ').Select(term => int.Parse(term)).ToArray();

			List<int[]> arrays = new List<int[]>();
			arrays.Add(current);

			bool isFound = false;
			while (!isFound)
			{
				diff = new int[current.Length - 1];
				for (int j = 0; j < current.Length - 1; j++)
				{
					int first = current[j];
					int second = current[j + 1];
					diff[j] = second - first;
				}

				current = diff;
				arrays.Add(current);
				isFound = !diff.Where(value => value != 0).Any();
			}

			int interpolatedValue = 0;
			if (isBackward)
			{
				for (int j = 1; j < arrays.Count; j++)
				{
					int knownValue = arrays[arrays.Count - 1 - j].First();
					interpolatedValue = knownValue - interpolatedValue;
				}
			}
			else
			{
				interpolatedValue = arrays.Select(array => array.Last()).Sum();
			}

			return interpolatedValue;
		}
	}
}
