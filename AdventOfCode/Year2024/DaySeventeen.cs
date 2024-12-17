namespace AdventOfCode.Year2024
{
	public class DaySeventeen : Day2024
	{
		private const bool LOG = false;

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
			//long boundMin = 35184372088832;
			//long boundMax = 281474976710656;

			//long boundMin = 235938659532992; // ça me donne pas mal de chiffres
			//long boundMax = 235939733532992; // ça me donne pas mal de chiffres

			// First number : Bound confirmée
			//long boundMin = 211106172088832;
			//long boundMax = 246290672088832;

			// ALORS MAYBE 216557692388832

			// Second number : First bound
			//long boundMin = 215504272088832; // ça me donne pas mal de chiffres
			//long boundMax = 219902332088832; // ça me donne pas mal de chiffres

			// Third number
			//long boundMin = 216122754288832; // ça me donne pas mal de chiffres
			//long boundMax = 216191473888832; // ça me donne pas mal de chiffres

			// Fourth number
			//long boundMin = 216139934188832; // ça me donne pas mal de chiffres
			//long boundMax = 216148524188832; // ça me donne pas mal de chiffres

			// Fifth number
			//long boundMin = 216139934198832; // ça me donne pas mal de chiffres
			//long boundMax = 216141007948832; // ça me donne pas mal de chiffres

			// Sixth number
			//long boundMin = 216139934198832; // ça me donne pas mal de chiffres
			//long boundMax = 216141007948832; // ça me donne pas mal de chiffres

			// Bounds sympa entre 216540511188832 : checker jusqu'à [6] ?
			// et 216558636988832
			//long boundMin = 216540511188832;
			//long boundMax = 216558636988832;
			//long step = 10000;

				// Bounds sympa entre 216549846238832 - 10000 : checker jusqu'à [4]
				// et 216549846238832 + 10000
				long boundMin = 216549846238832 - 10000;
				long boundMax = 216549846238832 + 10000;
				long step = 1;

			// Bounds sympa entre 233714942088832
			// et 246290672088832
			//long boundMin = 233714942088832;

				// Bounds sympa entre 234132697188832 : checker jusqu'à [5]
				// et 234150822988832
				//long boundMin = 234132697188832;
				//long boundMax = 234150822988832;
				//long step = 100000;
				// Pas fructueux ?

					// Bound sympa entre 234142032282832
					// et 234142032284832
					//long boundMin = 234142032282832;
					//long boundMax = 234142032284832;
					//long step = 1;
					// Pas fructueux

			long regB = long.Parse(input[1].Split(':')[1].Trim());
			long regC = long.Parse(input[2].Split(':')[1].Trim());
			List<long> program = input[4].Split(':')[1].Trim().Split(',').Select(val => (long)(val[0] - '0')).ToList();

			for (long i = boundMin; i <= boundMax; i += step)
			{
				List<long> outProgram = GetOutput(input, i, regB, regC, program);

				if (
					outProgram[0] == 2 &&
					outProgram[1] == 4 &&
					outProgram[2] == 1 &&
					outProgram[3] == 3 &&
					outProgram[4] == 7 &&
					outProgram[5] == 5 &&
					outProgram[6] == 1 &&
					outProgram[7] == 5 &&
					outProgram[8] == 0 &&
					outProgram[9] == 3 && 
					outProgram[10] == 4	&&
					outProgram[11] == 1	&&
					outProgram[12] == 5	&&
					outProgram[13] == 5 &&
					outProgram[14] == 3	&&
					outProgram[15] == 0
					)
				{
					string log = string.Empty;
					for (int j = 0; j < outProgram.Count; j++)
						log += outProgram[j].ToString() + ",";

					Console.WriteLine(log.Substring(0, log.Length - 1) + " with length " + log.Length / 2 + " number " + i);
				}
			}

			return -1;
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
					Log("RegA = " + regA + " - " + tmp + " (A) / " + 8);
				}
				// bxl
				if (opCode == 1)
				{
					regB ^= operand;
					Log("RegB = " + regB + " - XOR Operand " + operand);
				}
				// bst
				if (opCode == 2)
				{
					regB = regA % 8; // Instruction 2 uniquement appelée avec Registre A
					Log("RegB = " + regB + " - RegA " + regA + " % 8");
				}
				// jnz
				if (opCode == 3)
				{
					if (regA != 0)
					{
						Log("Reg A != 0, on continue");
						Log(string.Empty);
						pointer = (int)operand;
						hasModifiedPolonger = true;
					}
				}
				// bxc
				if (opCode == 4)
				{
					long tmp = regB;
					regB ^= regC;
					Log("RegB = " + regB + " - " + tmp + " (B) XOR par " + regC + " (C)");
				}
				// out
				if (opCode == 5)
				{
					long moduled = regB % 8; // Instruction 5 uniquement appelée avec Registre B
					outProgram.Add(moduled);
					Log("Sortie : " + moduled + " - RegB " + regB + " modulé par 8");
				}
				// cdv
				if (opCode == 7)
				{
					long denominator = (long)Math.Pow(2, regB); // Instruction 7 uniquement appelée avec Registre B
					regC = regA / denominator;

					Log("RegC = " + regC + " - " + regA + " (A) / " + denominator + " (2 ^ RegB " + regB + ")");
				}

				if (!hasModifiedPolonger)
					pointer += 2;
			}

			return outProgram;
		}

		private void Log(string log)
		{
			if (LOG)
				Console.WriteLine(log);
		}
	}
}
