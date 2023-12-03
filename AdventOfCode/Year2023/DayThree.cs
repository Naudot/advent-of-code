using System.Text.RegularExpressions;

namespace AdventOfCode.Year2023
{
	public class DayThree : Day2023
	{
		protected override object ResolveFirstPart(string[] input)
		{
			int sum = 0;

			for (int i = 1; i < input.Length - 1; i++)
			{
				string topLine = input[i - 1];
				string line = input[i];
				string bottomLine = input[i + 1];

				for (int j = 1; j < line.Length - 1; j++)
				{
					char currentChar = line[j];

					if ((currentChar >= '0' && currentChar <= '9') || currentChar == '.')
					{
						continue;
					}

					// Top left
					char topLeftLeft = topLine[j - 2];
					char topLeft = topLine[j - 1];
					char top = topLine[j];
					char topRight = topLine[j + 1];
					char topRightRight = topLine[j + 2];
					char left = line[j - 1];
					char leftLeft = line[j - 2];
					char right = line[j + 1];
					char rightRight = line[j + 2];
					char bottomLeft = bottomLine[j - 1];
					char bottomLeftLeft = bottomLine[j - 2];
					char bottom = bottomLine[j];
					char bottomRight = bottomLine[j + 1];
					char bottomRightRight = bottomLine[j + 2];

					// TopLine
					if (IsNumber(top))
					{
						if (IsNumber(topLeft) && IsNumber(topRight))
						{
							sum += GetSumIn(topLine.Substring(j - 1, 3));
						}
						else if (IsNumber(topLeft))
						{
							sum += GetSumIn(topLine.Substring(j - 2, 3));
						}
						else if (IsNumber(topRight))
						{
							sum += GetSumIn(topLine.Substring(j, 3));
						}
						else
						{
							sum += GetSumIn(topLine.Substring(j, 1));
						}
					}
					else
					{
						if (IsNumber(topLeft))
						{
							if (!IsNumber(topLeftLeft))
							{
								sum += GetSumIn(topLine.Substring(j - 1, 1));
							}
							else
							{
								sum += GetSumIn(topLine.Substring(j - 3, 3));
							}
						}
						if (IsNumber(topRight))
						{
							if (!IsNumber(topRightRight))
							{
								sum += GetSumIn(topLine.Substring(j + 1, 1));
							}
							else
							{
								sum += GetSumIn(topLine.Substring(j + 1, 3));
							}
						}
					}

					// BottomLine
					if (IsNumber(bottom))
					{
						if (IsNumber(bottomLeft) && IsNumber(bottomRight))
						{
							sum += GetSumIn(bottomLine.Substring(j - 1, 3));
						}
						else if (IsNumber(bottomLeft))
						{
							sum += GetSumIn(bottomLine.Substring(j - 2, 3));
						}
						else if (IsNumber(bottomRight))
						{
							sum += GetSumIn(bottomLine.Substring(j, 3));
						}
						else
						{
							sum += GetSumIn(bottomLine.Substring(j, 1));
						}
					}
					else
					{
						if (IsNumber(bottomLeft))
						{
							if (!IsNumber(bottomLeftLeft))
							{
								sum += GetSumIn(bottomLine.Substring(j - 1, 1));
							}
							else
							{
								sum += GetSumIn(bottomLine.Substring(j - 3, 3));
							}
						}
						if (IsNumber(bottomRight))
						{
							if (!IsNumber(bottomRightRight))
							{
								sum += GetSumIn(bottomLine.Substring(j + 1, 1));
							}
							else
							{
								sum += GetSumIn(bottomLine.Substring(j + 1, 3));
							}
						}
					}

					// Left
					if (IsNumber(left))
					{
						if (!IsNumber(leftLeft))
						{
							sum += GetSumIn(line.Substring(j - 1, 1));
						}
						else
						{
							sum += GetSumIn(line.Substring(j - 3, 3));
						}
					}
					// Right
					if (IsNumber(right))
					{
						if (!IsNumber(rightRight))
						{
							sum += GetSumIn(line.Substring(j + 1, 1));
						}
						else
						{
							sum += GetSumIn(line.Substring(j + 1, 3));
						}
					}
				}
			}

			return sum;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			int sum = 0;

			for (int i = 1; i < input.Length - 1; i++)
			{
				string topLine = input[i - 1];
				string line = input[i];
				string bottomLine = input[i + 1];

				for (int j = 1; j < line.Length - 1; j++)
				{
					char currentChar = line[j];

					if (currentChar != '*')
					{
						continue;
					}

					int gearRatio = 1;

					// Top left
					char topLeftLeft = topLine[j - 2];
					char topLeft = topLine[j - 1];
					char top = topLine[j];
					char topRight = topLine[j + 1];
					char topRightRight = topLine[j + 2];
					char left = line[j - 1];
					char leftLeft = line[j - 2];
					char right = line[j + 1];
					char rightRight = line[j + 2];
					char bottomLeft = bottomLine[j - 1];
					char bottomLeftLeft = bottomLine[j - 2];
					char bottom = bottomLine[j];
					char bottomRight = bottomLine[j + 1];
					char bottomRightRight = bottomLine[j + 2];

					int multCounter = 0;

					// TopLine
					if (IsNumber(top))
					{
						if (IsNumber(topLeft) && IsNumber(topRight))
						{
							gearRatio *= GetSumIn(topLine.Substring(j - 1, 3));
							multCounter++;
						}
						else if (IsNumber(topLeft))
						{
							gearRatio *= GetSumIn(topLine.Substring(j - 2, 3));
							multCounter++;
						}
						else if (IsNumber(topRight))
						{
							gearRatio *= GetSumIn(topLine.Substring(j, 3));
							multCounter++;
						}
						else
						{
							gearRatio *= GetSumIn(topLine.Substring(j, 1));
							multCounter++;
						}
					}
					else
					{
						if (IsNumber(topLeft))
						{
							if (!IsNumber(topLeftLeft))
							{
								gearRatio *= GetSumIn(topLine.Substring(j - 1, 1));
								multCounter++;
							}
							else
							{
								gearRatio *= GetSumIn(topLine.Substring(j - 3, 3));
								multCounter++;
							}
						}
						if (IsNumber(topRight))
						{
							if (!IsNumber(topRightRight))
							{
								gearRatio *= GetSumIn(topLine.Substring(j + 1, 1));
								multCounter++;
							}
							else
							{
								gearRatio *= GetSumIn(topLine.Substring(j + 1, 3));
								multCounter++;
							}
						}
					}

					// BottomLine
					if (IsNumber(bottom))
					{
						if (IsNumber(bottomLeft) && IsNumber(bottomRight))
						{
							gearRatio *= GetSumIn(bottomLine.Substring(j - 1, 3));
							multCounter++;
						}
						else if (IsNumber(bottomLeft))
						{
							gearRatio *= GetSumIn(bottomLine.Substring(j - 2, 3));
							multCounter++;
						}
						else if (IsNumber(bottomRight))
						{
							gearRatio *= GetSumIn(bottomLine.Substring(j, 3));
							multCounter++;
						}
						else
						{
							gearRatio *= GetSumIn(bottomLine.Substring(j, 1));
							multCounter++;
						}
					}
					else
					{
						if (IsNumber(bottomLeft))
						{
							if (!IsNumber(bottomLeftLeft))
							{
								gearRatio *= GetSumIn(bottomLine.Substring(j - 1, 1));
								multCounter++;
							}
							else
							{
								gearRatio *= GetSumIn(bottomLine.Substring(j - 3, 3));
								multCounter++;
							}
						}
						if (IsNumber(bottomRight))
						{
							if (!IsNumber(bottomRightRight))
							{
								gearRatio *= GetSumIn(bottomLine.Substring(j + 1, 1));
								multCounter++;
							}
							else
							{
								gearRatio *= GetSumIn(bottomLine.Substring(j + 1, 3));
								multCounter++;
							}
						}
					}

					// Left
					if (IsNumber(left))
					{
						if (!IsNumber(leftLeft))
						{
							gearRatio *= GetSumIn(line.Substring(j - 1, 1));
							multCounter++;
						}
						else
						{
							gearRatio *= GetSumIn(line.Substring(j - 3, 3));
							multCounter++;
						}
					}
					// Right
					if (IsNumber(right))
					{
						if (!IsNumber(rightRight))
						{
							gearRatio *= GetSumIn(line.Substring(j + 1, 1));
							multCounter++;
						}
						else
						{
							gearRatio *= GetSumIn(line.Substring(j + 1, 3));
							multCounter++;
						}
					}

					if (multCounter == 2)
					{
						sum += gearRatio;
					}
				}
			}

			return sum;
		}

		private bool IsNumber(char c)
		{
			return (c >= '0' && c <= '9');
		}

		private int GetSumIn(string substring)
		{
			int sum = 0;

			Match match = Regex.Match(substring, @"(\d+)");
			for (int i = 1; i < match.Groups.Count; i++)
			{
				Group group = match.Groups[i];
				sum += int.Parse(group.Value);
			}

			return sum;
		}
	}
}
