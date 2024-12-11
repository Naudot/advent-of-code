using System.Text.RegularExpressions;

namespace AdventOfCode.Year2020
{
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
}
