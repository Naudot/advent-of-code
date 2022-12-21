using System;
using System.Linq;

namespace AdventOfCode.Year2022
{
	public class DayTwenty : Day2022
	{
		protected override bool DeactivateJIT
		{
			get
			{
				return true;
			}
		}

		public class Number
		{
			public long Value;
			public Number OriginalNext;

			public Number Previous;
			public Number Next;
		}

		protected override object ResolveFirstPart(string[] input)
		{
			Number[] numbers = new Number[input.Length];

			for (int i = 0; i < input.Length; i++)
			{
				Number number = new Number();
				number.Value = long.Parse(input[i]);
				numbers[i] = number;
			}

			for (int i = 0; i < numbers.Length; i++)
			{
				numbers[i].Previous = i == 0 ? numbers[numbers.Length - 1] : numbers[i - 1];
				numbers[i].Next = i == numbers.Length - 1 ? numbers[0] : numbers[i + 1];

				if (i != numbers.Length - 1)
				{
					numbers[i].OriginalNext = numbers[i + 1];
				}
			}

			Number current = numbers[0];

			while (current != null)
			{
				//Console.WriteLine("Moving " + current.Value);

				long valToMove = current.Value % (numbers.Length - 1);

				Number ourNext = current.Next;
				Number ourPrevious = current.Previous;
				Number newNext = null;
				Number newPrevious = null;

				if (valToMove < 0)
				{
					newPrevious = current.Previous;
					for (int i = 0; i < Math.Abs(valToMove); i++)
					{
						newPrevious = newPrevious.Previous;
					}
					newNext = newPrevious.Next;
				}
				else if (valToMove > 0)
				{
					newNext = current.Next;
					for (int i = 0; i < valToMove; i++)
					{
						newNext = newNext.Next;
					}
					newPrevious = newNext.Previous;
				}

				if (valToMove != 0)
				{
					// Once we got our new previous et new next, we plug our current between them
					newNext.Previous = current;
					newPrevious.Next = current;

					current.Previous = newPrevious;
					current.Next = newNext;

					// We need to plug them together when our number moved
					ourNext.Previous = ourPrevious;
					ourPrevious.Next = ourNext;

					//Console.WriteLine(current.Value + " move between " + newPrevious.Value + " and " + newNext.Value);
				}

				current = current.OriginalNext;

				//Number toWrite = numbers[0];
				//for (int i = 0; i < numbers.Length; i++)
				//{
				//	Console.Write(toWrite.Value + ",");
				//	toWrite = toWrite.Next;
				//}
				//Console.WriteLine();
			}

			long result = 0;
			int index = 0;
			current = numbers.First(num => num.Value == 0);

			while (index < 3001)
			{
				if (index == 1000 || index == 2000 || index == 3000)
				{
					result += current.Value;
				}

				current = current.Next;
				index++;
			}

			return result;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			Number[] numbers = new Number[input.Length];

			for (int i = 0; i < input.Length; i++)
			{
				Number number = new Number();
				number.Value = long.Parse(input[i]) * 811589153;
				numbers[i] = number;
			}

			for (int i = 0; i < numbers.Length; i++)
			{
				numbers[i].Previous = i == 0 ? numbers[numbers.Length - 1] : numbers[i - 1];
				numbers[i].Next = i == numbers.Length - 1 ? numbers[0] : numbers[i + 1];

				if (i != numbers.Length - 1)
				{
					numbers[i].OriginalNext = numbers[i + 1];
				}
			}

			Number main = numbers[0];
			Number current = numbers[0];

			for (int mix = 0; mix < 10; mix++)
			{
				current = main;
				while (current != null)
				{
					//Console.WriteLine("Moving " + current.Value);

					long valToMove = current.Value % (numbers.Length - 1);

					Number ourNext = current.Next;
					Number ourPrevious = current.Previous;
					Number newNext = null;
					Number newPrevious = null;

					if (valToMove < 0)
					{
						newPrevious = current.Previous;
						for (int i = 0; i < Math.Abs(valToMove); i++)
						{
							newPrevious = newPrevious.Previous;
						}
						newNext = newPrevious.Next;
					}
					else if (valToMove > 0)
					{
						newNext = current.Next;
						for (int i = 0; i < valToMove; i++)
						{
							newNext = newNext.Next;
						}
						newPrevious = newNext.Previous;
					}

					if (valToMove != 0)
					{
						// Once we got our new previous et new next, we plug our current between them
						newNext.Previous = current;
						newPrevious.Next = current;

						current.Previous = newPrevious;
						current.Next = newNext;

						// We need to plug them together when our number moved
						ourNext.Previous = ourPrevious;
						ourPrevious.Next = ourNext;

						//Console.WriteLine(current.Value + " move between " + newPrevious.Value + " and " + newNext.Value);
					}

					current = current.OriginalNext;
				}

				//Number toWrite = numbers.First(num => num.Value == 0);
				//for (int i = 0; i < numbers.Length; i++)
				//{
				//	Console.Write(toWrite.Value + ",");
				//	toWrite = toWrite.Next;
				//}
				//Console.WriteLine();
			}

			long result = 0;
			int index = 0;
			current = numbers.First(num => num.Value == 0);

			while (index < 3001)
			{
				if (index == 1000 || index == 2000 || index == 3000)
				{
					result += current.Value;
				}

				current = current.Next;
				index++;
			}

			return result;
		}
	}
}
