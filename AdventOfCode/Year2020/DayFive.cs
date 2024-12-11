namespace AdventOfCode.Year2020
{
	public class DayFive : Day2020
	{
		protected override object ResolveFirstPart()
		{
			string[] input = File.ReadAllLines(GetResourcesPath());
			int result = 0;

			for (int i = 0; i < input.Length; i++)
			{
				string seat = input[i];

				int upperRow = 127;
				int upperColumn = 7;

				// Not optimized and ugly method I first tried
				for (int j = 0; j < seat.Length; j++)
				{
					if (seat[j] == 'F')
					{
						upperRow -= (int)Math.Pow(2, (6 - j));
					}
					else if (seat[j] == 'L')
					{
						upperColumn -= (int)Math.Pow(2, (9 - j));
					}
				}

				int value = upperRow * 8 + upperColumn;
				if (value > result)
				{
					result = value;
				}
			}

			return result;
		}

		protected override object ResolveSecondPart()
		{
			string[] input = File.ReadAllLines(GetResourcesPath());

			int[] seats = new int[input.Length];

			for (int i = 0; i < input.Length; i++)
			{
				string seat = input[i];

				int row = 0;
				int column = 0;

				for (int j = 0; j < seat.Length; j++)
				{
					if (j <= 6)
					{
						row <<= 1;
					}
					else
					{
						column <<= 1;
					}

					if (seat[j] == 'B')
					{
						row |= 1;
					}
					else if (seat[j] == 'R')
					{
						column |= 1;
					}
				}

				seats[i] = row * 8 + column;
			}

			Array.Sort(seats);

			for (int i = 0; i < seats.Length; i++)
			{
				if (seats[i] - seats[i + 1] < -1)
				{
					return seats[i + 1] - 1;
				}
			}

			return -1;
		}
	}
}
