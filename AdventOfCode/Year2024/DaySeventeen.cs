namespace AdventOfCode.Year2024
{
	public class DaySeventeen : Day2024
	{
		protected override bool DeactivateJIT => true;

		protected override object ResolveFirstPart(string[] input)
		{
			int regA = int.Parse(input[0].Split(':')[1].Trim());
			int regB = int.Parse(input[1].Split(':')[1].Trim());
			int regC = int.Parse(input[2].Split(':')[1].Trim());
			List<int> program = input[4].Split(':')[1].Trim().Split(',').Select(val => val[0] - '0').ToList();

			List<int> outProgram = GetOutput(input, regA, regB, regC, program, -1);

			string log = string.Empty;
			for (int i = 0; i < outProgram.Count; i++)
				log += outProgram[i].ToString() + ",";
			return log.Substring(0, log.Length - 1);
		}

		protected override object ResolveSecondPart(string[] input)
		{
			int regA = 0;
			int regB = int.Parse(input[1].Split(':')[1].Trim());
			int regC = int.Parse(input[2].Split(':')[1].Trim());
			List<int> program = input[4].Split(':')[1].Trim().Split(',').Select(val => val[0] - '0').ToList();

			List<int> outProgram = GetOutput(input, regA, regB, regC, program, program.Count);

			//while (!AreListEquals(outProgram, program))
			//{
			//	outProgram = GetOutput(input, regA, regB, regC, program, program.Count);
			//	regA++;
			//}

			return regA;
		}

		private bool AreListEquals(List<int> first, List<int> second)
		{
			if (first.Count != second.Count)
				return false;

			for (int i = 0; i < first.Count; i++)
				if (first[i] != second[i])
					return false;

			return true;
		}

		private List<int> GetOutput(string[] input, int regAStartValue, int regB, int regC, List<int> program, int maxOutputCount)
		{
			int regA = regAStartValue;

			int pointer = 0;
			List<int> outProgram = new();

			while (pointer > -1 && pointer < program.Count)
			{
				int opCode = program[pointer];
				int operand = program[pointer + 1];
				int comboOperand = GetComboOperandValue(operand, regA, regB, regC);

				bool hasModifiedPointer = false;

				// adv : Division
				if (opCode == 0)
				{
					int numerator = regA;
					int denominator = (int)Math.Pow(2, comboOperand);

					regA = numerator / denominator;
				}
				// bxl
				if (opCode == 1)
				{
					regB ^= operand;
				}
				// bst
				if (opCode == 2)
				{
					regB = comboOperand % 8;
				}
				// jnz
				if (opCode == 3)
				{
					if (regA != 0)
					{
						pointer = operand;
						hasModifiedPointer = true;
					}
				}
				// bxc
				if (opCode == 4)
				{
					regB ^= regC;
				}
				// out
				if (opCode == 5)
				{
					int moduled = comboOperand % 8;
					outProgram.Add(moduled);

					if (maxOutputCount != -1 && outProgram.Count > maxOutputCount)
						break;
				}
				// bdv
				if (opCode == 6)
				{
					int numerator = regA;
					int denominator = (int)Math.Pow(2, comboOperand);

					regB = numerator / denominator;
				}
				// cdv
				if (opCode == 7)
				{
					int numerator = regA;
					int denominator = (int)Math.Pow(2, comboOperand);

					regC = numerator / denominator;
				}

				if (!hasModifiedPointer)
					pointer += 2;
			}

			return outProgram;
		}

		private int GetComboOperandValue(int operand, int regA, int regB, int regC)
		{
			if (operand == 7)
				Console.WriteLine("Error");

			if (operand == 4)
				return regA;
			if (operand == 5)
				return regB;
			if (operand == 6)
				return regC;

			return operand;
		}
	}
}
