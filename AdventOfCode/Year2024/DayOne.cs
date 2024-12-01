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
				string line = input[i];
				string[] split = line.Split("   ");

				int leftValue = int.Parse(split[0]);
				int rightValue = int.Parse(split[1]);

				if (leftValues.ContainsKey(leftValue))
					leftValues[leftValue]++;
				else
					leftValues.Add(leftValue, 1);

				if (rightValues.ContainsKey(rightValue))
					rightValues[rightValue]++;
				else
					rightValues.Add(rightValue, 1);
			}

			int similarityScore = 0;

			foreach (KeyValuePair<int, int> leftValuePair in leftValues)
			{
				int leftKey = leftValuePair.Key;
				int countInLeft = leftValuePair.Value;
				int countInRight = rightValues.ContainsKey(leftKey) ? rightValues[leftKey] : 0;
				similarityScore += leftKey * countInLeft * countInRight;
			}

			return similarityScore;
		}
	}
}
