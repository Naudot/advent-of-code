namespace AdventOfCode.Year2021
{
	public class DaySeventeen : Day2021
	{
		protected override object ResolveFirstPart(string[] input)
		{
			int xMin = 257;
			int xMax = 286;
			int yMin = -101;
			int yMax = -57;

			Probe probe = new Probe()
			{
				VelX = 0,
				VelY = 0
			};

			List<int> foundY = new List<int>();

			for (int i = 0; i < 100; i++)
			{
				for (int j = 0; j < 1000; j++)
				{
					List<int> yPositionsReached = new List<int>();
					probe.VelX = i;
					probe.VelY = j;
					probe.PosX = 0;
					probe.PosY = 0;

					while (probe.PosX <= xMax && probe.PosY >= yMin)
					{
						probe.PosX += probe.VelX;
						probe.PosY += probe.VelY;
						yPositionsReached.Add(probe.PosY);

						if (probe.VelX > 0)
						{
							probe.VelX--;
						}
						probe.VelY--;

						if (probe.PosX >= xMin && probe.PosX <= xMax
							&& probe.PosY >= yMin && probe.PosY <= yMax)
						{
							foundY.AddRange(yPositionsReached);
						}
					}
				}
			}

			return foundY.Max();
		}

		protected override object ResolveSecondPart(string[] input)
		{
			int xMin = 257;
			int xMax = 286;
			int yMin = -101;
			int yMax = -57;

			//xMin = 20;
			//xMax = 30;
			//yMin = -10;
			//yMax = -5;

			Probe probe = new Probe()
			{
				VelX = 0,
				VelY = 0
			};

			List<(int, int)> foundVelocities = new List<(int, int)>();

			for (int i = 0; i < xMax + 1; i++)
			{
				for (int j = (yMin - 1); j < Math.Abs(yMin); j++)
				{
					probe.VelX = i;
					probe.VelY = j;
					probe.PosX = 0;
					probe.PosY = 0;

					while (probe.PosX <= xMax && probe.PosY >= yMin)
					{
						probe.PosX += probe.VelX;
						probe.PosY += probe.VelY;

						if (probe.VelX > 0)
						{
							probe.VelX--;
						}
						probe.VelY--;

						if (probe.PosX >= xMin && probe.PosX <= xMax
							&& probe.PosY >= yMin && probe.PosY <= yMax)
						{
							if (!foundVelocities.Contains((i, j)))
							{
								foundVelocities.Add((i, j));
							}

							break;
						}
					}
				}
			}

			return foundVelocities.Count();
		}

		public class Probe
		{
			public int PosX;
			public int PosY;
			public int VelX;
			public int VelY;
		}
	}
}
