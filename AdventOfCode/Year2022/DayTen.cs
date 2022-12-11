using System;

namespace AdventOfCode.Year2022
{
	public class DayTen : Day2022
	{
		protected override object ResolveFirstPart(string[] input)
		{
			int signalStrengthSums = 0;
			int xRegister = 1;
			int currentCycle = 0;

			for (int i = 0; i < input.Length; i++)
			{
				string instr = input[i];

				if (instr == "noop")
				{
					currentCycle++;
					if (currentCycle == 20 || currentCycle == 60 || currentCycle == 100 || currentCycle == 140 || currentCycle == 180 || currentCycle == 220)
					{
						signalStrengthSums += currentCycle * xRegister;
					}
				}
				else
				{
					for (int j = 0; j < 2; j++)
					{
						currentCycle++;
						if (currentCycle == 20 || currentCycle == 60 || currentCycle == 100 || currentCycle == 140 || currentCycle == 180 || currentCycle == 220)
						{
							signalStrengthSums += currentCycle * xRegister;
						}
					}
					xRegister += int.Parse(instr.Split(' ')[1]);
				}
			}

			return signalStrengthSums;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			int xRegister = 1;
			int currentCycle = 0;

			Console.WriteLine();
			for (int i = 0; i < input.Length; i++)
			{
				string instr = input[i];

				if (instr == "noop")
				{
					currentCycle++;
					int cycleTrim = (currentCycle % 40) - 1;
					Console.Write((xRegister == cycleTrim || xRegister - 1 == cycleTrim || xRegister + 1 == cycleTrim) ? "#" : ".");

					if (currentCycle % 40 == 0)
					{
						Console.WriteLine();
					}
				}
				else
				{
					for (int j = 0; j < 2; j++)
					{
						currentCycle++;
						int cycleTrim = (currentCycle % 40) - 1;
						Console.Write((xRegister == cycleTrim || xRegister - 1 == cycleTrim || xRegister + 1 == cycleTrim) ? "#" : ".");

						if (currentCycle % 40 == 0)
						{
							Console.WriteLine();
						}
					}
					xRegister += int.Parse(instr.Split(' ')[1]);
				}
			}

			return 0;
		}
	}
}
