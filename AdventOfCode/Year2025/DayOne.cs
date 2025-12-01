namespace AdventOfCode.Year2025
{
	public class DayOne : Day2025
	{
		protected override object ResolveFirstPart(string[] input)
		{
			int dialToZeroCount = 0;
			int dial = 50;

			for (int i = 0; i < input.Length; i++)
			{
				string rotationText = input[i];
				bool isNegative = rotationText[0] == 'L';
				int rotationValue = int.Parse(rotationText[1..]);

				dial += (isNegative ? -rotationValue : rotationValue);

				while (dial > 99)
					dial -= 100;
				while (dial < 0)
					dial += 100;

				dialToZeroCount += (dial == 0 ? 1 : 0);
			}

			return dialToZeroCount;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			int dialToZeroCount = 0;
			int dial = 50;

			for (int i = 0; i < input.Length; i++)
			{
				string rotationText = input[i];
				bool isNegative = rotationText[0] == 'L';
				int rotationValue = int.Parse(rotationText[1..]);
				int finalRotationValue = (isNegative ? -rotationValue : rotationValue);

				bool l_isDialZero = dial == 0;

				dial += finalRotationValue;

				while (dial > 99)
				{
					dial -= 100;
					if (dial != 0)
						dialToZeroCount++;
				}

				while (dial < 0)
				{
					if (l_isDialZero)
					{
						l_isDialZero = false;
					}
					else
					{
						if (dial != 0)
							dialToZeroCount++;
					}

					dial += 100;
				}

				dialToZeroCount += (dial == 0 ? 1 : 0);
			}

			return dialToZeroCount;
		}
	}
}
