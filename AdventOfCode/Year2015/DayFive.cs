namespace AdventOfCode.Year2015
{
	public class DayFive : Day2015
	{
		protected override object ResolveFirstPart()
		{
			string[] input = File.ReadAllLines(GetResourcesPath());

			char[] vowels = new char[] { 'a', 'e', 'i', 'o', 'u' };

			int result = 0;

			for (int i = 0; i < input.Length; i++)
			{
				int foundVowels = 0;
				char previousChar = char.MinValue;

				bool meetConditionTwo = false;
				bool meetConditionThree = true;

				string stringToAnalyze = input[i];

				for (int j = 0; j < stringToAnalyze.Length; j++)
				{
					char currentChar = stringToAnalyze[j];

					if (vowels.Contains(currentChar))
					{
						foundVowels += 1;
					}

					if (previousChar == currentChar)
					{
						meetConditionTwo = true;
					}

					if ((previousChar == 'a' && currentChar == 'b') ||
						(previousChar == 'c' && currentChar == 'd') ||
						(previousChar == 'p' && currentChar == 'q') ||
						(previousChar == 'x' && currentChar == 'y'))
					{
						meetConditionThree = false;
						break;
					}

					previousChar = currentChar;
				}

				bool meetConditionOne = foundVowels >= 3;

				result += meetConditionOne && meetConditionTwo && meetConditionThree ? 1 : 0;
			}

			return result;
		}

		protected override object ResolveSecondPart()
		{
			string[] input = File.ReadAllLines(GetResourcesPath());

			int result = 0;

			int[,] objects = new int[26, 26];
			int[] indexes = new int[input[0].Length];

			for (int i = 0; i < input.Length; i++)
			{
				bool meetConditionOne = false;
				bool meetConditionTwo = false;

				string stringToAnalyze = input[i];
				char previousChar = stringToAnalyze[0];
				char secondPreviousChar = char.MinValue;

				for (int j = 1; j < stringToAnalyze.Length; j++)
				{
					char currentChar = stringToAnalyze[j];

					if (objects[previousChar - 97, currentChar - 97] == 0)
					{
						objects[previousChar - 97, currentChar - 97] = j + 1; // The +1 is to avoid working with 0s
						indexes[j] = (previousChar - 97) * 100 + currentChar - 97;
					}

					// First condition
					int value = objects[previousChar - 97, currentChar - 97];
					if (value != 0 && ((j + 1) - value) > 1)
					{
						meetConditionOne = true;
					}

					// Second condition
					if (secondPreviousChar == currentChar)
					{
						meetConditionTwo = true;
					}

					if (meetConditionOne && meetConditionTwo)
					{
						result += 1;
						break;
					}

					secondPreviousChar = previousChar;
					previousChar = currentChar;
				}

				for (int j = 0; j < indexes.Length; j++)
				{
					int value = indexes[j];
					objects[value / 100, value % 100] = 0;
					indexes[j] = 0;
				}
			}

			return result;
		}
	}
}
