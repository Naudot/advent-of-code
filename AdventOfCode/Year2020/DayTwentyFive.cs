using System.IO;

namespace AdventOfCode.Year2020
{
	public class DayTwentyFive : Day2020
	{
		private const int MAXIMUM_LOOP_SIZE = 100;

		protected override object ResolveFirstPart()
		{
			string[] input = File.ReadAllLines(GetResourcesPath());
			ulong cardPublicKey = ulong.Parse(input[0]);
			ulong doorPublicKey = ulong.Parse(input[1]);

			// Exemple
			// ulong exempleCardPublicKey = ProcessSubjectNumber(7, 8);
			// ulong exempleDoorublicKey = ProcessSubjectNumber(7, 11);

			// Calls that determined precomputed results

			//ulong cardSubjectNumber = 0;
			//int cardLoopSize = 0;
			//LookForValue(cardPublicKey, out cardSubjectNumber, out cardLoopSize);

			//ulong doorSubjectNumber = 0;
			//int doorLoopSize = 0;
			//LookForValue(doorPublicKey, out doorSubjectNumber, out doorLoopSize);

			// Precomputed results
			ulong cardSubjectNumber = 26810;
			int cardLoopSize = 24;
			ulong doorSubjectNumber = 209765;
			int doorLoopSize = 4;

			return string.Empty;
		}

		protected override object ResolveSecondPart()
		{
			return string.Empty;
		}

		private void LookForValue(ulong lookedValue, out ulong subjectNumber, out int loopSize)
		{
			ulong currentValue = 0;
			subjectNumber = 0;
			loopSize = 0;

			while (currentValue != lookedValue)
			{
				subjectNumber++;

				loopSize = 0;
				for (int i = 0; i < MAXIMUM_LOOP_SIZE; i++)
				{
					loopSize++;
					currentValue = ProcessSubjectNumber(subjectNumber, loopSize);

					if (currentValue == lookedValue)
					{
						break;
					}
				}
			}
		}

		private ulong ProcessSubjectNumber(ulong subjectNumber, int loopSize)
		{
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
