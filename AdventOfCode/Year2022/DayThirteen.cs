using System.Collections.Generic;

namespace AdventOfCode.Year2022
{
	public class DayThirteen : Day2022
	{
		public class Element
		{
			public List<Element> Elements = new List<Element>();
			public int IntVal = int.MinValue;
		}

		protected override object ResolveFirstPart(string[] input)
		{
			int validePairIndexSum = 0;
			int pairIndex = 0;

			for (int i = 0; i < input.Length; i += 3)
			{
				pairIndex++;
				string left = input[i];
				string right = input[i + 1];

				Element leftElement = BuildElement(left, 0).Item1;
				Element rightElement = BuildElement(right, 0).Item1;

				if (ComparePairs(leftElement, rightElement) >= 0)
				{
					validePairIndexSum += pairIndex;
				}
			}

			return validePairIndexSum;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			List<Element> packets = new List<Element>();
			Element firstDivider = new Element() { Elements = new List<Element>() { new Element() { IntVal = 2 } } };
			Element secondDivider = new Element() { Elements = new List<Element>() { new Element() { IntVal = 6 } } };

			packets.Add(firstDivider);
			packets.Add(secondDivider);

			for (int i = 0; i < input.Length; i++)
			{
				if (input[i] == string.Empty)
				{
					continue;
				}

				Element packet = BuildElement(input[i], 0).Item1;
				packets.Add(packet);
			}

			packets.Sort(ComparePairs);
			packets.Reverse();

			return (packets.IndexOf(firstDivider) + 1) * (packets.IndexOf(secondDivider) + 1);
		}

		private int ComparePairs(Element leftElement, Element rightElement)
		{
			if (leftElement.IntVal != int.MinValue && rightElement.IntVal != int.MinValue)
			{
				// Returns 1 if left element is superior
				return rightElement.IntVal.CompareTo(leftElement.IntVal);
			}

			if (leftElement.IntVal != int.MinValue)
			{
				leftElement.Elements.Add(new Element() { IntVal = leftElement.IntVal });
				leftElement.IntVal = int.MinValue;
				return ComparePairs(leftElement, rightElement);
			}

			if (rightElement.IntVal != int.MinValue)
			{
				rightElement.Elements.Add(new Element() { IntVal = rightElement.IntVal });
				rightElement.IntVal = int.MinValue;
				return ComparePairs(leftElement, rightElement);
			}

			for (int i = 0; i < leftElement.Elements.Count; i++)
			{
				// Right side run out of elements
				if (i >= rightElement.Elements.Count)
				{
					return -1;
				}

				int compareValue = ComparePairs(leftElement.Elements[i], rightElement.Elements[i]);
				if (compareValue != 0)
				{
					return compareValue;
				}
			}

			if (leftElement.Elements.Count < rightElement.Elements.Count)
			{
				return 1;
			}

			return 0;
		}

		private (Element, int) BuildElement(string input, int currentIndex)
		{
			Element element = new Element();

			for (int i = currentIndex; i < input.Length; i++)
			{
				char letter = input[i];

				if (letter == '[')
				{
					(Element, int) builtElement = BuildElement(input, i + 1);
					element.Elements.Add(builtElement.Item1);
					i = builtElement.Item2;
				}
				else if (letter == ']')
				{
					return (element, i);
				}
				else if (letter == ',')
				{
					// Do nothing
				}
				else
				{
					string number = string.Empty;
					while (letter != ',' && letter != ']')
					{
						number += letter;
						i++;
						letter = input[i];
					}
					i--;
					element.Elements.Add(new Element() { IntVal = int.Parse(number) });
				}
			}

			return (element, 0);
		}
	}
}
