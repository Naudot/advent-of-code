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
}
