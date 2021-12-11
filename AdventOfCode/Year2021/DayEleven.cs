using System;
using System.Collections.Generic;

namespace AdventOfCode.Year2021
{
	public class DayEleven : Day2021
	{
		protected override object ResolveFirstPart(string[] input)
		{
			return ResolveOctopus(input, 100).flashCount;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return ResolveOctopus(input).stepSync;
		}

		private (int flashCount, int stepSync) ResolveOctopus(string[] input, int step = -1)
		{
			int stepSync = -1;
			int height = input.Length;
			int width = input[0].Length;

			int[,] octopus = new int[height, width];

			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					octopus[i, j] = int.Parse(input[i][j].ToString());
				}
			}

			HashSet<int> HasFlashed = new HashSet<int>();
			int stepCount = 0;
			int flashCount = 0;
			while ((step != -1 && stepCount != step) || (step == -1 && stepSync == -1))
			{
				HasFlashed.Clear();

				// First, the energy level of each octopus increases by 1.
				for (int i = 0; i < height; i++)
				{
					for (int j = 0; j < width; j++)
					{
						octopus[i, j]++;
					}
				}

				//Console.WriteLine("Step " + (stepCount + 1));
				//Console.WriteLine("Before flashes");

				// Then, any octopus with an energy level greater than 9 flashes. 
				// This increases the energy level of all adjacent octopuses by 1, including octopuses that are diagonally adjacent. 
				// If this causes an octopus to have an energy level greater than 9, it also flashes. 
				// This process continues as long as new octopuses keep having their energy level increased beyond 9. 
				// (An octopus can only flash at most once per step.)
				int localFlashCount;
				do
				{
					localFlashCount = 0;
					for (int i = 0; i < height; i++)
					{
						for (int j = 0; j < width; j++)
						{
							int hashedCoord = i * 256 + j;
							if (octopus[i, j] > 9 && !HasFlashed.Contains(hashedCoord))
							{
								// Can not flash two times
								HasFlashed.Add(hashedCoord);
								flashCount++;
								localFlashCount++;

								if (i > 0 && j > 0)
								{
									octopus[i - 1, j - 1]++; // Top Left
								}
								if (i > 0)
								{
									octopus[i - 1, j]++; // Top
								}
								if (i > 0 && j < (width - 1))
								{
									octopus[i - 1, j + 1]++; // Top Right
								}
								if (j > 0)
								{
									octopus[i, j - 1]++; // Left
								}
								if (j < (width - 1))
								{
									octopus[i, j + 1]++; // Right
								}
								if (i < (height - 1) && j > 0)
								{
									octopus[i + 1, j - 1]++; // Bottom Left
								}
								if (i < (height - 1))
								{
									octopus[i + 1, j]++; // Bottom
								}
								if (i < (height - 1) && j < (width - 1))
								{
									octopus[i + 1, j + 1]++; // Bottom Right
								}
							}
							//Console.Write(octopus[i, j] + " ");
						}
						//Console.WriteLine();
					}
				} while (localFlashCount != 0);

				//Console.WriteLine("After flashes");

				bool allFlashed = true;
				// Finally, any octopus that flashed during this step has its energy level set to 0, as it used all of its energy to flash.
				for (int i = 0; i < height; i++)
				{
					for (int j = 0; j < width; j++)
					{
						if (octopus[i, j] > 9)
						{
							octopus[i, j] = 0;
						}
						else
						{
							allFlashed = false;
						}
						//Console.Write(octopus[i, j]);
					}
					//Console.WriteLine();
				}

				stepCount++;

				if (allFlashed && stepSync == -1)
				{
					stepSync = stepCount;
				}
			}

			return (flashCount, stepSync);
		}
	}
}
