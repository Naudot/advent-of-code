namespace AdventOfCode.Year2021
{
	public class DayTen : Day2021
	{
		protected override object ResolveFirstPart(string[] input)
		{
			int result = 0;
			Stack<char> stack = new Stack<char>();

			for (int i = 0; i < input.Length; i++)
			{
				stack.Clear();
				string line = input[i];

				for (int j = 0; j < line.Length; j++)
				{
					char c = line[j];

					if (IsOpeningChar(c))
					{
						stack.Push(c);
					}
					else
					{
						char openingChar = stack.Pop();
						if (MatchOpeningChar(openingChar, c))
						{
							continue;
						}
						else
						{
							Console.WriteLine("Found " + GetValue(c) + " in line " + i);
							result += GetValue(c);
							break;
						}
					}
				}
			}

			return result;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			List<double> results = new List<double>();
			Stack<char> stack = new Stack<char>();

			for (int i = 0; i < input.Length; i++)
			{
				stack.Clear();
				string line = input[i];

				for (int j = 0; j < line.Length; j++)
				{
					char c = line[j];

					if (IsOpeningChar(c))
					{
						stack.Push(c);
					}
					else
					{
						char openingChar = stack.Pop();
						if (!MatchOpeningChar(openingChar, c))
						{
							// Incorrect line
							break;
						}
					}

					// Incomplete line
					if (j == line.Length - 1)
					{
						double score = 0;
						while (stack.Count != 0)
						{
							score *= 5;
							score += GetGoodValue(stack.Pop());
						}
						results.Add(score);
					}
				}
			}

			results.Sort();
			return results[results.Count / 2];
		}

		private int GetValue(char c)
		{
			return c == ')' ? 3 :
				c == ']' ? 57 : 
				c == '}' ? 1197 : 
				c == '>' ? 25137 : -1;
		}

		private double GetGoodValue(char c)
		{
			return c == '(' ? 1 :
				c == '[' ? 2 :
				c == '{' ? 3 :
				c == '<' ? 4 : -1;
		}

		private bool IsOpeningChar(char c)
		{
			return c == '(' || c == '[' || c == '<' || c == '{';
		}

		private bool MatchOpeningChar(char stackChar, char c)
		{
			return (stackChar == '(' && c == ')')
				|| (stackChar == '[' && c == ']')
				|| (stackChar == '<' && c == '>')
				|| (stackChar == '{' && c == '}');
		}
	}
}
