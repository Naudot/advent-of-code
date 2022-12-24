using System;
using System.Text;

namespace AdventOfCode.Year2015
{
	public class DayTen : Day2015
	{
		protected override bool DeactivateJIT
		{
			get
			{
				return true;
			}
		}

		protected override object ResolveFirstPart(string[] input)
		{
			string customInput = "1113122113";

			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < 40; i++)
			{
				stringBuilder.Clear();
				int repeat = 1;
				for (int j = 0; j < customInput.Length; j++)
				{
					char currentChar = customInput[j];

					if (j + 1 <= customInput.Length - 1 && customInput[j + 1] == currentChar)
					{
						repeat++;
					}
					else
					{
						stringBuilder.Append(repeat.ToString());
						stringBuilder.Append(currentChar);
						repeat = 1;
					}
				}

				customInput = stringBuilder.ToString();
				Console.WriteLine("Computed " + i);
			}

			return customInput.Length;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			string customInput = "1113122113";

			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < 50; i++)
			{
				stringBuilder.Clear();
				int repeat = 1;
				for (int j = 0; j < customInput.Length; j++)
				{
					char currentChar = customInput[j];

					if (j + 1 <= customInput.Length - 1 && customInput[j + 1] == currentChar)
					{
						repeat++;
					}
					else
					{
						stringBuilder.Append(repeat.ToString());
						stringBuilder.Append(currentChar);
						repeat = 1;
					}
				}

				customInput = stringBuilder.ToString();
				Console.WriteLine("Computed " + i);
			}

			return customInput.Length;
		}
	}
}
