using System.Text.RegularExpressions;

namespace AdventOfCode.Year2023
{
	public class DayEight : Day2023
	{
		protected override object ResolveFirstPart(string[] input)
		{
			Dictionary<string, (string, string)> values = GetValues(input);

			string instr = input[0];
			string currentKey = "AAA";
			int step = 0;
			while (currentKey != "ZZZ")
			{
				char next = instr[step % instr.Length];

				if (next == 'L')
				{
					currentKey = values[currentKey].Item1;
				}
				else
				{
					currentKey = values[currentKey].Item2;
				}

				step++;
			}

			return step;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			Dictionary<string, (string, string)> values = GetValues(input);

			string[] keys = values.Keys.Where(key => key[key.Length - 1] == 'A').ToArray();
			ulong[] period = new ulong[keys.Length];

			string instr = input[0];
			int step = 0;

			while (keys.Where(key => key[key.Length - 1] != 'Z').Any())
			{
				char next = instr[step % instr.Length];

				for (int i = 0; i < keys.Length; i++)
				{
					if (next == 'L')
					{
						keys[i] = values[keys[i]].Item1;
					}
					else
					{
						keys[i] = values[keys[i]].Item2;
					}
				}

				step++;

				for (int i = 0; i < keys.Length; i++)
				{
					if (keys[i][keys[i].Length - 1] == 'Z')
					{
						period[i] = (ulong)(step / instr.Length);
					}
				}

				if (!period.Where(value => value == 0).Any())
				{
					break;
				}
			}

			return period.Aggregate((a, b) => a * b) * (ulong)instr.Length;
		}

		private Dictionary<string, (string, string)> GetValues(string[] input)
		{
			Dictionary<string, (string, string)> values = new Dictionary<string, (string, string)>();

			for (int i = 2; i < input.Length; i++)
			{
				MatchCollection matches = Regex.Matches(input[i], @"(\w+)");
				string key = matches[0].Value;
				string left = matches[1].Value;
				string right = matches[2].Value;
				values.Add(key, (left, right));
			}

			return values;
		}
	}
}
