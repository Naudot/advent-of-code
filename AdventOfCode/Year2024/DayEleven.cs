namespace AdventOfCode.Year2024
{
	public class DayEleven : Day2024
	{
		protected override object ResolveFirstPart(string[] input)
		{
			return GetSum(input, 25);
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return GetSum(input, 75);
		}

		private long GetSum(string[] input, int blinkCount)
		{
			Dictionary<long, long> stonesMap = input[0]
				.Split(' ')
				.Select(long.Parse)
				.ToDictionary(val => val, val => (long)1);

			for (int i = 0; i < blinkCount; i++)
			{
				Dictionary<long, long> newStonesMap = new();
				foreach (KeyValuePair<long, long> stoneCount in stonesMap)
				{
					if (stoneCount.Key == 0)
					{
						Add(newStonesMap, 1, stoneCount.Value);
					}
					else if (stoneCount.Key.AreDigitsEven(out int digits))
					{
						long power = (long)Math.Pow(10, digits / 2);
						Add(newStonesMap, stoneCount.Key / power, stoneCount.Value);
						Add(newStonesMap, stoneCount.Key % power, stoneCount.Value);
					}
					else
					{
						Add(newStonesMap, stoneCount.Key * 2024, stoneCount.Value);
					}
				}
				stonesMap = new(newStonesMap);
			}

			return stonesMap.Values.Sum();
		}
		
		private static void Add(Dictionary<long, long> dic, long key, long value)
		{
			if (dic.ContainsKey(key))
				dic[key] += value;
			else
				dic.Add(key, value);
		}
	}
}
