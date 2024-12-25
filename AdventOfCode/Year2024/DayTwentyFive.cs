namespace AdventOfCode.Year2024
{
	public class DayTwentyFive : Day2024
	{
		protected override bool DeactivateJIT
		{
			get
			{
				return true;
			}
		}

		private List<(int, int, int, int, int)> locks = new();
		private List<(int, int, int, int, int)> keys = new();

		protected override object ResolveFirstPart(string[] input)
		{
			for (int objects = 0; objects < input.Length; objects += 8)
			{
				(int, int, int, int, int) values = (0, 0, 0, 0, 0);

				for (int y = 0; y < 7; y++)
				{
					for (int x = 0; x < 5; x++)
					{
						if (input[objects + y][x] == '#')
						{
							if (x == 0)
								values.Item1++;
							if (x == 1)
								values.Item2++;
							if (x == 2)
								values.Item3++;
							if (x == 3)
								values.Item4++;
							if (x == 4)
								values.Item5++;
						}
					}
				}

				if (objects % 8 == 0 && input[objects][0] == '#')
					locks.Add(values);
				else
					keys.Add(values);
			}

			int result = 0;

			for (int i = 0; i < locks.Count; i++)
			{
				(int, int, int, int, int) lockObj = locks[i];

				for (int j = 0; j < keys.Count; j++)
				{
					(int, int, int, int, int) key = keys[j];

					if (lockObj.Item1 + key.Item1 > 7 ||
						lockObj.Item2 + key.Item2 > 7 ||
						lockObj.Item3 + key.Item3 > 7 ||
						lockObj.Item4 + key.Item4 > 7 ||
						lockObj.Item5 + key.Item5 > 7)
						continue;

					result++;
				}
			}

			return result;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return 0;
		}
	}
}
