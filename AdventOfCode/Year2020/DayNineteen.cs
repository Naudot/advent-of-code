using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2020
{
	public class DayNineteen : Day2020
	{
		private Dictionary<int, string> mDecryptedRules = new Dictionary<int, string>();

		protected override object ResolveFirstPart()
		{
			mDecryptedRules.Clear();

			string input = File.ReadAllText(GetResourcesPath());
			MatchCollection rules = Regex.Matches(input, @"(\d*): (.*)"); ; // TODO split avec | et ensuite espace
			MatchCollection messages = Regex.Matches(input, @"([a-b]+\n)");

			for (int i = 0; i < rules.Count; i++)
			{
				Match rule = rules[i];

				int ruleNumber = int.Parse(rule.Groups[1].Value);
				string ruleDesc = rule.Groups[2].Value;

				if (ruleDesc == "\"a\"")
				{
					mDecryptedRules.Add(ruleNumber, "a");
				}
				else if (ruleDesc == "\"b\"")
				{
					mDecryptedRules.Add(ruleNumber, "b");
				}
				else
				{
					mDecryptedRules.Add(ruleNumber, ruleDesc);
				}
			}

			int result = 0;

			for (int i = 0; i < messages.Count; i++)
			{
				string finalMessage = messages[i].Groups[1].Value.Replace("\n", "");
				int letterMatching = IsMatching(mDecryptedRules[0], finalMessage, 0);
				result += letterMatching == finalMessage.Length ? 1 : 0;
			}

			return result;
		}

		private int IsMatching(string rule, string message, int currentIndex)
		{
			if (rule == "a")
			{
				return message[currentIndex] == 'a' ? 1 : 0;
			}
			if (rule == "b")
			{
				return message[currentIndex] == 'b' ? 1 : 0;
			}

			// Parsing branching rules E.g. : 45 49 | 49 45
			string[] branchingRules = rule.Split('|');

			// Each branching rule has his own index
			int[] localIndexes = new int[branchingRules.Length];

			for (int i = 0; i < branchingRules.Length; i++)
			{
				// We initialize the local index to the current index
				localIndexes[i] = currentIndex;

				// Parsing inner rules E.g. : 45 49
				int[] innerRules = branchingRules[i].TrimStart(' ').TrimEnd(' ').Split(' ').Select(int.Parse).ToArray();

				// We want to know for each inner rule how much indexes it went through, if it is a bad inner rule, it will be 0
				int indexInnerRule = 0;

				for (int j = 0; j < innerRules.Length; j++)
				{
					int indexWentThrough = IsMatching(mDecryptedRules[innerRules[j]], message, localIndexes[i]);

					// Bad branching path
					if (indexWentThrough == 0)
					{
						indexInnerRule = 0;
						break;
					}
					else
					{
						indexInnerRule += indexWentThrough;
						localIndexes[i] += indexWentThrough;
					}
				}

				if (indexInnerRule == 0)
				{
					continue;
				}

				// If inner rules are ok we return the index
				return indexInnerRule;
			}

			return 0; // It means no branching path was correct
		}

		protected override object ResolveSecondPart()
		{
			return string.Empty;
		}
	}
}