using System.Collections.Generic;

namespace AdventOfCode.Year2015
{
	public class DayEleven : Day2015
	{
		protected override object ResolveFirstPart(string[] input)
		{
			return GetNext("hxbxwxba");
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return GetNext(GetNext("hxbxwxba"));
		}

		private string GetNext(string input)
		{
			// First element is last string element
			List<int> intPass = new List<int>();
			for (int i = 0; i < input.Length; i++)
			{
				intPass.Add(input[input.Length - 1 - i]);
			}

			do
			{
				IncreaseIndex(intPass, 0);
			} while (!MatchRequirements(intPass));

			string result = string.Empty;
			for (int i = 0; i < intPass.Count; i++)
			{
				result += (char)intPass[intPass.Count - 1 - i];
			}
			return result;
		}

		private void IncreaseIndex(List<int> values, int index)
		{
			for (int i = index; i < values.Count; i++)
			{
				values[i]++;

				if (values[i] > 'z')
				{
					values[i] = 'a' - 1;
					IncreaseIndex(values, i);
				}
				else
				{
					break;
				}
			}
		}

		private bool MatchRequirements(List<int> pass)
		{
			if (pass.Contains('i') || pass.Contains('o') || pass.Contains('l'))
			{
				return false;
			}

			int numberOfPairs = 0;

			for (int i = 0; i < pass.Count - 1; i++)
			{
				int value = pass[i];
				int nextValue = pass[i + 1];
				if (nextValue == value)
				{
					numberOfPairs++;
					i++;
				}
			}

			if (numberOfPairs < 2)
			{
				return false;
			}

			bool hasThreeStraight = false;

			for (int i = 0; i < pass.Count - 2; i++)
			{
				int value = pass[i];
				int nextValue = pass[i + 1];
				int nextNextValue = pass[i + 2];
				if (value == (nextValue + 1) && value == (nextNextValue +2))
				{
					hasThreeStraight = true;
					break;
				}
			}

			if (!hasThreeStraight)
			{
				return false;
			}

			return true;
		}
	}
}
