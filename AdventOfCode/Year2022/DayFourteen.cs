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

			int currentSandX = 500;
			int currentSandY = 0;
			int sandUnits = 0;

			int floorY = lowestRockPoint + 2;
			bool wentDownLowestRockPoint = false;

			while ((!wentDownLowestRockPoint && !useFloor) || (rocks[0, 500] != State.SAND && useFloor))
			{
				if (rocks[currentSandY + 1, currentSandX] != State.ROCK && rocks[currentSandY + 1, currentSandX] != State.SAND && (!useFloor || currentSandY + 1 != floorY))
				{
					currentSandY++;
				}
				else if (rocks[currentSandY + 1, currentSandX - 1] != State.ROCK && rocks[currentSandY + 1, currentSandX - 1] != State.SAND && (!useFloor || currentSandY + 1 != floorY))
				{
					currentSandY++;
					currentSandX--;
				}
				else if (rocks[currentSandY + 1, currentSandX + 1] != State.ROCK && rocks[currentSandY + 1, currentSandX + 1] != State.SAND && (!useFloor || currentSandY + 1 != floorY))
				{
					currentSandY++;
					currentSandX++;
				}
				else
				{
					rocks[currentSandY, currentSandX] = State.SAND;
					currentSandX = 500;
					currentSandY = 0;
					sandUnits++;
					continue;
				}

				if (!useFloor && currentSandY > lowestRockPoint)
				{
					wentDownLowestRockPoint = true;
				}
			}

			return sandUnits;
		}
	}
}
