using System.Collections.Generic;
using System.IO;

namespace AdventOfCode.Year2020
{
	public class DayEight : Day2020
	{
		protected override object ResolveFirstPart()
		{
			string[] input = File.ReadAllLines(GetResourcesPath());

			int accumulator;
			ExecuteProgram(input, out accumulator);
			return accumulator;
		}

		protected override object ResolveSecondPart()
		{
			string[] input = File.ReadAllLines(GetResourcesPath());

			int lastInstructionChanged = -1;
			string lastInstructionValue = string.Empty;

			int accumulator;
			while (!ExecuteProgram(input, out accumulator))
			{
				// Revert the last instruction changed
				if (lastInstructionChanged != -1)
				{
					input[lastInstructionChanged] = lastInstructionValue;
				}

				for (int i = lastInstructionChanged + 1; i < input.Length; i++)
				{
					string[] values = input[i].Split(' ');
					string operation = values[0];
					int value = int.Parse(values[1]);

					if (operation == "nop")
					{
						// Save operation changed index
						lastInstructionValue = input[i];
						lastInstructionChanged = i;

						// Modify operation
						operation = "jmp";
						input[i] = operation + " " + value;
						break;
					}
					else if (operation == "jmp")
					{
						// Save operation changed index
						lastInstructionValue = input[i];
						lastInstructionChanged = i;

						// Modify operation
						operation = "nop";
						input[i] = operation + " " + value;
						break;
					}
				}
			}

			return accumulator;
		}

		// Execute a program and check if it has terminated
		private bool ExecuteProgram(string[] input, out int accumulator)
		{
			HashSet<int> instructionVisited = new HashSet<int>();
			accumulator = 0;

			for (int i = 0; i < input.Length; i++)
			{
				if (instructionVisited.Contains(i))
				{
					return false;
				}

				instructionVisited.Add(i);

				string[] values = input[i].Split(' ');
				string operation = values[0];
				int value = int.Parse(values[1]);

				if (operation != "nop")
				{
					if (operation == "acc")
					{
						accumulator += value;
					}
					else if (operation == "jmp")
					{
						i += value - 1;
					}
				}
			}

			return true;
		}
	}
}
