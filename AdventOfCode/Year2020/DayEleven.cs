namespace AdventOfCode.Year2020
{
	public class DayEleven : Day2020
	{
		protected override object ResolveFirstPart()
		{
			string[] input = File.ReadAllLines(GetResourcesPath());

			int height = input.Length;
			int width = input[0].Length;

			int[,] inputs = new int[height, width];

			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					inputs[i, j] = input[i][j] == '.' ? 0 : 1;
				}
			}

			int newUsedSeats = 0;
			List<string> changedSeats = new List<string>();

			int totalUsedSeats;
			do
			{
				changedSeats.Clear();
				totalUsedSeats = newUsedSeats;

				for (int i = 0; i < height; i++)
				{
					for (int j = 0; j < width; j++)
					{
						if (inputs[i, j] == 0)
						{
							continue;
						}

						int aroundUsedSeats = GetUsedSeatsCount(inputs, i, j, width, height);

						if (inputs[i, j] == 1 && aroundUsedSeats == 0)
						{
							changedSeats.Add("i," + i + ",j," + j + ",2");
						}

						if (inputs[i, j] == 2 && aroundUsedSeats >= 4)
						{
							changedSeats.Add("i," + i + ",j," + j + ",1");
						}
					}
				}

				for (int i = 0; i < changedSeats.Count; i++)
				{
					string[] seat = changedSeats[i].Split(',');
					int newValue = int.Parse(seat[4]);
					int heightValue = int.Parse(seat[1]);
					int widthValue = int.Parse(seat[3]);
					inputs[heightValue, widthValue] = newValue;
					newUsedSeats += newValue == 2 ? 1 : -1;
				}

			} while (totalUsedSeats != newUsedSeats);

			return totalUsedSeats;
		}

		protected override object ResolveSecondPart()
		{
			string[] input = File.ReadAllLines(GetResourcesPath());

			int height = input.Length;
			int width = input[0].Length;

			int[,] inputs = new int[height, width];

			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					inputs[i, j] = input[i][j] == '.' ? 0 : (input[i][j] == 'L' ? 1 : 2);
				}
			}

			int newUsedSeats = 0;
			List<string> changedSeats = new List<string>();

			int totalUsedSeats;
			do
			{
				changedSeats.Clear();
				totalUsedSeats = newUsedSeats;

				for (int i = 0; i < height; i++)
				{
					for (int j = 0; j < width; j++)
					{
						if (inputs[i, j] == 0)
						{
							continue;
						}

						int aroundUsedSeats = GetAdvancedUsedSeatsCount(inputs, i, j, width, height);

						if (inputs[i, j] == 1 && aroundUsedSeats == 0)
						{
							changedSeats.Add("i," + i + ",j," + j + ",2");
						}

						if (inputs[i, j] == 2 && aroundUsedSeats >= 5)
						{
							changedSeats.Add("i," + i + ",j," + j + ",1");
						}
					}
				}

				for (int i = 0; i < changedSeats.Count; i++)
				{
					string[] seat = changedSeats[i].Split(',');
					int newValue = int.Parse(seat[4]);
					int heightValue = int.Parse(seat[1]);
					int widthValue = int.Parse(seat[3]);
					inputs[heightValue, widthValue] = newValue;
					newUsedSeats += newValue == 2 ? 1 : -1;
				}

			} while (totalUsedSeats != newUsedSeats);

			return totalUsedSeats;
		}

		private int GetUsedSeatsCount(int[,] inputs, int i, int j, int width, int height)
		{
			int usedSeat = 0;

			if (j != 0 && inputs[i, j - 1] == 2) // Left place
			{
				usedSeat++;
			}
			if (j != width - 1 && inputs[i, j + 1] == 2) // Right place
			{
				usedSeat++;
			}
			if (i != 0 && inputs[i - 1, j] == 2) // Top place
			{
				usedSeat++;
			}
			if (i != height - 1 && inputs[i + 1, j] == 2) // Bottom place
			{
				usedSeat++;
			}
			if (i != 0 && j != 0 && inputs[i - 1, j - 1] == 2) // Top left
			{
				usedSeat++;
			}
			if (i != 0 && j != width - 1 && inputs[i - 1, j + 1] == 2) // Top right
			{
				usedSeat++;
			}
			if (i != height - 1 && j != 0 && inputs[i + 1, j - 1] == 2) // Bottom left
			{
				usedSeat++;
			}
			if (i != height - 1 && j != width - 1 && inputs[i + 1, j + 1] == 2) // Bottom right
			{
				usedSeat++;
			}

			return usedSeat;
		}

		private int GetAdvancedUsedSeatsCount(int[,] inputs, int i, int j, int width, int height)
		{
			int usedSeat = 0;

			for (int k = 1; k < j + 1; k++) // Left place
			{
				if (inputs[i, j - k] == 2)
				{
					usedSeat++;
					break;
				}
				if (inputs[i, j - k] == 1)
				{
					break;
				}
			}
			for (int k = 1; k < width - j; k++) // Right place
			{
				if (inputs[i, j + k] == 2)
				{
					usedSeat++;
					break;
				}
				if (inputs[i, j + k] == 1)
				{
					break;
				}
			}
			for (int k = 1; k < i + 1; k++) // Top place
			{
				if (inputs[i - k, j] == 2)
				{
					usedSeat++;
					break;
				}
				if (inputs[i - k, j] == 1)
				{
					break;
				}
			}
			for (int k = 1; k < height - i; k++) // Bottom place
			{
				if (inputs[i + k, j] == 2)
				{
					usedSeat++;
					break;
				}
				if (inputs[i + k, j] == 1)
				{
					break;
				}
			}
			int topLeft = Math.Min(i, j);
			for (int k = 1; k < topLeft + 1; k++) // Top left place
			{
				if (inputs[i - k, j - k] == 2)
				{
					usedSeat++;
					break;
				}
				if (inputs[i - k, j - k] == 1)
				{
					break;
				}
			}

			int topRight = Math.Min(i, width - j - 1);
			for (int k = 1; k < topRight + 1; k++) // Top right place
			{
				if (inputs[i - k, j + k] == 2)
				{
					usedSeat++;
					break;
				}
				if (inputs[i - k, j + k] == 1)
				{
					break;
				}
			}

			int bottomLeft = Math.Min(height - i - 1, j);
			for (int k = 1; k < bottomLeft + 1; k++) // Bottom left place
			{
				if (inputs[i + k, j - k] == 2)
				{
					usedSeat++;
					break;
				}
				if (inputs[i + k, j - k] == 1)
				{
					break;
				}
			}

			int bottomRight = Math.Min(height - i - 1, width - j - 1);
			for (int k = 1; k < bottomRight + 1; k++) // Bottom right place
			{
				if (inputs[i + k, j + k] == 2)
				{
					usedSeat++;
					break;
				}
				if (inputs[i + k, j + k] == 1)
				{
					break;
				}
			}

			return usedSeat;
		}
	}
}
