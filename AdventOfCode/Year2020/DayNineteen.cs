using System.Text.RegularExpressions;

namespace AdventOfCode.Year2020
{
	public class DayNineteen : Day2020
	{
		private Dictionary<int, string> mDecryptedRules = new Dictionary<int, string>();
		private HashSet<string> mMatchedStrings = new HashSet<string>();

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

		protected override object ResolveSecondPart()
		{
			mMatchedStrings.Clear();
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

			result += EvaluateResult("42", "42 31", messages);
			result += EvaluateResult("42", "42 42 31 31", messages);
			result += EvaluateResult("42", "42 42 42 31 31 31", messages);
			result += EvaluateResult("42", "42 42 42 42 31 31 31 31", messages);

			result += EvaluateResult("42 42", "42 31", messages);
			result += EvaluateResult("42 42", "42 42 31 31", messages);
			result += EvaluateResult("42 42", "42 42 42 31 31 31", messages);
			result += EvaluateResult("42 42", "42 42 42 42 31 31 31 31", messages);

			result += EvaluateResult("42 42 42", "42 31", messages);
			result += EvaluateResult("42 42 42", "42 42 31 31", messages);
			result += EvaluateResult("42 42 42", "42 42 42 31 31 31", messages);
			result += EvaluateResult("42 42 42", "42 42 42 42 31 31 31 31", messages);

			result += EvaluateResult("42 42 42 42", "42 31", messages);
			result += EvaluateResult("42 42 42 42", "42 42 31 31", messages);
			result += EvaluateResult("42 42 42 42", "42 42 42 31 31 31", messages);
			result += EvaluateResult("42 42 42 42", "42 42 42 42 31 31 31 31", messages);

			result += EvaluateResult("42 42 42 42 42", "42 31", messages);
			result += EvaluateResult("42 42 42 42 42", "42 42 31 31", messages);
			result += EvaluateResult("42 42 42 42 42", "42 42 42 31 31 31", messages);
			result += EvaluateResult("42 42 42 42 42", "42 42 42 42 31 31 31 31", messages);

			// Too much
			// result += EvaluateResult("42 42 42 42 42 42", "42 31", messages);

			// Too much
			// result += EvaluateResult("42", "42 42 42 42 42 31 31 31 31 31", messages);

			return result;
		}

		private int EvaluateResult(string firstRule, string secondRule, MatchCollection messages)
		{
			mDecryptedRules[8] = firstRule;
			mDecryptedRules[11] = secondRule;

			int result = 0;

			for (int i = 0; i < messages.Count; i++)
			{
				string finalMessage = messages[i].Groups[1].Value.Replace("\n", "");

				if (mMatchedStrings.Contains(finalMessage))
				{
					continue;
				}

				int letterMatching = IsMatching(mDecryptedRules[0], finalMessage, 0);
				if (letterMatching == finalMessage.Length)
				{
					mMatchedStrings.Add(finalMessage);
					result++;
				}
			}

			return result;
		}

		private int IsMatching(string rule, string message, int currentIndex)
		{
			if (currentIndex >= message.Length)
			{
				return 0;
			}

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
			int[] innerIndexes = new int[branchingRules.Length];

			for (int i = 0; i < branchingRules.Length; i++)
			{
				// We initialize the local index to the current index
				localIndexes[i] = currentIndex;

				// Parsing inner rules E.g. : 45 49
				int[] innerRules = branchingRules[i].TrimStart(' ').TrimEnd(' ').Split(' ').Select(int.Parse).ToArray();

				// We want to know for each inner rule how much indexes it went through, if it is a bad inner rule, it will be 0
				innerIndexes[i] = 0;

				for (int j = 0; j < innerRules.Length; j++)
				{
					int indexWentThrough = IsMatching(mDecryptedRules[innerRules[j]], message, localIndexes[i]);

					// Bad branching path
					if (indexWentThrough == 0)
					{
						innerIndexes[i] = 0;
						break;
					}
					else
					{
						innerIndexes[i] += indexWentThrough;
						localIndexes[i] += indexWentThrough;
					}
				}

				if (innerIndexes[i] == 0)
				{
					continue;
				}

				// If inner rules are ok we return the index
				return innerIndexes[i];
			}

			return 0; // It means no branching path was correct
		}
	}
}