using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2015
{
	public class DayOne : Day2015
	{
		protected override object ResolveFirstPart()
		{
			string input = File.ReadAllText(GetResourcesPath());
			return input.Count(character => character == '(') - input.Count(charactaer => charactaer == ')');
		}

		protected override object ResolveSecondPart()
		{
			char[] input = File.ReadAllText(GetResourcesPath()).ToCharArray();

			int result = 0;

			for (int i = 0; i < input.Length; i++)
			{
				char character = input[i];
				result += character == '(' ? 1 : -1;
				if (result == -1)
				{
					return i + 1;
				}
			}

			return 0;
		}
	}

	public class DayTwo : Day2015
	{
		protected override object ResolveFirstPart()
		{
			int result = 0;
			string[] input = File.ReadAllLines(GetResourcesPath());
			for (int i = 0; i < input.Length; i++)
			{
				string[] dimensions = input[i].Split('x');
				int length = int.Parse(dimensions[0]);
				int width = int.Parse(dimensions[1]);
				int height = int.Parse(dimensions[2]);
				int firstSide = length * width;
				int secondSide = width * height;
				int thirdSide = height * length;
				int lowestSide = Math.Min(Math.Min(firstSide, secondSide), thirdSide);
				result += (lowestSide + 2 * firstSide + 2 * secondSide + 2 * thirdSide);
			}
			return result;
		}

		protected override object ResolveSecondPart()
		{
			int result = 0;
			string[] input = File.ReadAllLines(GetResourcesPath());
			for (int i = 0; i < input.Length; i++)
			{
				string[] dimensions = input[i].Split('x');
				int length = int.Parse(dimensions[0]);
				int width = int.Parse(dimensions[1]);
				int height = int.Parse(dimensions[2]);

				int perimeter = 0;
				if (length >= width && length >= height)
				{
					perimeter = 2 * (width + height);
				}
				else if (width >= height && width >= length)
				{
					perimeter = 2 * (height + length);
				}
				else if (height >= length && height >= width)
				{
					perimeter = 2 * (length + width);
				}

				int bow = height * length * width;

				result += (perimeter + bow);
			}
			return result;
		}
	}

	public class DayThree : Day2015
	{
		protected override object ResolveFirstPart()
		{
			char[] input = File.ReadAllText(GetResourcesPath()).ToCharArray();

			int north = 0;
			int east = 0;

			HashSet<Tuple<int, int>> cases = new HashSet<Tuple<int, int>>();

			cases.Add(new Tuple<int, int>(0, 0));

			for (int i = 0; i < input.Length; i++)
			{
				if (input[i] == '<')
				{
					east -= 1;
				}
				if (input[i] == '>')
				{
					east += 1;
				}
				if (input[i] == '^')
				{
					north += 1;
				}
				if (input[i] == 'v')
				{
					north -= 1;
				}

				if (cases.FirstOrDefault(t => t.Item1 == north && t.Item2 == east) == null)
				{
					cases.Add(new Tuple<int, int>(north, east));
				}
			}

			return cases.Count;
		}

		protected override object ResolveSecondPart()
		{
			char[] input = File.ReadAllText(GetResourcesPath()).ToCharArray();

			int northSanta = 0;
			int eastSanta = 0;

			int northRobotSanta = 0;
			int eastRobotSanta = 0;

			HashSet<Tuple<int, int>> cases = new HashSet<Tuple<int, int>>();

			cases.Add(new Tuple<int, int>(0, 0));

			for (int i = 0; i < input.Length; i++)
			{
				if (input[i] == '<')
				{
					if (i % 2 == 0)
					{
						eastSanta -= 1;
					}
					else
					{
						eastRobotSanta -= 1;
					}
				}
				if (input[i] == '>')
				{
					if (i % 2 == 0)
					{
						eastSanta += 1;
					}
					else
					{
						eastRobotSanta += 1;
					}
				}
				if (input[i] == '^')
				{
					if (i % 2 == 0)
					{
						northSanta += 1;
					}
					else
					{
						northRobotSanta += 1;
					}
				}
				if (input[i] == 'v')
				{
					if (i % 2 == 0)
					{
						northSanta -= 1;
					}
					else
					{
						northRobotSanta -= 1;
					}
				}

				if (i % 2 == 0)
				{
					if (cases.FirstOrDefault(t => t.Item1 == northSanta && t.Item2 == eastSanta) == null)
					{
						cases.Add(new Tuple<int, int>(northSanta, eastSanta));
					}
				}
				else
				{
					if (cases.FirstOrDefault(t => t.Item1 == northRobotSanta && t.Item2 == eastRobotSanta) == null)
					{
						cases.Add(new Tuple<int, int>(northRobotSanta, eastRobotSanta));
					}
				}
			}

			return cases.Count;
		}
	}

	public class DayFour : Day2015
	{
		protected override object ResolveFirstPart()
		{
			MD5 hasher = MD5.Create();
			int result = 0;
			string stringResult = string.Empty;

			// Convert the byte array to hexadecimal string
			StringBuilder sb = new StringBuilder();

			do
			{
				sb.Clear();
				result++;
				byte[] input = Encoding.ASCII.GetBytes("bgvyzdsv" + result);
				byte[] hashedResult = hasher.ComputeHash(input);
				for (int i = 0; i < hashedResult.Length; i++)
				{
					sb.Append(hashedResult[i].ToString("X2"));
				}
				stringResult = sb.ToString();

			} while (stringResult[0] != '0' || stringResult[1] != '0' || stringResult[2] != '0' || stringResult[3] != '0' || stringResult[4] != '0');

			return result;
		}

		protected override object ResolveSecondPart()
		{
			MD5 hasher = MD5.Create();
			int result = 0;
			string stringResult = string.Empty;

			// Convert the byte array to hexadecimal string
			StringBuilder sb = new StringBuilder();

			do
			{
				sb.Clear();
				result++;
				byte[] input = Encoding.ASCII.GetBytes("bgvyzdsv" + result);
				byte[] hashedResult = hasher.ComputeHash(input);
				for (int i = 0; i < hashedResult.Length; i++)
				{
					sb.Append(hashedResult[i].ToString("X2"));
				}
				stringResult = sb.ToString();

			} while (stringResult[0] != '0' || stringResult[1] != '0' || stringResult[2] != '0' || stringResult[3] != '0' || stringResult[4] != '0' || stringResult[5] != '0');

			return result;
		}
	}

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

			for (int i = 0; i < input.Length; i++)
			{
				// First index found of the pair
				Dictionary<Tuple<char, char>, int> pairs = new Dictionary<Tuple<char, char>, int>();
				bool meetConditionOne = false;
				bool meetConditionTwo = false;

				string stringToAnalyze = input[i];
				char previousChar = char.MinValue;
				char secondPreviousChar = char.MinValue;

				for (int j = 0; j < stringToAnalyze.Length; j++)
				{
					char currentChar = stringToAnalyze[j];

					// Pair aggregation
					Tuple<char, char> pair = new Tuple<char, char>(previousChar, currentChar);
					if (!pairs.ContainsKey(pair))
					{
						pairs.Add(pair, j);
					}

					// First condition
					if ((j - pairs[pair]) > 1)
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
			}

			return result;
		}
	}

	public class DaySix : Day2015
	{
		protected override object ResolveFirstPart()
		{
			string[] input = File.ReadAllLines(GetResourcesPath());

			bool[,] lights = new bool[1000, 1000];

			int result = 0;

			for (int i = 0; i < input.Length; i++)
			{
				MatchCollection matches = Regex.Matches(input[i], @"(\d*),(\d*)");

				bool isToggle = input[i].Contains("toggle");
				bool isOn = input[i].Contains("on");

				int beginX = int.Parse(matches[0].Groups[1].Value);
				int beginY = int.Parse(matches[0].Groups[2].Value);
				int endX = int.Parse(matches[1].Groups[1].Value);
				int endY = int.Parse(matches[1].Groups[2].Value);

				for (int j = 0; j <= endX - beginX; j++)
				{
					for (int k = 0; k <= endY - beginY; k++)
					{
						int xIndex = beginX + j;
						int yIndex = beginY + k;

						bool oldValue = lights[xIndex, yIndex];
						if (isToggle)
						{
							lights[xIndex, yIndex] = !lights[xIndex, yIndex];
						}
						else
						{
							lights[xIndex, yIndex] = isOn;
						}
						bool newValue = lights[xIndex, yIndex];
						if (oldValue != newValue)
						{
							result += newValue ? 1 : -1;
						}
					}
				}
			}

			return result;
		}

		protected override object ResolveSecondPart()
		{
			string[] input = File.ReadAllLines(GetResourcesPath());

			int[,] lights = new int[1000, 1000];

			int result = 0;

			for (int i = 0; i < input.Length; i++)
			{
				MatchCollection matches = Regex.Matches(input[i], @"(\d*),(\d*)");

				bool isToggle = input[i].Contains("toggle");
				bool isOn = input[i].Contains("on");

				int beginX = int.Parse(matches[0].Groups[1].Value);
				int beginY = int.Parse(matches[0].Groups[2].Value);
				int endX = int.Parse(matches[1].Groups[1].Value);
				int endY = int.Parse(matches[1].Groups[2].Value);

				for (int j = 0; j <= endX - beginX; j++)
				{
					for (int k = 0; k <= endY - beginY; k++)
					{
						int xIndex = beginX + j;
						int yIndex = beginY + k;

						if (isToggle)
						{
							lights[xIndex, yIndex] += 2;
							result += 2;
						}
						else if (isOn)
						{
							lights[xIndex, yIndex] += 1;
							result += 1;
						}
						else if (lights[xIndex, yIndex] > 0)
						{
							lights[xIndex, yIndex] -= 1;
							result -= 1;
						}
					}
				}
			}

			return result;
		}
	}

	public class DaySeven : Day2015
	{
		protected override object ResolveFirstPart()
		{
			return string.Empty;
		}

		protected override object ResolveSecondPart()
		{
			return string.Empty;
		}
	}
}
