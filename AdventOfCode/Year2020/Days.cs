using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2020
{
	public class DayOne : Day2020
	{
		#region Fields

		private HashSet<int> mHashset = new HashSet<int>();
		private Dictionary<int, int> mHashMap = new Dictionary<int, int>();

		#endregion

		#region Methods

		protected override object ResolveFirstPart()
		{
			mHashset.Clear();

			int[] input = File.ReadAllLines(GetResourcesPath()).Select(int.Parse).ToArray();

			for (int i = 0; i < input.Length; i++)
			{
				mHashset.Add(input[i]);
			}

			for (int i = 0; i < input.Length; i++)
			{
				if (mHashset.Contains(2020 - input[i]))
				{
					return input[i] * (2020 - input[i]);
				}
			}

			return 0;
		}

		protected override object ResolveSecondPart()
		{
			mHashMap.Clear();

			int[] input = File.ReadAllLines(GetResourcesPath()).Select(int.Parse).ToArray();

			for (int i = 0; i < input.Length; i++)
			{
				for (int j = i; j < input.Length; j++)
				{
					if (!mHashMap.ContainsKey(input[i] + input[j]))
					{
						mHashMap.Add(input[i] + input[j], input[i] * input[j]);
					}
				}
			}

			for (int i = 0; i < input.Length; i++)
			{
				if (mHashMap.ContainsKey(2020 - input[i]))
				{
					return mHashMap[2020 - input[i]] * input[i];
				}
			}

			return 0;
		}

		#endregion
	}

	public class DayTwo : Day2020
	{
		protected override object ResolveFirstPart()
		{
			string[] input = File.ReadAllLines(GetResourcesPath());

			int result = 0;

			for (int i = 0; i < input.Length; i++)
			{
				Match match = Regex.Match(input[i], @"([\d]*)-([\d]*) ([a-z]): ([a-z]*)");
				int min = int.Parse(match.Groups[1].Value);
				int max = int.Parse(match.Groups[2].Value);
				char letter = char.Parse(match.Groups[3].Value);
				int count = match.Groups[4].Value.Count((arg) => arg == letter);
				if (min <= count && count <= max)
				{
					result++;
				}
			}

			return result;
		}

		protected override object ResolveSecondPart()
		{
			string[] input = File.ReadAllLines(GetResourcesPath());

			int result = 0;

			for (int i = 0; i < input.Length; i++)
			{
				Match match = Regex.Match(input[i], @"([\d]*)-([\d]*) ([a-z]): ([a-z]*)");
				int firstIndex = int.Parse(match.Groups[1].Value);
				int secondIndex = int.Parse(match.Groups[2].Value);
				char letter = char.Parse(match.Groups[3].Value);
				string password = match.Groups[4].Value;
				bool isFirstMatching = password[firstIndex - 1] == letter;
				bool isSecondMatching = password[secondIndex - 1] == letter;
				result += (isFirstMatching ^ isSecondMatching) ? 1 : 0;
			}

			return result;
		}
	}

	public class DayThree : Day2020
	{
		protected override object ResolveFirstPart()
		{
			string[] input = File.ReadAllLines(GetResourcesPath());

			return CalculateTrees(input, 3, 1);
		}

		protected override object ResolveSecondPart()
		{
			string[] input = File.ReadAllLines(GetResourcesPath());

			return 
				CalculateTrees(input, 1, 1) *
				CalculateTrees(input, 3, 1) *
				CalculateTrees(input, 5, 1) *
				CalculateTrees(input, 7, 1) *
				CalculateTrees(input, 1, 2);
		}

		private long CalculateTrees(string[] input, int right, int down)
		{
			int maxWidth = input[0].Length - 1; // We remove the back space
			int maxHeight = input.Length - 1; // We remove the back space
			int currentWidth = 0;
			int currentHeight = 0;

			long result = 0;

			while (currentHeight < maxHeight)
			{
				currentWidth += right;
				currentHeight += down;

				if (currentWidth > maxWidth)
				{
					currentWidth -= (maxWidth + 1); // Backspace consideration
				}

				if (currentHeight <= maxHeight) // We can do an == because the backspace is handled
				{
					result += input[currentHeight][currentWidth] == '#' ? 1 : 0;
				}
			}

			return result;
		}
	}

	public class DayFour : Day2020
	{
		protected override object ResolveFirstPart()
		{
			MatchCollection matchCollection = Regex.Matches(File.ReadAllText(GetResourcesPath()), @"((\S*):(\S*))*");

			int result = 0;

			bool isPreviousEmptyGroup = false;
			bool hasCid = false;
			int fieldCount = 0;

			for (int i = 0; i < matchCollection.Count; i++)
			{
				Match match = matchCollection[i];

				if (match.Groups[2].Value.Length > 0)
				{
					if (match.Groups[2].Value == "cid")
					{
						hasCid = true;
					}
					fieldCount++;
				}

				if (isPreviousEmptyGroup && match.Groups[2].Value.Length == 0)
				{
					result += (fieldCount == 7 && !hasCid) || (fieldCount == 8) ? 1 : 0;
					fieldCount = 0;
					hasCid = false;
					isPreviousEmptyGroup = false;
					continue;
				}

				isPreviousEmptyGroup = match.Groups[2].Value.Length == 0;
			}

			return result;
		}

		protected override object ResolveSecondPart()
		{
			MatchCollection matchCollection = Regex.Matches(File.ReadAllText(GetResourcesPath()), @"((\S*):(\S*))*");

			int result = 0;

			bool isPreviousEmptyGroup = false;
			bool hasCid = false;
			int fieldCount = 0;

			for (int i = 0; i < matchCollection.Count; i++)
			{
				Match match = matchCollection[i];

				bool isInvalid = false;

				string header = match.Groups[2].Value;

				if (header.Length > 0)
				{
					string value = match.Groups[3].Value;
					if (header == "cid")
					{
						hasCid = true;
					}
					if (header == "byr")
					{
						int birthYear;
						isInvalid = value.Length != 4 || !int.TryParse(value, out birthYear) || birthYear < 1920 || birthYear > 2002;
					}
					if (header == "iyr")
					{
						int issueYear;
						isInvalid = value.Length != 4 || !int.TryParse(value, out issueYear) || issueYear < 2010 || issueYear > 2020;
					}
					if (header == "eyr")
					{
						int expirationYear;
						isInvalid = value.Length != 4 || !int.TryParse(value, out expirationYear) || expirationYear < 2020 || expirationYear > 2030;
					}
					if (header == "hgt")
					{
						string mesurement = string.Empty;
						bool isInch = value.Contains("in");
						if (isInch)
						{
							mesurement = value.Replace("in", string.Empty);
						}
						bool isCentimeter = value.Contains("cm");
						if (isCentimeter)
						{
							mesurement = value.Replace("cm", string.Empty);
						}

						int size;
						isInvalid = mesurement == string.Empty || !int.TryParse(mesurement, out size) || (isInch && (size < 59 || size > 76)) || (isCentimeter && (size < 150 || size > 193));
					}
					if (header == "hcl")
					{
						string osef = "0123456789abcdef";
						isInvalid = value.Length != 7 || value[0] != '#'
							|| !osef.Contains(value[1])
							 || !osef.Contains(value[2])
							  || !osef.Contains(value[3])
							   || !osef.Contains(value[4])
								|| !osef.Contains(value[5])
								 || !osef.Contains(value[6]);
					}
					if (header == "ecl")
					{
						isInvalid = value != "amb" && value != "blu" && value != "brn" && value != "gry" && value != "grn" && value != "hzl" && value != "oth";
					}
					if (header == "pid")
					{
						int passportID;
						isInvalid = value.Length != 9 || !int.TryParse(value, out passportID);
					}
					fieldCount++;
				}

				if (isInvalid)
				{
					fieldCount = 0;
					hasCid = false;
					isPreviousEmptyGroup = false;
					continue;
				}

				if (isPreviousEmptyGroup && match.Groups[2].Value.Length == 0)
				{
					result += (fieldCount == 7 && !hasCid) || (fieldCount == 8) ? 1 : 0;
					fieldCount = 0;
					hasCid = false;
					isPreviousEmptyGroup = false;
					continue;
				}

				isPreviousEmptyGroup = match.Groups[2].Value.Length == 0;
			}

			return result;
		}
	}

	public class DayFive : Day2020
	{
		protected override object ResolveFirstPart()
		{
			string[] input = File.ReadAllLines(GetResourcesPath());
			int result = 0;

			for (int i = 0; i < input.Length; i++)
			{
				string seat = input[i];

				int lowerRow = 0;
				int upperRow = 127;
				int lowerColumn = 0;
				int upperColumn = 7;

				for (int j = 0; j < seat.Length; j++)
				{
					if (seat[j] == 'F')
					{
						upperRow -= (int)Math.Pow(2, (6 - j));
					}
					else if (seat[j] == 'B')
					{
						lowerRow += (int)Math.Pow(2, (6 - j));
					}
					else if (seat[j] == 'L')
					{
						upperColumn -= (int)Math.Pow(2, (9 - j));
					}
					else if (seat[j] == 'R')
					{
						lowerColumn += (int)Math.Pow(2, (9 - j));
					}
				}

				int value = upperRow * 8 + upperColumn;
				if (value > result)
				{
					result = value;
				}
			}

			return result;
		}

		protected override object ResolveSecondPart()
		{
			string[] input = File.ReadAllLines(GetResourcesPath());

			bool[] seats = new bool[1024];

			for (int i = 0; i < input.Length; i++)
			{
				string seat = input[i];

				int lowerRow = 0;
				int upperRow = 127;
				int lowerColumn = 0;
				int upperColumn = 7;

				for (int j = 0; j < seat.Length; j++)
				{
					if (seat[j] == 'F')
					{
						upperRow -= (int)Math.Pow(2, (6 - j));
					}
					else if (seat[j] == 'B')
					{
						lowerRow += (int)Math.Pow(2, (6 - j));
					}
					else if (seat[j] == 'L')
					{
						upperColumn -= (int)Math.Pow(2, (9 - j));
					}
					else if (seat[j] == 'R')
					{
						lowerColumn += (int)Math.Pow(2, (9 - j));
					}
				}

				int value = upperRow * 8 + upperColumn;
				seats[value] = true;
			}

			bool hasFoundAtLeastOneOccupiedSeat = false;

			for (int i = 0; i < seats.Length; i++)
			{
				if (!seats[i] && hasFoundAtLeastOneOccupiedSeat)
				{
					return i;
				}
				else if (seats[i])
				{
					hasFoundAtLeastOneOccupiedSeat = true;
				}
			}

			return -1;
		}
	}
}
