namespace AdventOfCode.Year2024
{
	public class DaySeventeen : Day2024
	{
		protected override object ResolveFirstPart(string[] input)
		{
			int regA = int.Parse(input[0].Split(':')[1].Trim());
			int regB = int.Parse(input[1].Split(':')[1].Trim());
			int regC = int.Parse(input[2].Split(':')[1].Trim());

			List<int> program = input[4].Split(':')[1].Trim().Split(',').Select(val => val[0] - '0').ToList();

			int pointer = 0;
			string log = string.Empty;

			while (pointer < program.Count)
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
					log += moduled + ",";
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

			return log.Substring(0, log.Length - 1);
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return 0;
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
