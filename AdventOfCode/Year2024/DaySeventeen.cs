namespace AdventOfCode.Year2024
{
	public class DaySeventeen : Day2024
	{
		protected override bool DeactivateJIT => true;

		protected override object ResolveFirstPart(string[] input)
		{
			long regA = long.Parse(input[0].Split(':')[1].Trim());
			long regB = long.Parse(input[1].Split(':')[1].Trim());
			long regC = long.Parse(input[2].Split(':')[1].Trim());
			List<long> program = input[4].Split(':')[1].Trim().Split(',').Select(val => (long)(val[0] - '0')).ToList();

			List<long> outProgram = GetOutput(input, regA, regB, regC, program);

			string log = string.Empty;
			for (int i = 0; i < outProgram.Count; i++)
				log += outProgram[i].ToString() + ",";
			return log.Substring(0, log.Length - 1);
		}

		protected override object ResolveSecondPart(string[] input)
		{
			// 35 184 372 088 832 - 1 -> Bound Max
			long regA = 35184372088832 - 1;
			long regB = long.Parse(input[1].Split(':')[1].Trim());
			long regC = long.Parse(input[2].Split(':')[1].Trim());

			List<long> program = input[4].Split(':')[1].Trim().Split(',').Select(val => (long)(val[0] - '0')).ToList();
			List<long> outProgram = GetOutput(input, regA, regB, regC, program);

			string log = string.Empty;
			for (int i = 0; i < outProgram.Count; i++)
				log += outProgram[i].ToString() + ",";
			Console.WriteLine(log.Substring(0, log.Length - 1));
			return log.Substring(0, log.Length - 1);
		}

		private bool AreListEquals(List<long> first, List<long> second)
		{
			if (first.Count != second.Count)
				return false;

			for (int i = 0; i < first.Count; i++)
				if (first[i] != second[i])
					return false;

			return true;
		}

		private List<long> GetOutput(string[] input, long regAStartValue, long regB, long regC, List<long> program)
		{
			// 2,4, 1,3, 7,5, 1,5, 0,3, 4,1, 5,5, 3,0
			// 2,4 -> Dans B : Trois derniers bits du registre A
			// 1,3 -> Dans B : XOR 011 de B
			// 7,5 -> Dans C : Division de A par 2 puissance B
			// 1,5 -> Dans B : XOR 101 de B
			// 0,3 -> Dans A : A divisé par 8
			// 4,1 -> Dans B : XOR B par C
			// 5,5 -> Affichage : Affichage de la valeur des derniers bits de B (modulo 8)
			// 3,0 -> Si A n'est pas égale à 0, on recommence

			long regA = regAStartValue;

			int pointer = 0;
			List<long> outProgram = new();

			while (pointer > -1 && pointer < program.Count)
			{
				long opCode = program[pointer];
				long operand = program[pointer + 1];

				bool hasModifiedPolonger = false;

				// adv : Division
				if (opCode == 0)
				{
					long tmp = regA;
					regA /= 8; // Instruction 0 uniquement appelée avec 3 ce qui donne 8 en dénominateur
					Console.WriteLine("RegA = " + regA + " - " + tmp + " (A) / " + 8);
				}
				// bxl
				if (opCode == 1)
				{
					regB ^= operand;
					Console.WriteLine("RegB = " + regB + " - XOR Operand " + operand);
				}
				// bst
				if (opCode == 2)
				{
					regB = regA % 8; // Instruction 2 uniquement appelée avec Registre A
					Console.WriteLine("RegB = " + regB + " - RegA " + regA + " % 8");
				}
				// jnz
				if (opCode == 3)
				{
					if (regA != 0)
					{
						Console.WriteLine("Reg A != 0, on continue");
						Console.WriteLine();
						pointer = (int)operand;
						hasModifiedPolonger = true;
					}
				}
				// bxc
				if (opCode == 4)
				{
					long tmp = regB;
					regB ^= regC;
					Console.WriteLine("RegB = " + regB + " - " + tmp + " (B) XOR par " + regC + " (C)");
				}
				// out
				if (opCode == 5)
				{
					long moduled = regB % 8; // Instruction 5 uniquement appelée avec Registre B
					outProgram.Add(moduled);
					Console.WriteLine("Sortie : " + moduled + " - RegB " + regB + " modulé par 8");
				}
				// cdv
				if (opCode == 7)
				{
					long denominator = (long)Math.Pow(2, regB); // Instruction 7 uniquement appelée avec Registre B
					regC = regA / denominator;

					Console.WriteLine("RegC = " + regC + " - " + regA + " (A) / " + denominator + " (2 ^ RegB " + regB + ")");
				}

				if (!hasModifiedPolonger)
					pointer += 2;
			}

			return outProgram;
		}
	}
}
