using System.IO;

namespace AdventOfCode.Year2020
{
	public class DayTwentyFive : Day2020
	{
		private const int SUBJECT_NUMBER = 7;
		private const int MODULO_NUMBER = 20201227;

		protected override object ResolveFirstPart()
		{
			string[] input = File.ReadAllLines(GetResourcesPath());
			ulong cardPublicKey = ulong.Parse(input[0]);
			ulong doorPublicKey = ulong.Parse(input[1]);

			int currentLoopSize = 0;
			int doorLoopSizeTest = 0;
			int cardLoopSizeTest = 0;
			ulong currentValue = 1;
			while (doorLoopSizeTest == 0 || cardLoopSizeTest == 0)
			{
				currentLoopSize++;
				currentValue *= SUBJECT_NUMBER;
				currentValue %= MODULO_NUMBER;

				if (currentValue == cardPublicKey)
				{
					cardLoopSizeTest = currentLoopSize;
				}
				else if (currentValue == doorPublicKey)
				{
					doorLoopSizeTest = currentLoopSize;
				}

				if (doorLoopSizeTest != 0 && cardLoopSizeTest != 0)
				{
					break;
				}
			}

			return ProcessSubjectNumber(cardPublicKey, doorLoopSizeTest);
		}

		protected override object ResolveSecondPart()
		{
			return "gg ez";
		}

		private ulong ProcessSubjectNumber(ulong subjectNumber, int loopSize)
		{
			// Value entre 0 et 20201227 pour les deux public keys
			//

			ulong value = 1;

			for (int i = 0; i < loopSize; i++)
			{
				value *= subjectNumber;
				value %= 20201227;
			}

			return value;
		}
	}
}
