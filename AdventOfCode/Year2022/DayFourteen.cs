using System.Text.RegularExpressions;

namespace AdventOfCode.Year2022
{
	public class DayFourteen : Day2022
	{
		public enum State
		{
			AIR = 0,
			ROCK = 1,
			SAND = 2
		}

		protected override object ResolveFirstPart(string[] input)
		{
			return GetSandAmount(input, false);
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return GetSandAmount(input, true);
		}

		private int GetSandAmount(string[] input, bool useFloor)
		{
			// Vertical, Horizontal
			State[,] rocks = new State[10000, 10000];

			int lowestRockPoint = 0;

			for (int i = 0; i < input.Length; i++)
			{
				string rockPath = input[i];
				string[] coords = Regex.Split(rockPath, " -> ");

				for (int j = 1; j < coords.Length; j++)
				{
					string[] startCoord = coords[j - 1].Split(',');
					string[] endCoord = coords[j].Split(',');

					int startX = int.Parse(startCoord[0]);
					int startY = int.Parse(startCoord[1]);

					int endX = int.Parse(endCoord[0]);
					int endY = int.Parse(endCoord[1]);

					// Horizontal
					if (startX != endX)
					{
						int departure = startX < endX ? startX : endX;
						int arrival = startX > endX ? startX : endX;

						for (int k = departure; k <= arrival; k++)
						{
							rocks[startY, k] = State.ROCK;
						}
					}
					// Vertical
					else
					{
						int departure = startY < endY ? startY : endY;
						int arrival = startY > endY ? startY : endY;

						for (int k = departure; k <= arrival; k++)
						{
							rocks[k, startX] = State.ROCK;
						}

						if (departure > lowestRockPoint)
						{
							lowestRockPoint = departure;
						}
						else if (arrival > lowestRockPoint)
						{
							lowestRockPoint = arrival;
						}
					}
				}
			}

			int sandX = 500;
			int sandY = 0;
			int sandsComeToRest = 0;

			int floorY = lowestRockPoint + 2;
			bool wentDownLowestRockPoint = false;

			while ((!wentDownLowestRockPoint && !useFloor) || (rocks[0, 500] != State.SAND && useFloor))
			{
				if (rocks[sandY + 1, sandX] != State.ROCK && rocks[sandY + 1, sandX] != State.SAND && (!useFloor || sandY + 1 != floorY))
				{
					sandY++;
				}
				else if (rocks[sandY + 1, sandX - 1] != State.ROCK && rocks[sandY + 1, sandX - 1] != State.SAND && (!useFloor || sandY + 1 != floorY))
				{
					sandY++;
					sandX--;
				}
				else if (rocks[sandY + 1, sandX + 1] != State.ROCK && rocks[sandY + 1, sandX + 1] != State.SAND && (!useFloor || sandY + 1 != floorY))
				{
					sandY++;
					sandX++;
				}
				else
				{
					rocks[sandY, sandX] = State.SAND;
					sandX = 500;
					sandY = 0;
					sandsComeToRest++;
					continue;
				}

				if (!useFloor && sandY > lowestRockPoint)
				{
					wentDownLowestRockPoint = true;
				}
			}

			return sandsComeToRest;
		}
	}
}
