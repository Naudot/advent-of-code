using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2021
{
	public class DayThirteen : Day2021
	{
		protected override object ResolveFirstPart(string[] input)
		{
			return Resolve(input, true);
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return Resolve(input, false);
		}

		public int Resolve(string[] input, bool doOnlyFirstStep)
		{
			List<Tuple<int, int>> coords = new List<Tuple<int, int>>();
			List<(char, int)> folds = new List<(char, int)>();

			for (int i = 0; i < input.Length; i++)
			{
				Match matchCoord = Regex.Match(input[i], @"(\d+),(\d+)");
				Match matchFold = Regex.Match(input[i], @"fold along ([x|y])=(\d+)");

				if (matchCoord.Success)
				{
					int x = int.Parse(matchCoord.Groups[1].Value);
					int y = int.Parse(matchCoord.Groups[2].Value);
					coords.Add(new Tuple<int, int>(y, x)); // Height, Width
				}
				else if (matchFold.Success)
				{
					char coord = char.Parse(matchFold.Groups[1].Value);
					int value = int.Parse(matchFold.Groups[2].Value);
					folds.Add((coord, value));
				}
			}

			int step = doOnlyFirstStep ? 1 : folds.Count;
			int maxY = int.MaxValue;
			int maxX = int.MaxValue;

			for (int k = 0; k < step; k++)
			{
				// On fold en 655 en x
				// donc le 656 pass en 654 etc
				// donc le 657 pass en 653 etc
				(char coord, int value) firstFold = folds[k];
				bool isHeight = firstFold.coord == 'y';

				List<Tuple<int, int>> toRemove = new List<Tuple<int, int>>();

				for (int i = 0; i < coords.Count; i++)
				{
					if (coords[i].Item1 > maxY || coords[i].Item2 > maxX)
					{
						continue;
					}

					if (isHeight)
					{
						if (coords[i].Item1 > firstFold.value)
						{
							coords[i] = Tuple.Create(coords[i].Item1 - (coords[i].Item1 - firstFold.value) * 2, coords[i].Item2);
						}
					}
					else
					{
						if (coords[i].Item2 > firstFold.value)
						{
							coords[i] = Tuple.Create(coords[i].Item1, coords[i].Item2 - (coords[i].Item2 - firstFold.value) * 2);
						}
					}

					if (coords.Where(elem => elem.Item1 == coords[i].Item1 && elem.Item2 == coords[i].Item2 && elem.Item1 <= maxY && elem.Item2 <= maxX).Count() == 2)
					{
						if (toRemove.FirstOrDefault(elem => elem.Item1 == coords[i].Item1 && elem.Item2 == coords[i].Item2) == null)
						{
							toRemove.Add(coords.First(elem => elem.Item1 == coords[i].Item1 && elem.Item2 == coords[i].Item2));
						}
					}
				}

				if (isHeight)
				{
					maxY = firstFold.value;
				}
				else
				{
					maxX = firstFold.value;
				}

				for (int i = 0; i < toRemove.Count; i++)
				{
					coords.Remove(toRemove[i]);
				}

				Console.WriteLine();
				for (int l = 0; l < 20; l++)
				{
					for (int m = 0; m < 45; m++)
					{
						if (coords.FirstOrDefault(elem => elem.Item1 == l && elem.Item2 == m && elem.Item1 <= maxY && elem.Item2 <= maxX) != null)
						{
							Console.Write("#");
						}
						else
						{
							Console.Write(".");
						}
					}
					Console.WriteLine();
				}

				Console.WriteLine("Count " + coords.Where(elem => elem.Item1 <= maxY && elem.Item2 <= maxX).Count());
			}

			return coords.Where(elem => elem.Item1 <= maxY && elem.Item2 <= maxX).Count();
		}
	}
}
