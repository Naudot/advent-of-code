using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace AdventOfCode.Year2020
{
	public class ClockwiseValue
	{
		public ClockwiseValue Next;
		public int Value;
	}

	public class DayTwentyThree : Day2020
	{
		protected override object ResolveFirstPart()
		{
			List<int> inputs = new List<int>() { 9, 5, 2, 4, 3, 8, 7, 1, 6 };
			//List<int> inputs = new List<int>() { 3, 8, 9, 1, 2, 5, 4, 6 ,7 }; // Exemple
			
			int moveCount = 100;
			int lowestValue = inputs.Min();
			int highestValue = inputs.Max();

			List<ClockwiseValue> values = new List<ClockwiseValue>();

			for (int i = 0; i < inputs.Count; i++)
			{
				values.Add(new ClockwiseValue() { Value = inputs[i] });
			}

			for (int i = 0; i < values.Count; i++)
			{
				if (i == values.Count - 1)
				{
					values[i].Next = values[0];
				}
				else
				{
					values[i].Next = values[i + 1];
				}
			}

			ClockwiseValue currentCup = values[0];

			for (int i = 0; i < moveCount; i++)
			{
				Console.WriteLine();
				Console.WriteLine("-- move " + (i + 1) + " --");

				string cups = "(" + currentCup.Value + ") ";
				ClockwiseValue tmp = currentCup.Next;
				while (tmp != currentCup)
				{
					cups += tmp .Value + " ";
					tmp = tmp.Next;
				}

				Console.WriteLine("cups: " + cups);

				ClockwiseValue firstCup = currentCup.Next;
				ClockwiseValue secondCup = firstCup.Next;
				ClockwiseValue thirdCup = secondCup.Next;

				int firstCupValue = firstCup.Value;
				int secondCupValue = secondCup.Value;
				int thirdCupValue = thirdCup.Value;

				Console.WriteLine("pick up: " + firstCupValue + "," + secondCupValue + "," + thirdCupValue);

				int currentCupValue = currentCup.Value;
				int destinationCupValue = currentCupValue - 1;

				bool isOk = false;
				while (!isOk)
				{
					isOk = true;
					if (destinationCupValue == firstCupValue)
					{
						destinationCupValue--;
						isOk = false;
					}
					else if (destinationCupValue == secondCupValue)
					{
						destinationCupValue--;
						isOk = false;
					}
					else if (destinationCupValue == thirdCupValue)
					{
						destinationCupValue--;
						isOk = false;
					}
					else if (destinationCupValue < lowestValue)
					{
						destinationCupValue = highestValue;
						isOk = false;
					}
				}

				Console.WriteLine("destination: " + destinationCupValue);
				ClockwiseValue destinationCup = values.FirstOrDefault(cup => cup.Value == destinationCupValue);

				currentCup.Next = thirdCup.Next;
				thirdCup.Next = destinationCup.Next;
				destinationCup.Next = firstCup;
				currentCup = currentCup.Next;
			}

			ClockwiseValue departure = values.FirstOrDefault(cup => cup.Value == 1).Next;
			string result = departure.Value.ToString();
			ClockwiseValue tmp2 = departure.Next;
			while (tmp2.Value != 1)
			{
				result += tmp2.Value;
				tmp2 = tmp2.Next;
			}

			return result;
		}

		protected override object ResolveSecondPart()
		{
			return string.Empty;
		}
	}
}
