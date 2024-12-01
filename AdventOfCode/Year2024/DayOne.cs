namespace AdventOfCode.Year2024
{
	public class DayOne : Day2024
	{
		protected override object ResolveFirstPart(string[] input)
		{
			List<int> left = new();
			List<int> right = new();

			for (int i = 0; i < input.Length; i++)
			{
				string line = input[i];
				string[] split = line.Split("   ");
				left.Add(int.Parse(split[0]));
				right.Add(int.Parse(split[1]));
			}

			left.Sort();
			right.Sort();

			int distance = 0;
			for (int i = 0; i < left.Count; i++)
				distance += Math.Abs(left[i] - right[i]);
			return distance;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			Dictionary<int, int> leftValues = new();
			Dictionary<int, int> rightValues = new();

			for (int i = 0; i < input.Length; i++)
			{
				string[] split = input[i].Split("   ");

				int leftKey = int.Parse(split[0]);
				int rightKey = int.Parse(split[1]);

				leftValues[leftKey] = leftValues.GetValueOrDefault(leftKey, 0) + 1;
				rightValues[rightKey] = rightValues.GetValueOrDefault(rightKey, 0) + 1;
			}

			int similarityScore = 0;
			foreach (KeyValuePair<int, int> leftValuePair in leftValues)
				similarityScore += leftValuePair.Key * leftValuePair.Value * rightValues.GetValueOrDefault(leftKey, 0);
			return similarityScore;
		}
	}
}
