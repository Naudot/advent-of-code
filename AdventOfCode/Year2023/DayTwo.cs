namespace AdventOfCode.Year2023
{
	public class DayTwo : Day2023
	{
		protected override object ResolveFirstPart(string[] input)
		{
			int gamePossibleSumID = 0;

			for (int i = 0; i < input.Length; i++)
			{
				string line = input[i];
				string[] split = line.Split(':');
				int id = int.Parse(split[0].Replace("Game ", string.Empty));

				string[] subsets = split[1].Split(';');
				bool isPossible = true;

				for (int j = 0; j < subsets.Length; j++)
				{
					string[] balls = subsets[j].Split(',');
					bool isSubsetGood = true;

					for (int k = 0; k < balls.Length; k++)
					{
						string ball = balls[k];
						bool isWrong = false;

						if (ball.Contains("red"))
						{
							int val = int.Parse(ball.Replace(" red", string.Empty));
							isWrong = val > 12;
						}
						else if (ball.Contains("green"))
						{
							int val = int.Parse(ball.Replace(" green", string.Empty));
							isWrong = val > 13;
						}
						else if (ball.Contains("blue"))
						{
							int val = int.Parse(ball.Replace(" blue", string.Empty));
							isWrong = val > 14;
						}

						if (isWrong)
						{
							isSubsetGood = false;
							break;
						}
					}

					if (!isSubsetGood)
					{
						isPossible = false;
						break;
					}
				}

				if (isPossible)
				{
					gamePossibleSumID += id;
				}
			}

			return gamePossibleSumID;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			int gamePossibleSumID = 0;

			for (int i = 0; i < input.Length; i++)
			{
				string line = input[i];
				string[] split = line.Split(':');
				string[] subsets = split[1].Split(';');

				int minRed = 1;
				int minGreen = 1;
				int minBlue = 1;

				for (int j = 0; j < subsets.Length; j++)
				{
					string[] balls = subsets[j].Split(',');

					for (int k = 0; k < balls.Length; k++)
					{
						string ball = balls[k];

						if (ball.Contains("red"))
						{
							int val = int.Parse(ball.Replace(" red", string.Empty));
							if (minRed < val)
							{
								minRed = val;
							}
						}
						else if (ball.Contains("green"))
						{
							int val = int.Parse(ball.Replace(" green", string.Empty));
							if (minGreen < val)
							{
								minGreen = val;
							}
						}
						else if (ball.Contains("blue"))
						{
							int val = int.Parse(ball.Replace(" blue", string.Empty));
							if (minBlue < val)
							{
								minBlue = val;
							}
						}
					}
				}

				gamePossibleSumID += minRed * minBlue * minGreen;
			}

			return gamePossibleSumID;
		}
	}
}
